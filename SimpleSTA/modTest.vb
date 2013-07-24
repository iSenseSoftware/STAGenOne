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
    Public strCurrentID As String              ' SensorID for last gathered reading


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
