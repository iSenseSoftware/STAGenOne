Module modTest
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
End Module
