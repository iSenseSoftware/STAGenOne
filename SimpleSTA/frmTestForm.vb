Imports System
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports Keithley.Ke37XX
Imports Ivi.Driver.Interop
Imports Keithley.Ke37XX.Interop
Imports System.Runtime.InteropServices

Public Class frmTestForm
    Dim lngSettling As Long = 20 ' The default settling time in milliseconds
    Dim strConfigFileLocation As String
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
    Dim currentTime As Long
    Dim currentCurrent As Double
    Dim currentSlot As Integer
    Dim currentColumn As Integer
    Dim currentID As String
    Dim boolIsTestRunning As Boolean
    Dim boolOpenTest As Boolean = False
    Dim strOpenFileName As String
    Dim boolColorsSet As Boolean = False

    Private Sub btnStartTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartTest.Click
        Try
            If (boolIsTestRunning) Then
                boolIsTestRunning = False
                btnStartTest.Text = "Test Complete"
                btnStartTest.Enabled = False
            Else
                boolIsTestRunning = True
                btnStartTest.Text = "Stop"
                BackgroundWorker1.RunWorkerAsync()
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub TestForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            ' Close the instrument connection when exiting the test form
            If (switchDriver.Initialized) Then
                switchDriver.Close()
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub OpenFile(ByVal openFileName As String)
        Try
            ' Set the testFile to the file to be opened
            currentTestFile = TestFile.testFileFactory(openFileName)
            ' If a test is still running, terminate it
            boolIsTestRunning = False
            ' Hide the Start/Stop buttons
            btnStartTest.Hide()
            btnNoteInjection.Hide()
            prepareForm()
            populateSeries()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub populateSeries()
        Try
            For Each aSensor As Sensor In currentTestFile.Sensors
                For Each aReading As Reading In aSensor.Readings
                    TestChart.Series(aSensor.SensorID).Points.AddXY(aReading.Time, aReading.Current)
                Next
            Next
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub prepareForm()
        Try
            ' Clear the default or previous series and legends from the test chart
            TestChart.Series.Clear()
            TestChart.Legends.Clear()
            ' Configure the test chart
            With TestChart.ChartAreas(0)
                .AxisY.Interval = 0
                Dim aGrid As New Grid
                aGrid.Interval = 0.5
                .AxisY.MajorGrid = aGrid
                Dim anotherGrid As New Grid
                anotherGrid.Interval = 0.1
                .AxisY.MinorGrid = anotherGrid
                .CursorX.AutoScroll = False
                .CursorY.AutoScroll = False
                .CursorX.IsUserEnabled = True
                .CursorY.IsUserEnabled = True
                .CursorX.Interval = 0
                .CursorY.Interval = 0
                .AxisX.ScaleView.MinSize = 0
                .AxisY.ScaleView.MinSize = 0
                .AxisX.Title = "Elapsed Time (s)"
                .AxisY.Title = "Current (nA)"
                ' Setting IsMarginVisible to false increases the accuracy of deep zooming.  If this is true then zooms are padded
                ' and do not show the actual area selected
                .AxisX.IsMarginVisible = False
                .AxisY.IsMarginVisible = False
                .Name = "Main"
            End With
            ' Populate the chart series and legend
            For Each aSensor As Sensor In currentTestFile.Sensors
                TestChart.Series.Add(aSensor.SensorID)
                With TestChart.Series(aSensor.SensorID)
                    .ChartType = SeriesChartType.Line
                    .BorderWidth = 2
                    ' other properties go here later
                End With
                TestChart.Legends.Add(aSensor.SensorID)
                With TestChart.Legends(aSensor.SensorID)
                    .Title = aSensor.SensorID
                    .BorderColor = Color.Black
                    .BorderWidth = 2
                    .LegendStyle = LegendStyle.Column
                    .DockedToChartArea = "Main"
                    .IsDockedInsideChartArea = True
                End With
                Dim newBox As New CheckBox
                With newBox
                    .Name = aSensor.SensorID
                    .Text = aSensor.SensorID
                    .Enabled = True
                    .Visible = True
                    .Checked = True
                    AddHandler newBox.Click, AddressOf UpdateTraces
                End With
                HideShowSensors.Controls.Add(newBox)
            Next
            Dim showAllButton As New Button
            With showAllButton
                .Name = "btnShowAllSensors"
                .Text = "Show All"
                .Font = New Font("Microsoft Sans Serif", 12)
                .AutoSize = True
            End With
            HideShowSensors.Controls.Add(showAllButton)
            Dim hideAllButton As New Button
            With hideAllButton
                .Name = "btnHideAllSensors"
                .Text = "Hide All"
                .Font = New Font("Microsoft Sans Serif", 12)
                .AutoSize = True
            End With
            HideShowSensors.Controls.Add(hideAllButton)
            AddHandler showAllButton.Click, AddressOf showAllButton_Click
            AddHandler hideAllButton.Click, AddressOf hideAllButton_Click
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub TestForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            boolIsTestRunning = False
            btnStartTest.Show()
            btnNoteInjection.Show()
            ' Set the background worker to report progress so that it can make cross-thread communications to the chart updater
            BackgroundWorker1.WorkerReportsProgress = True
            Dim options As String
            ' An option string must be explicitly declared or the driver throws a COMException.  This may be fixed by firmware upgrades
            options = "QueryInstStatus=true, RangeCheck=true, Cache=true, Simulate=false, RecordCoercions=false, InterchangeCheck=false"
            switchDriver.Initialize(config.Address, False, False, options)
            If (switchDriver.Initialized) Then
                directIOWrapper("print(localnode.serialno)")
                Dim serialNo As String
                serialNo = switchDriver.System.DirectIO.ReadString()
                currentTestFile.SwitchSerial = serialNo
                'directIOWrapper("print(node[2].serialno)")
                'serialNo = switchDriver.System.DirectIO.ReadString()
                'currentTestFile.SourceMeterSerial = serialNo
                prepareForm()
            Else
                Throw New Exception("Unable to initialize driver.  Verify configuration settings are correct")
            End If
         
            ' Reset the TSPLink network between the devices
            switchDriver.TspLink.Reset()
            ' Clear the source meter display and update the user
            directIOWrapper("node[2].display.clear()")
            directIOWrapper("node[2].display.settext('Ready to test')")

        Catch ex As COMException
            MsgBox(ex.ErrorCode & " " & ex.ToString)
            Dim errMessage As String = ""
            switchDriver.Utility.ErrorQuery(ex.ErrorCode, errMessage)
            MsgBox(errMessage)
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    ' BackgroundWorker1 is the thread responsible for the instrument control loop.
    ' This runs in the background so that the test chart can be updated and the user can continue to make inputs while
    ' Sensor data is gathered.

    ' @TODO: If decreasing the measurement interval becomes a priority, several threads can be run in parallel
    '        to distribute the measurement work among several SMU/DMMs
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            ' Start with all intersections open
            switchDriver.Channel.OpenAll()
            ' set both SMU channels to DC volts
            ' Note: The 2602A does not appear to understand the enum variables spelled out in the user manual.  Integers are used instead
            directIOWrapper("node[2].smua.source.func = 1")
            directIOWrapper("node[2].smub.source.func = 1")
            ' Set the bias for both channels based on the value in config
            directIOWrapper("node[2].smua.source.levelv = " & config.Bias)
            directIOWrapper("node[2].smub.source.levelv = " & config.Bias)
            ' Range is hard-coded to 1.  Does this need to be a config setting in the future?
            directIOWrapper("node[2].smua.source.rangev = 1")
            directIOWrapper("node[2].smub.source.rangev = 1")
            ' disable autorange for both output channels
            directIOWrapper("node[2].smua.source.autorangei = 0")
            directIOWrapper("node[2].smub.source.autorangei = 0")
            Select Case config.Range
                Case CurrentRange.one_uA
                    directIOWrapper("node[2].smua.source.rangei = .001")
                    directIOWrapper("node[2].smub.source.rangei = .001")
                Case CurrentRange.ten_uA
                    directIOWrapper("node[2].smua.source.rangei = .01")
                    directIOWrapper("node[2].smub.source.rangei = .01")
                Case CurrentRange.hundred_uA
                    directIOWrapper("node[2].smua.source.rangei = .1")
                    directIOWrapper("node[2].smub.source.rangei = .1")
            End Select
            ' Turn both channels on
            directIOWrapper("node[2].smua.source.output = 1")
            directIOWrapper("node[2].smub.source.output = 1")

            ' Set connection rule to "make before break"
            ' @TODO: This is the setting from the old software.  Should this be changed?
            directIOWrapper("node[1].channel.connectrule = 2")
            ' build the string for the exclusiveclose function
            Dim channelString As String = ""
            Dim sensorCount As Integer = currentTestFile.Sensors.Length
            For i = 0 To sensorCount - 1
                If (i = sensorCount - 1) Then
                    channelString = channelString & currentTestFile.Sensors(i).Slot & 1 & strPad(CStr(currentTestFile.Sensors(i).Column), 2) & ""
                Else
                    channelString = channelString & currentTestFile.Sensors(i).Slot & 1 & strPad(CStr(currentTestFile.Sensors(i).Column), 2) & ","
                End If
            Next

            ' Add backplane relay channels as well
            ' @TODO: After hardware change to utilize backplane for card 1, the cardCount > 1 condition must be removed
            Dim cardCount As Integer = currentTestFile.Sensors.Length / 16
            If (cardCount > 1) Then
                For x As Integer = 1 To cardCount
                    channelString = channelString & "," & x & "911," & x + "912"
                Next
            End If
            directIOWrapper("node[2].display.reset()")
            ' close all relays in row 1
            directIOWrapper("node[1].channel.exclusiveclose('" & channelString & "')")
            ' Configure the DMM
            directIOWrapper("node[2].smua.measure.filter.type = " & config.Filter)
            directIOWrapper("node[2].smub.measure.filter.type = " & config.Filter)
            directIOWrapper("node[2].smua.measure.filter.count = " & config.Samples)
            directIOWrapper("node[2].smub.measure.filter.count = " & config.Samples)
            directIOWrapper("node[2].smua.measure.filter.enable = 1")
            directIOWrapper("node[2].smub.measure.filter.enable = 1")
            directIOWrapper("node[2].smua.measure.nplc = " & config.NPLC)
            directIOWrapper("node[2].smub.measure.nplc = " & config.NPLC)
            ' Clear the non-volatile measurement buffers
            directIOWrapper("node[2].smub.nvbuffer1.clear()")
            directIOWrapper("node[2].smub.nvbuffer2.clear()")

            ' timer is reset after each loop, totalTime is used for the x-axis of the test chart
            Dim timer As New Stopwatch
            Dim totalTime As New Stopwatch
            Dim interval As Integer = config.RecordInterval
            Dim theTime As Date
            timer.Start()
            totalTime.Start()
            currentTestFile.TestStart = DateTime.Now()
            ' Run the test loop until the boolTestStop variable returns false (the user clicks Abort)
            Do While boolIsTestRunning
                For z = 0 To currentTestFile.Sensors.Length - 1
                    ' 1. Open relay to row 1
                    directIOWrapper("node[1].channel.open('" & currentTestFile.Sensors(z).Slot & "1" & strPad(CStr(currentTestFile.Sensors(z).Column), 2) & "')")
                    ' 2. Close relay to row 2
                    directIOWrapper("node[1].channel.close('" & currentTestFile.Sensors(z).Slot & "2" & strPad(CStr(currentTestFile.Sensors(z).Column), 2) & "')")
                    ' 3. Allow settling time
                    Delay(lngSettling)
                    ' 4. Record V and I readings to buffer
                    theTime = DateTime.Now()
                    directIOWrapper("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")
                    directIOWrapper("printbuffer(1, node[2].smub.nvbuffer1.n, node[2].smub.nvbuffer1)")
                    Dim current As Double = CDbl(switchDriver.System.DirectIO.ReadString())
                    switchDriver.System.DirectIO.FlushRead()

                    directIOWrapper("printbuffer(1, node[2].smub.nvbuffer2.n, node[2].smub.nvbuffer2)")
                    Dim volts As Double = CDbl(switchDriver.System.DirectIO.ReadString())
                    ' 5. Return relays to their previous state
                    directIOWrapper("node[1].channel.open('" & currentTestFile.Sensors(z).Slot & "2" & strPad(CStr(currentTestFile.Sensors(z).Column), 2) & "')")
                    directIOWrapper("node[1].channel.close('" & currentTestFile.Sensors(z).Slot & "1" & strPad(CStr(currentTestFile.Sensors(z).Column), 2) & "')")
                    ' 6. Add reading to currentTestFile.Sensors(z)'s reading array

                    currentTestFile.Sensors(z).addReading(theTime, current, volts)
                    ' Set module-wide vars for the most recent readings for use by the chart-updating function
                    currentID = currentTestFile.Sensors(z).SensorID
                    currentTime = totalTime.ElapsedMilliseconds
                    currentCurrent = current * 10 ^ 9
                    ' Report progress so the chart can be updated
                    BackgroundWorker1.ReportProgress(0)
                Next
                BackgroundWorker1.ReportProgress(10)
                Do Until timer.ElapsedMilliseconds >= interval * 1000
                    ' do nothing.  This is to ensure that the interval elapses before another round of measurements
                Loop
                timer.Restart()
            Loop
            totalTime.Stop()
            directIOWrapper("node[2].smub.source.output = 0 node[2].smua.source.output = 0")
        Catch ex As COMException
            MsgBox(ex.ErrorCode & " " & ex.ToString)
            Dim errMessage As String = ""
            switchDriver.Utility.ErrorQuery(ex.ErrorCode, errMessage)
            MsgBox(errMessage)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' This function is triggered by backgroundworker1.  Because it runs in the main thread we can use it to update the chart object
    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged 
        Try
            If (e.ProgressPercentage = 10) Then
                TestChart.Update()
                currentTestFile.writeToFile()
            Else
                TestChart.Series(currentID).Points.AddXY(currentTime / 1000, currentCurrent)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub btnNoteInjection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoteInjection.Click
        ' Add the current time to the test file injections array
        Try
            Dim timestamp As DateTime = DateTime.Now()
            currentTestFile.addInjection(timestamp)
            MsgBox("Injection noted at " & timestamp, vbOKOnly)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    
    ' This method requires the series names to be the same as their legend entries
    Private Sub TestChart_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TestChart.MouseDown
        ' Call hit test method
        Try
            Dim result As HitTestResult = TestChart.HitTest(e.X, e.Y)
            If result.ChartElementType <> ChartElementType.DataPoint Then
                If (result.ChartElementType <> ChartElementType.LegendItem) Then
                    Return
                Else
                    Dim seriesName As String = result.Object.Name
                    For Each aSeries As Series In TestChart.Series
                        aSeries.BorderWidth = 2
                    Next
                    For Each aLegend As Legend In TestChart.Legends
                        aLegend.BorderWidth = 2
                    Next
                    TestChart.Legends(seriesName).BorderWidth = 6
                    TestChart.Series(seriesName).BorderWidth = 6
                End If
            Else
                Dim seriesName As String = result.Series.Name
                For Each aSeries As Series In TestChart.Series
                    aSeries.BorderWidth = 2
                Next
                For Each aLegend As Legend In TestChart.Legends
                    aLegend.BorderWidth = 2
                Next
                TestChart.Legends(seriesName).BorderWidth = 6
                TestChart.Series(seriesName).BorderWidth = 6
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub btnZoomReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomReset.Click
        Try
            TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
            TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
            TestChart.ChartAreas(0).AxisX.Maximum = 0
            TestChart.ChartAreas(0).AxisX.Minimum = 0
            TestChart.ChartAreas(0).AxisY.Maximum = 0
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
    Private Sub showAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In TestChart.Series
                aSeries.Enabled = True
                aSeries.IsVisibleInLegend = True
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
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

End Class