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



End Module
