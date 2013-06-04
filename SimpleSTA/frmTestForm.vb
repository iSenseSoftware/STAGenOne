Option Explicit On
Imports System
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
'Imports Keithley.Ke37XX
'Imports Ivi.Driver.Interop
'Imports Keithley.Ke37XX.Interop
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization
' --------------------------------------------------------------------------------------------------
' frmTestForm is the interface presented to the user during the actual conduct of a sensor test.
' It contains the following major components:
' 1. Current vs. Time chart with traces for each sensor being tested
' 2. Show/Hide menu allowing user to toggle the display of each sensor individually
' 3. A set of elements allowing the user to customize the look of the current vs. time chart
' 4. Basic information about the test (test name, elapsed time, etc)
' 5. A summary of user-noted injections
' 6. Buttons to start and stop test
' 7. Button to note analyte injections
' --------------------------------------------------------------------------------------------------
Public Class frmTestForm
    Dim dblBias As Double
    Dim dblRecordInterval As Double
    Dim strCurrentRange As String
    Dim strFilterType As String
    Dim lngSamples As Long
    Dim lngNPLC As Long
    Dim strName As String
    Dim strSTAID As String
    Dim strDumpDir As String
    Dim strCardConfig As String

    ' All variables prefaced with current- are declared with
    ' module-level scope so that cross-thread references can be made without throwing an exception
    Dim lngCurrentTime As Long ' Timestamp for last gathered reading
    Dim dblCurrentCurrent As Double ' Current for last gathered reading
    Dim intCurrentSlot As Integer ' Card slow for last gathered reading
    Dim intCurrentColumn As Integer ' Card column for last gathered reading
    Dim strCurrentID As String ' SensorID for last gathered reading

    Dim boolIsTestRunning As Boolean = False 'Boolean flag - has the user clicked start and not yet clicked stop
    Dim boolIsTestStopped As Boolean = True ' Boolean flag - has the test actually stopped
    Dim stpTotalTime As New Stopwatch ' Stopwatch to track the total elapsed time in the test
    Dim stpInjectionTime As New Stopwatch ' Stopwatch to track the time since the last noted injection
    ' ----------------------------------
    ' Test Loop thread
    ' ----------------------------------
    ' Sub: MainLoop_DoWork
    ' Handles: DoWork method of MainLoop component
    ' Description:
    ' This is a processing thread that runs independently of the main form events thread, allowing the user
    ' to interact with the frmTestForm interface while this test loop runs in the background
    '
    ' @TODO: If decreasing the measurement interval becomes a priority, several threads can be run in parallel
    '        to distribute the measurement work among several SMU/DMMs
    ' @Notes:
    '       - The 2602A does not appear to understand the enumerated variables spelled out in the user manual.  Integers are used instead
    Private Sub MainLoop_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles MainLoop.DoWork
        Try
            ' --------------------------------------
            ' Declare locally-scoped variables
            ' --------------------------------------
            Dim stpIntervalTimer As New Stopwatch ' Tracks elapsed time for current measurement interval
            Dim intInterval As Integer = cfgGlobal.RecordInterval ' Length of measurement interval in seconds
            Dim dtTheTime As Date ' Represents the timestamp of the current reading
            Dim intMilliseconds As Integer ' Measurement interval in milliseconds
            Dim intCardCount As Integer = cfgGlobal.CardConfig
            Dim intSensorCount As Integer = tfCurrentTestFile.Sensors.Length
            intMilliseconds = intInterval * 1000
            ' To simplify interaction with user interface in main thread and timer threads, disable prevention of
            ' cross-thread calls.  Because of the simplicity of the test sequence and its form this can be done safely
            ' though in general, allowing the program to make cross-thread calls can lead to problematic and unexpected results
            ' such as duplicate assignment of variable values or attempts to simultaneously access the same resource.
            Control.CheckForIllegalCrossThreadCalls = False
            ' Update the instrument front displays
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[1].display.clear()")
            SwitchIOWrite("node[2].display.settext('Test Running')")
            SwitchIOWrite("node[1].display.settext('Test Running')")
            ' Set both SMU channels to DC volts
            SwitchIOWrite("node[2].smua.source.func = 1")
            SwitchIOWrite("node[2].smub.source.func = 1")
            ' NOTE: I had to add a 10ms delat between source meter setting changes to avoid overflowing the System Switch
            ' input buffer.  It appears that the delay in relaying messages from the switch to the SMU through the TSP-Net link
            ' can cause a backlog.
            Delay(10)
            ' Set the bias for both channels based on the value in config
            SwitchIOWrite("node[2].smua.source.levelv = " & cfgGlobal.Bias)
            SwitchIOWrite("node[2].smub.source.levelv = " & cfgGlobal.Bias)
            Delay(10)
            ' @TODO: Range is hard-coded to 1.  This should be changed to adapt to the user-entered config setting
            ' as the specified range for the SMU is up to 40v
            SwitchIOWrite("node[2].smua.source.rangev = 1")
            SwitchIOWrite("node[2].smub.source.rangev = 1")
            Delay(10)
            ' disable autorange for both output channels.  This reflects the settings used in the experimental version of the STA
            SwitchIOWrite("node[2].smua.source.autorangei = 0")
            SwitchIOWrite("node[2].smub.source.autorangei = 0")
            Delay(10)
            ' Set the dynamic range based on the configuration settings
            Select Case cfgGlobal.Range
                Case CurrentRange.one_uA
                    SwitchIOWrite("node[2].smua.source.rangei = .001")
                    SwitchIOWrite("node[2].smub.source.rangei = .001")
                Case CurrentRange.ten_uA
                    SwitchIOWrite("node[2].smua.source.rangei = .01")
                    SwitchIOWrite("node[2].smub.source.rangei = .01")
                Case CurrentRange.hundred_uA
                    SwitchIOWrite("node[2].smua.source.rangei = .1")
                    SwitchIOWrite("node[2].smub.source.rangei = .1")
            End Select
            ' Turn both channels on
            Delay(10)
            SwitchIOWrite("node[2].smua.source.output = 1")
            SwitchIOWrite("node[2].smub.source.output = 1")
            Delay(10)
            ' Configure the DMM
            SwitchIOWrite("node[2].smua.measure.filter.type = " & cfgGlobal.Filter - 1)
            SwitchIOWrite("node[2].smub.measure.filter.type = " & cfgGlobal.Filter - 1)
            Delay(10)
            SwitchIOWrite("node[2].smua.measure.filter.count = " & cfgGlobal.Samples)
            SwitchIOWrite("node[2].smub.measure.filter.count = " & cfgGlobal.Samples)
            Delay(10)
            SwitchIOWrite("node[2].smua.measure.filter.enable = 1")
            SwitchIOWrite("node[2].smub.measure.filter.enable = 1")
            Delay(10)
            SwitchIOWrite("node[2].smua.measure.nplc = " & cfgGlobal.NPLC)
            SwitchIOWrite("node[2].smub.measure.nplc = " & cfgGlobal.NPLC)
            Delay(10)
            ' Clear the non-volatile measurement buffers.  These will be used as transient storage of measured values.
            SwitchIOWrite("node[2].smub.nvbuffer1.clear()")
            SwitchIOWrite("node[2].smub.nvbuffer2.clear()")
            Delay(10)
            ' Set connection rule to "make before break"
            ' @NOTE: This is the setting from the old software.  Should this be changed?
            SwitchIOWrite("node[1].channel.connectrule = 2")
            ' Build a string which will allow us to close all intersections in Row 1
            ' The string is formatted as a series of comma separated codes each of which identify a single switch matrix intersection
            Dim strChannelString As String = ""
            For i = 0 To intSensorCount - 1
                If (i = intSensorCount - 1) Then
                    strChannelString = strChannelString & tfCurrentTestFile.Sensors(i).Slot & 1 & StrPad(CStr(tfCurrentTestFile.Sensors(i).Column), 2) & ""
                Else
                    strChannelString = strChannelString & tfCurrentTestFile.Sensors(i).Slot & 1 & StrPad(CStr(tfCurrentTestFile.Sensors(i).Column), 2) & ","
                End If
            Next
            ' Add codes to the string to identify the necessary backplane relay intersections
            For x As Integer = 1 To intCardCount
                strChannelString = strChannelString & "," & x & "911," & x & "912"
            Next
            Delay(10)
            ' close all relays in row 1
            SwitchIOWrite("node[1].channel.exclusiveclose('" & strChannelString & "')")
            ' Start all timers
            ElapsedTimer.Start() ' Start the form timer component
            stpIntervalTimer.Start() ' Current interval stopwatch
            stpTotalTime.Start() ' total test time stopwatch
            tfCurrentTestFile.TestStart = DateTime.Now()
            ' Run the test loop until the boolTestStop variable returns false (the user clicks Abort)
            boolIsTestRunning = True
            boolIsTestStopped = False
            Do While boolIsTestRunning
                For z = 0 To tfCurrentTestFile.Sensors.Length - 1
                    ' Open relay to row 1
                    SwitchIOWrite("node[1].channel.open('" & tfCurrentTestFile.Sensors(z).Slot & "1" & StrPad(CStr(tfCurrentTestFile.Sensors(z).Column), 2) & "')")
                    Debug.Print("node[1].channel.open('" & tfCurrentTestFile.Sensors(z).Slot & "1" & StrPad(CStr(tfCurrentTestFile.Sensors(z).Column), 2) & "')")
                    ' Close relay to row 2
                    SwitchIOWrite("node[1].channel.close('" & tfCurrentTestFile.Sensors(z).Slot & "2" & StrPad(CStr(tfCurrentTestFile.Sensors(z).Column), 2) & "')")
                    Debug.Print("node[1].channel.close('" & tfCurrentTestFile.Sensors(z).Slot & "2" & StrPad(CStr(tfCurrentTestFile.Sensors(z).Column), 2) & "')")
                    ' Allow settling time
                    Delay(cfgGlobal.SettlingTime)
                    ' Record V and I readings to buffer
                    dtTheTime = DateTime.Now()
                    SwitchIOWrite("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")
                    Dim dblCurrent As Double = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smub.nvbuffer1.n, node[2].smub.nvbuffer1)"))
                    'switchDriver.System.DirectIO.FlushRead()

                    Dim dblVolts As Double = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smub.nvbuffer2.n, node[2].smub.nvbuffer2)"))
                    'switchDriver.System.DirectIO.FlushRead()
                    ' Return relays to their previous state
                    SwitchIOWrite("node[1].channel.open('" & tfCurrentTestFile.Sensors(z).Slot & "2" & StrPad(CStr(tfCurrentTestFile.Sensors(z).Column), 2) & "')")
                    SwitchIOWrite("node[1].channel.close('" & tfCurrentTestFile.Sensors(z).Slot & "1" & StrPad(CStr(tfCurrentTestFile.Sensors(z).Column), 2) & "')")
                    ' Add reading to tfCurrentTestFile.Sensors(z)'s reading array

                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    ' Set module-wide vars for the most recent readings for use by the chart-updating function
                    strCurrentID = tfCurrentTestFile.Sensors(z).SensorID
                    lngCurrentTime = stpTotalTime.ElapsedMilliseconds
                    dblCurrentCurrent = dblCurrent * 10 ^ 9
                    ' Report progress so the chart can be updated
                    MainLoop.ReportProgress(0)
                    ' Update system info file with switch count
                    tsInfoFile.AddSwitchEvent(tfCurrentTestFile.Sensors(z).Slot, 4)
                Next
                ' Record the voltage and current across all sensors
                Delay(cfgGlobal.SettlingTime)
                Dim dtAllTheTime As DateTime = DateTime.Now()
                SwitchIOWrite("node[2].smua.measure.iv(node[2].smua.nvbuffer1, node[2].smua.nvbuffer2)")
                Dim dblAllCurrent As Double = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smua.nvbuffer1.n, node[2].smua.nvbuffer1)"))
                'switchDriver.System.DirectIO.FlushRead()
                Dim dblAllVolts As Double = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smua.nvbuffer2.n, node[2].smua.nvbuffer2)"))
                'switchDriver.System.DirectIO.FlushRead()
                tfCurrentTestFile.addFullCircuitReading(dtAllTheTime, dblAllCurrent, dblAllVolts)
                MainLoop.ReportProgress(10)
                If (stpIntervalTimer.ElapsedMilliseconds > intMilliseconds) Then
                    MsgBox("Could not finish measurements within injection interval specified")
                    boolIsTestRunning = False
                    boolIsTestStopped = True
                End If
                Dim intLast As Integer
                Do Until stpIntervalTimer.ElapsedMilliseconds >= intMilliseconds
                    ' do nothing.  This is to ensure that the interval elapses before another round of measurements

                    If Not boolIsTestRunning Then
                        If intLast = 0 And (intMilliseconds - stpIntervalTimer.ElapsedMilliseconds) / 1000 = 0 Then
                            btnStartTest.Text = "Test Complete"
                        Else
                            If Not ((intMilliseconds - stpIntervalTimer.ElapsedMilliseconds) / 1000 = intLast) Then
                                intLast = (intMilliseconds - stpIntervalTimer.ElapsedMilliseconds) / 1000
                                btnStartTest.Text = "Ending In " & intLast
                            End If
                        End If
                    End If
                Loop
                stpIntervalTimer.Restart()
            Loop
            ' After test is complete, reset state
            tfCurrentTestFile.TestEnd = DateTime.Now()
            SetLastTestForSysInfo()
            tsInfoFile.writeToFile(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)
            tfCurrentTestFile.WriteToFile()
            btnStartTest.Text = "Test Complete"
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[1].display.clear()")
            SwitchIOWrite("node[2].display.settext('Standby')")
            SwitchIOWrite("node[1].display.settext('Standby')")
            boolIsTestStopped = True
            stpTotalTime.Stop()
            SwitchIOWrite("node[2].smub.source.output = 0 node[2].smua.source.output = 0")
        Catch ex As COMException
            ComExceptionHandler(ex)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    ' --------------------------------------------
    ' Timer loop threads
    ' ---------------------------------------------
    ' Timer1_Tick and InjectionTime_Tick are both triggered every second by the Timer1 and InjectionTimer components in the user form.
    ' There are corresponding stopwatches that are managed within the test control loop
    ' and these are referenced within the Tick events to update the elapsed time shown to the user.

    ' Name: ElapsedTimer_Tick()
    ' Handles: Tick event for ElapsedTimer
    ' Description: The ElapsedTimer triggers the Tick event every second.  This sub runs when this event is triggered.  
    '           It grabs the the time elapsed in the stpTotalTime stopwatch, parses it into human-readable format and updates
    '           the user interface
    Private Sub ElapsedTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ElapsedTimer.Tick
        Try
            ' Update the total time timer in the user interface
            txtTime.Text = "Total Time: " & StrPad(stpTotalTime.Elapsed.Hours, 2) & ":" & StrPad(stpTotalTime.Elapsed.Minutes, 2) & ":" & StrPad(stpTotalTime.Elapsed.Seconds, 2)
            tfCurrentTestFile.TestLength = stpTotalTime.Elapsed.TotalSeconds
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: InjectionTimer_Tick()
    ' Handles: Tick event for InjectionTimer
    ' Description: The InjectionTimer triggers the Tick event every second.  This sub runs when this event is triggered.  
    '           It grabs the the time elapsed in the stpInjectionTime stopwatch, parses it into human-readable format and updates
    '           the user interface
    Private Sub InjectionTimer_Tick(sender As Object, e As EventArgs) Handles InjectionTimer.Tick
        Try
            ' Updat the time since injection in the user interface
            If Not boolIsTestStopped Then
                txtTimeSinceInjection.Text = "Time Since Injection: " & StrPad(stpInjectionTime.Elapsed.Hours, 2) & ":" & StrPad(stpInjectionTime.Elapsed.Minutes, 2) & ":" & StrPad(stpInjectionTime.Elapsed.Seconds, 2)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' --------------------------------------------
    ' Event Handlers
    ' --------------------------------------------
    ' Name: MainLoop_ProgressChanged()
    ' Handles: MainLoop.ReportProgress
    ' This function is triggered by the MainLoop.ReportProgress method.  This runs in the same thread as the main form elements
    ' so we use this function to update user interface elements based on data from the MainLoop thread.
    Private Sub MainLoop_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles MainLoop.ProgressChanged
        Try
            ' Typically the ProgressPercentage is used to update the user on the progress through a lengthy task
            ' in this case I use it as a flag to tell this sub whether to update the test chart and test file
            ' 10 implies that the test chart should be updated
            ' 0 implies that a single point should be added to a particular series
            If (e.ProgressPercentage = 10) Then
                If Not (TestChart.ChartAreas(0).AxisY.ScaleView.IsZoomed Or TestChart.ChartAreas(0).AxisX.ScaleView.IsZoomed) Then
                    TestChart.ChartAreas(0).AxisX.Interval = TestChart.ChartAreas(0).AxisX.Maximum \ 10
                    TestChart.ChartAreas(0).AxisY.Interval = Math.Round(TestChart.ChartAreas(0).AxisY.Maximum / 10, 1)
                Else
                    ' do nothing
                End If
                TestChart.Update()
                tfCurrentTestFile.WriteToFile()
                tsInfoFile.writeToFile(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)
            Else
                ' Note that the  \ division operator is used instead of  / to force rounding down to an integer.  This
                ' helps improve readability of the graph
                TestChart.Series(strCurrentID).Points.AddXY(lngCurrentTime \ 1000, Math.Round(dblCurrentCurrent, 2))
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    
    ' Name: TestForm_FormClosing()
    ' Handles: User closes frmTestForm
    ' Description: Behavior is dependent on the state of the test.  If the MainLoop is still running, it warns the user they cannot close the 
    ' form while the test is running.  Otherwise it closes the test form and resets the switchdriver and its attendant UI element
    ' indicating IO has been broken.
    Private Sub TestForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            ' Close the instrument connection when exiting the test form
            If boolIsTestStopped Then
                If (switchDriver.Connected) Then
                    switchDriver.Close()
                    frmMain.chkIOStatus.Checked = False
                End If
            Else
                e.Cancel = True
                MsgBox("Cannot close test form while test is running.  Stop test and close form.", vbOKOnly)
            End If
        Catch comEx As COMException
            ComExceptionHandler(comEx)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: TestForm_Load()
    ' Handles: The frmTestForm is opened
    ' Description: Enabled the start and note injection buttons, populates the form with test specific data
    ' and sets the display text on the SMU to indicate readiness to perform a test
    Private Sub TestForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            btnStartTest.Show()
            btnNoteInjection.Show()
            ' Set the background worker to report progress so that it can make cross-thread communications to the chart updater
            MainLoop.WorkerReportsProgress = True
            prepareForm()
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[2].display.settext('Ready to test')")
        Catch ex As COMException
            ComExceptionHandler(ex)
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    ' Name: btnNoteInjection_Click
    ' Handles: User clicks 'Note Injection' button
    ' Description: Adds a new timestamp to the TestFile Injections array and resets the stpInjectionTime stopwatch.
    Private Sub btnNoteInjection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoteInjection.Click
        ' Add the current time to the test file injections array
        Try
            Dim timestamp As DateTime = DateTime.Now()
            tfCurrentTestFile.addInjection(timestamp)
            Dim txtNewInjection As New Label
            txtNewInjection.Text = StrPad(stpTotalTime.Elapsed.Hours, 2) & ":" & StrPad(stpTotalTime.Elapsed.Minutes, 2) & ":" & StrPad(stpTotalTime.Elapsed.Seconds, 2)
            flwInjections.Controls.Add(txtNewInjection)
            If (stpInjectionTime.IsRunning) Then
                stpInjectionTime.Restart()
            Else
                stpInjectionTime.Start()
            End If
            MsgBox("Injection noted", vbOKOnly)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Sub: btnStartTest_Click()
    ' Handles: User clicks the Start/Stop button
    ' If a test is not already running, this sub starts the test thread (BackgroundWorker1)
    ' If the test is running it initiates the test stop sequence by setting the boolIsTestRunning
    ' flag to false and disabling further user interaction with the start/stop button
    Private Sub btnStartTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartTest.Click
        Try
            If (boolIsTestRunning) Then
                boolIsTestRunning = False
                btnStartTest.Text = "Ending Test"
                btnStartTest.Enabled = False
                btnNoteInjection.Enabled = False
            Else
                boolIsTestRunning = True
                btnStartTest.Text = "Stop"
                MainLoop.RunWorkerAsync()
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' --------------------------------------------------------
    ' Zoom and Chart updating functions and event handlers
    ' --------------------------------------------------------
    ' Name: TestChart_AxisViewChanged()
    Private Sub TestChart_AxisViewChanged(sender As Object, e As ViewEventArgs) Handles TestChart.AxisViewChanged
        Dim dblMin As Double
        dblMin = TestChart.ChartAreas(0).AxisX.ScaleView.Position
        If dblMin < 0 Then
            TestChart.ChartAreas(0).AxisX.ScaleView.Position = 0
        Else
            TestChart.ChartAreas(0).AxisX.ScaleView.Position = dblMin \ 1
        End If
        dblMin = TestChart.ChartAreas(0).AxisY.ScaleView.Position
        If dblMin < 0 Then
            TestChart.ChartAreas(0).AxisY.ScaleView.Position = 0
        End If
        TestChart.ChartAreas(0).AxisY.ScaleView.Position = Math.Round(TestChart.ChartAreas(0).AxisY.ScaleView.Position, 1)
        ' Adjust the interval so there are 10 lines shown, and round this interval to an integer
        TestChart.ChartAreas(0).AxisX.Interval = TestChart.ChartAreas(0).AxisX.ScaleView.Size \ 10
        ' Adjust the y interval so 10 lines are shown and round interval to the nearest 0.1
        TestChart.ChartAreas(0).AxisY.Interval = Math.Round(TestChart.ChartAreas(0).AxisY.ScaleView.Size / 10, 1)
    End Sub
    ' This method requires the series names to be the same as their legend entries
    Private Sub TestChart_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TestChart.MouseDown
        ' Call hit test method
        Try
            Dim result As HitTestResult = TestChart.HitTest(e.X, e.Y)
            If result.ChartElementType <> ChartElementType.DataPoint Then
                If (result.ChartElementType <> ChartElementType.LegendItem) Then
                    Return
                End If
            End If
            Dim strSeriesName As String = result.Series.Name
            For Each aSeries As Series In TestChart.Series
                aSeries.BorderWidth = 2
            Next
            For Each aLegend As Legend In TestChart.Legends
                aLegend.BorderWidth = 2
            Next
            TestChart.Legends(strSeriesName).BorderWidth = 6
            TestChart.Series(strSeriesName).BorderWidth = 6
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub btnZoomReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomReset.Click
        Try
            TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
            TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
            TestChart.ChartAreas(0).AxisX.Maximum = Double.NaN
            TestChart.ChartAreas(0).AxisX.Minimum = 0
            TestChart.ChartAreas(0).AxisY.Maximum = Double.NaN
            TestChart.ChartAreas(0).AxisY.Minimum = 0
            txtXMax.Text = ""
            txtYMax.Text = ""
            txtXMin.Text = ""
            txtYMin.Text = ""
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub chkZoomEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZoomEnabled.CheckedChanged
        Try
            If (chkZoomEnabled.Checked) Then
                With TestChart.ChartAreas(0)
                    .AxisX.ScaleView.Zoomable = True
                    .AxisY.ScaleView.Zoomable = True
                    .AxisX.ScaleView.MinSizeType = DateTimeIntervalType.Number
                    .AxisX.ScaleView.MinSize = cfgGlobal.RecordInterval * 3
                    .AxisY.ScaleView.MinSizeType = DateTimeIntervalType.Number
                    .AxisY.ScaleView.MinSize = 0.5
                    .AxisY.RoundAxisValues()
                    .AxisX.RoundAxisValues()
                    .CursorY.IsUserSelectionEnabled = True
                    .CursorX.IsUserSelectionEnabled = True
                End With

            Else
                With TestChart.ChartAreas(0)
                    .AxisX.ScaleView.Zoomable = False
                    .AxisY.ScaleView.Zoomable = False
                    .CursorY.IsUserSelectionEnabled = False
                    .CursorX.IsUserSelectionEnabled = False
                End With
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Private Sub chkScrollEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScrollEnabled.CheckedChanged
        Try
            If (chkZoomEnabled.Checked) Then
                With TestChart.ChartAreas(0)
                    .CursorX.AutoScroll = True
                    .CursorY.AutoScroll = True
                End With
            Else
                With TestChart.ChartAreas(0)
                    .CursorX.AutoScroll = False
                    .CursorY.AutoScroll = False
                End With
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Private Sub btnZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomOut.Click
        Try
            TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Try
            If Not (txtXMax.Text = "") Then
                TestChart.ChartAreas(0).AxisX.Maximum = txtXMax.Text
            Else
                TestChart.ChartAreas(0).AxisX.Maximum = Double.NaN
            End If
            If Not (txtYMax.Text = "") Then
                TestChart.ChartAreas(0).AxisY.Maximum = txtYMax.Text
            End If
            If Not (txtXMin.Text = "") Then
                TestChart.ChartAreas(0).AxisX.Minimum = txtXMin.Text
            End If
            If Not (txtYMin.Text = "") Then
                TestChart.ChartAreas(0).AxisY.Minimum = txtYMin.Text
            End If
            TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
            TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Private Sub showAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In TestChart.Series
                aSeries.Enabled = True
                aSeries.IsVisibleInLegend = True
            Next
            For Each ctrl In HideShowSensors.Controls
                If ctrl.GetType.ToString = "System.Windows.Forms.CheckBox" Then
                    ctrl.Checked = True
                End If
            Next
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub hideAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In TestChart.Series
                aSeries.Enabled = False
                aSeries.IsVisibleInLegend = False
            Next
            For Each ctrl In HideShowSensors.Controls
                If ctrl.GetType.ToString = "System.Windows.Forms.CheckBox" Then
                    ctrl.Checked = False
                End If
            Next
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' ----------------------------------------------
    ' Utility functions
    ' ----------------------------------------------
    Private Sub prepareForm()
        Try
            ' Clear the default or previous series and legends from the test chart
            TestChart.Series.Clear()
            TestChart.Legends.Clear()
            txtTestName.Text = "Test Name: " & tfCurrentTestFile.Name
            txtOperator.Text = "Operator: " & tfCurrentTestFile.OperatorID
            ' Configure the test chart
            With TestChart.ChartAreas(0)
                .CursorX.AutoScroll = False
                .CursorY.AutoScroll = False
                .CursorX.IsUserEnabled = True
                .CursorY.IsUserEnabled = True
                .CursorX.Interval = 0
                .CursorY.Interval = 0
                .AxisX.ScaleView.MinSize = 0
                .AxisY.ScaleView.MinSize = 0
                .AxisY.Minimum = 0
                .AxisX.Minimum = 0
                .AxisX.Title = "Elapsed Time (s)"
                .AxisY.Title = "Current (nA)"
                ' Setting IsMarginVisible to false increases the accuracy of deep zooming.  If this is true then zooms are padded
                ' and do not show the actual area selected
                .AxisX.IsMarginVisible = False
                .AxisY.IsMarginVisible = False
                .Name = "Main"
            End With
            ' Populate the chart series and legend
            For Each ssrSensor As Sensor In tfCurrentTestFile.Sensors
                TestChart.Series.Add(ssrSensor.SensorID)
                With TestChart.Series(ssrSensor.SensorID)
                    .ChartType = SeriesChartType.Line
                    .BorderWidth = 2
                    ' other properties go here later
                End With
                TestChart.Legends.Add(ssrSensor.SensorID)
                With TestChart.Legends(ssrSensor.SensorID)
                    .Title = ssrSensor.SensorID
                    .BorderColor = Color.Black
                    .BorderWidth = 2
                    .LegendStyle = LegendStyle.Column
                    .DockedToChartArea = "Main"
                    .IsDockedInsideChartArea = True
                End With
                Dim chkNewBox As New CheckBox
                With chkNewBox
                    .Width = 140
                    .Name = ssrSensor.SensorID
                    .Text = ssrSensor.SensorID
                    .Enabled = True
                    .Visible = True
                    .Checked = True
                    AddHandler chkNewBox.Click, AddressOf UpdateTraces
                End With
                HideShowSensors.Controls.Add(chkNewBox)
            Next
            Dim btnShowAllButton As New Button
            With btnShowAllButton
                .Name = "btnShowAllSensors"
                .Text = "Show All"
                .Font = New Font("Microsoft Sans Serif", 12)
                .AutoSize = True
            End With
            HideShowSensors.Controls.Add(btnShowAllButton)
            Dim btnHideAllButton As New Button
            With btnHideAllButton
                .Name = "btnHideAllSensors"
                .Text = "Hide All"
                .Font = New Font("Microsoft Sans Serif", 12)
                .AutoSize = True
            End With
            HideShowSensors.Controls.Add(btnHideAllButton)
            AddHandler btnShowAllButton.Click, AddressOf showAllButton_Click
            AddHandler btnHideAllButton.Click, AddressOf hideAllButton_Click
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub UpdateTraces(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If (sender.Checked) Then
                TestChart.Series(sender.name).Enabled = True
                TestChart.Series(sender.name).IsVisibleInLegend = True
            Else
                TestChart.Series(sender.name).Enabled = False
                TestChart.Series(sender.name).IsVisibleInLegend = False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub GenerateGlucoseTestFile()
        Try
            Dim dtTheTime As Date
            Dim dblCurrent As Double
            Dim dblVolts As Double
            Dim i As Long
            Dim dtRefTime As Date

            dblVolts = 0.65
            tfCurrentTestFile.TestStart = DateTime.Now()
            dtTheTime = tfCurrentTestFile.TestStart
            dtRefTime = dtTheTime
            For i = 0 To 374
                dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
            Next
            tfCurrentTestFile.AddInjection(dtTheTime)
            For i = 0 To 59
                dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
            Next
            tfCurrentTestFile.AddInjection(dtTheTime)
            For i = 0 To 59
                dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
            Next
            tfCurrentTestFile.AddInjection(dtTheTime)
            For i = 0 To 59
                dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
            Next
            tfCurrentTestFile.AddInjection(dtTheTime)

            ' Run the test loop until the boolTestStop variable returns false (the user clicks Abort)
            For z = 0 To tfCurrentTestFile.Sensors.Length - 1
                dblCurrent = 0.000000001
                dtTheTime = dtRefTime
                For i = 0 To 374
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
                '    tfCurrentTestFile.addInjection(theTime)
                dblCurrent = 0.0000000022
                For i = 0 To 59
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
                '   tfCurrentTestFile.addInjection(theTime)
                dblCurrent = 0.000000005
                For i = 0 To 59
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
                '  tfCurrentTestFile.addInjection(theTime)
                dblCurrent = 0.00000001
                For i = 0 To 59
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
                ' tfCurrentTestFile.addInjection(theTime)
                dblCurrent = 0.000000016
                For i = 0 To 59
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
            Next
            tfCurrentTestFile.WriteToFile()
        Catch ex As COMException
            ComExceptionHandler(ex)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub GenerateAPAPTestFile()
        Try
            Dim dtTheTime As Date
            Dim dblCurrent As Double
            Dim dblVolts As Double
            Dim i As Long
            Dim dtRefTime As Date

            dblVolts = 0.65
            tfCurrentTestFile.TestStart = DateTime.Now()
            dtTheTime = tfCurrentTestFile.TestStart
            dtRefTime = dtTheTime
            For i = 0 To 75
                dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
            Next
            tfCurrentTestFile.AddInjection(dtTheTime)

            ' Run the test loop until the boolTestStop variable returns false (the user clicks Abort)
            For z = 0 To tfCurrentTestFile.Sensors.Length - 1
                dblCurrent = 0.000000001
                dtTheTime = dtRefTime
                For i = 0 To 75
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
                '    tfCurrentTestFile.addInjection(theTime)
                dblCurrent = 0.0000000033
                For i = 0 To 59
                    tfCurrentTestFile.Sensors(z).AddReading(Reading.ReadingFactory(dtTheTime, dblCurrent, dblVolts))
                    dtTheTime = DateAdd(DateInterval.Second, 8, dtTheTime)
                Next
            Next
            tfCurrentTestFile.WriteToFile()
        Catch comEx As COMException
            ComExceptionHandler(comEx)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
End Class