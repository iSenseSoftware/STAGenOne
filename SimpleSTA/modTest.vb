Imports System.Runtime.InteropServices

Public Module modTest
    Public boolIsTestRunning As Boolean = False 'Boolean flag - has the user clicked start and not yet clicked stop
    Public boolIsTestStopped As Boolean = True  ' Boolean flag - has the test actually stopped
    Public stpInjectionTime As New Stopwatch    ' Stopwatch to track the time since the last noted injection
    Public stpTotalTime As New Stopwatch        ' Stopwatch to track the total elapsed time in the test
    Public intInjectionCounter As Integer       ' Counter for the number of injections that have been performed


    ' All variables prefaced with current- are declared with
    ' module-level scope so that cross-thread references can be made without throwing an exception
    Dim lngCurrentTime As Long              ' Timestamp for last gathered reading
    Dim dblCurrentCurrent As Double         ' Current for last gathered reading
    Dim intCurrentSlot As Integer           ' Card slow for last gathered reading
    Dim intCurrentColumn As Integer         ' Card column for last gathered reading
    Dim strCurrentID As String              ' SensorID for last gathered reading


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
    Public Sub MeasurementLoop(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            ' --------------------------------------
            ' Declare locally-scoped variables
            ' --------------------------------------
            Dim stpIntervalTimer As New Stopwatch ' Tracks elapsed time for current measurement interval
            Dim intInterval As Integer = cfgGlobal.RecordInterval ' Length of measurement interval in seconds
            Dim intMilliseconds As Integer      ' Measurement interval in milliseconds
            Dim intCardCount As Integer = cfgGlobal.CardConfig
            Dim intSensorCount As Integer       ' Number of Sensors to scan
            Dim strVoltageReadings As String    ' String for building list of voltage readings
            Dim strCurrentReadings As String    ' String for building list of current readings
            Dim intReading As Integer = 0       ' Integer for counting the number of measurements; used for calculating
            Dim intTimeInterval As Integer      ' IntTimeInterval = intReading * intInterval
            Dim intLoopSensor As Integer        ' Loop counter for main measurement loop
            Dim dblCurrent As Double
            Dim dblVolts As Double

            ' Define Variables
            intMilliseconds = intInterval * 1000
            intSensorCount = 16 * cfgGlobal.CardConfig

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

            ' Configure SourceMeter
            ConfigureHardware(cfgGlobal.Bias, cfgGlobal.Range, cfgGlobal.Filter, cfgGlobal.Samples, cfgGlobal.NPLC)

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

            'Here's some legacy code to figure out:
            '' '' Start all timers
            ' ''frmTestForm.ElapsedTimer.Start() ' Start the form timer component
            ' ''stpIntervalTimer.Start() ' Current interval stopwatch
            ' ''stpTotalTime.Start() ' total test time stopwatch
            ' ''fCurrentTestFile.TestStart = DateTime.Now()

            'Set the flags for running the loop
            boolIsTestRunning = True
            boolIsTestStopped = False

            'Run the testloop until the boolTestStop variable returns false (the user clicks Abort)
            Do While boolIsTestRunning
                ' Determine the "Time" for the set of measurements, and add it to the current measurement string
                intTimeInterval = intReading * intInterval
                strCurrentReadings = intTimeInterval + ","
                ' Reset the voltage measurement string
                strVoltageReadings = ","

                'loop through sensor readings
                For intLoopSensor = 1 To intSensorCount
                    'Close the appropriate switch pattern
                    SwitchIOWrite("node[1].channel.exclusiveclose('Sensor" & intLoopSensor & "')")
                    Debug.Print("node[1].channel.exclusiveclose('Sensor" & intLoopSensor & "')")

                    ' Allow settling time
                    Delay(cfgGlobal.SettlingTime)

                    ' Record V and I readings to buffer
                    SwitchIOWrite("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")

                    ' Read V and I from buffer
                    dblCurrent = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smub.nvbuffer1.n, node[2].smub.nvbuffer1)")) * 10 ^ 9
                    dblVolts = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smub.nvbuffer2.n, node[2].smub.nvbuffer2)"))

                    ' Add reading to current and voltage strings
                    strCurrentReadings = strCurrentReadings + dblCurrent + ","
                    strVoltageReadings = strVoltageReadings + dblVolts + ","

                    ' Update the Chart
                    strCurrentID = "Sensor" + intLoopSensor
                    AddGraphData(strCurrentID, intTimeInterval, dblCurrent)
                Next

                ' Record the voltage and current across all sensors
                ' Close all Switches
                SwitchIOWrite("node[1].channel.exclusiveclose('Sensor" & intSensorCount + 1 & "')")
                Debug.Print("node[1].channel.exclusiveclose('Sensor" & intSensorCount + 1 & "')")

                ' Allow settling time
                Delay(cfgGlobal.SettlingTime)

                ' Record V and I readings to buffer
                SwitchIOWrite("node[2].smua.measure.iv(node[2].smua.nvbuffer1, node[2].smua.nvbuffer2)")

                ' Read V and I from buffer
                dblCurrent = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smua.nvbuffer1.n, node[2].smua.nvbuffer1)")) * 10 ^ 9
                dblVolts = CDbl(SwitchIOWriteRead("printbuffer(1, node[2].smua.nvbuffer2.n, node[2].smua.nvbuffer2)"))

                ' Add Channel A readings to data string

                ' Write string to data file
                WriteToDataFile(strCurrentReadings + strVoltageReadings)

                RefreshGraph(sender)

                ' Check if measurements complete within the interval period
                If (stpIntervalTimer.ElapsedMilliseconds > intMilliseconds) Then
                    MsgBox("Could not finish measurements within injection interval specified")
                    boolIsTestRunning = False
                    boolIsTestStopped = True
                End If
                Dim intLast As Integer

                'Wait for the remainder of the interval period to elapse
                Do Until stpIntervalTimer.ElapsedMilliseconds >= intMilliseconds
                    ' do nothing.  This is to ensure that the interval elapses before another round of measurements
                    If Not boolIsTestRunning Then
                        If intLast = 0 And (intMilliseconds - stpIntervalTimer.ElapsedMilliseconds) / 1000 = 0 Then
                            frmTestForm.btnStartTest.Text = "Test Complete"
                        Else
                            If Not ((intMilliseconds - stpIntervalTimer.ElapsedMilliseconds) / 1000 = intLast) Then
                                intLast = (intMilliseconds - stpIntervalTimer.ElapsedMilliseconds) / 1000
                                frmTestForm.btnStartTest.Text = "Ending In " & intLast
                            End If
                        End If
                    End If
                Loop

                'Restart the interval timer, then repeat the loop
                stpIntervalTimer.Restart()
            Loop

            ' After test is complete, reset state, do the following:
            ' Write end test time to file
            WriteToDataFile("Test End:," & DateTime.Now())

            ' Change the text of the start test button to test complete
            frmTestForm.btnStartTest.Text = "Test Complete"

            ' Update the displays of the measurement hardware
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[1].display.clear()")
            SwitchIOWrite("node[2].display.settext('Standby')")
            SwitchIOWrite("node[1].display.settext('Standby')")

            ' Update the booleans to reflect the 
            boolIsTestStopped = True

            ' Stop the total time timer
            stpTotalTime.Stop()

            ' Turn off the sourceMeter
            SwitchIOWrite("node[2].smub.source.output = 0 node[2].smua.source.output = 0")

        Catch ex As COMException
            ComExceptionHandler(ex)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Public Sub EndTest()

        'Things to do if communication is open:
        If boolIOStatus Then
            'Open all switches
            SwitchIOWrite("channel.open('allslots')")

            'Turn off the sourcemeters
            SwitchIOWrite("node[2].smua.source.output = 0")
            SwitchIOWrite("node[2].smub.source.output = 0")

            'Close communication
            CloseKeithleyIO()
        End If

        'Things to do if data file is open
        If boolDataFileOpen Then
            'Close the data file
            CloseDataFile()
        End If

        'Re-enable the frmMainForm buttons
        With frmMain
            .btnConfig.Enabled = True
            .btnNewTest.Enabled = True
        End With

    End Sub
End Module
