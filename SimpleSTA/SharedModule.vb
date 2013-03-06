Imports System.IO
Imports System.Xml.Serialization
Imports Keithley.Ke37XX.Interop

' The SharedModule is, as the name suggests, a collection of shared utlity functions, enumerated variables
' and global variables for use in all objects and forms
Public Module SharedModule
    Public Const configFileName As String = "\Config.xml"
    Public config As New Configuration
    Public currentTestFile As New TestFile
    Public switchDriver As New Ke37XX
    Public appDir As String
    ' The admin password to unlock the configuration settings is hardcoded.  In the future
    ' it may be desireable to incorporate user authentication / authorization modules for granular permissions
    Public strAdminPassword As String = "C0balt22"
    ' Declare Enums for configuration settings
    ' Because of a quirk of VB, a value of 0 for an enumerated variable is equivalent to Nothing, making validation difficult
    ' When the CurrentRange and FilterType enums are used for direct input to the test system switch we must subtract 1
    Public Enum CurrentRange
        one_uA = 1
        ten_uA = 2
        hundred_uA = 3
    End Enum
    Public Enum FilterType
        FILTER_REPEAT_AVG = 1
        FILTER_MOVING_AVG = 2
        FILTER_MEDIAN = 3
    End Enum
    Public Enum CardConfiguration
        ONE_CARD_SIXTEEN_SENSORS = 1
        TWO_CARD_THIRTY_TWO_SENSORS = 2
        THREE_CARD_FOURTY_EIGHT_SENSORS = 3
        FOUR_CARD_SIXTY_FOUR_SENSORS = 4
        FIVE_CARD_EIGHTY_SENSORS = 5
        SIX_CARD_NINETY_SIX_SENSORS = 6
    End Enum
    ' Load configuration from file and set the values in the global Configuration object config
    ' @TODO: Make the file location optional rather than hard-coded and store between program sessions
    Public Sub loadConfiguration()
        Try
            Dim serializer As New XmlSerializer(config.GetType)
            Dim reader As New StreamReader(appDir & configFileName)
            config = serializer.Deserialize(reader)
            reader.Close()
        Catch parseException As InvalidOperationException
            MsgBox("Invalid configuration file.  Delete " & appDir & "\Config.xml and reload.")
            frmMain.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub initializeConfiguration()
        Try
            Dim serializer As New XmlSerializer(config.GetType)
            Dim writer As New StreamWriter(appDir & configFileName)
            serializer.Serialize(writer, config)
            writer.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Function verifyConfiguration() As Boolean
        Dim verifies As Boolean = True
        If (config.Bias = Nothing) Then
            verifies = False
        End If
        If (config.RecordInterval = Nothing) Then
            verifies = False
        End If
        If (config.Range = Nothing) Then
            verifies = False
        End If
        If (config.Filter = Nothing) Then
            verifies = False
        End If
        If (config.Samples = Nothing) Then
            verifies = False
        End If
        If (config.NPLC = Nothing) Then
            verifies = False
        End If
        If (config.Address = Nothing Or config.Address = "") Then
            verifies = False
        End If
        If (config.STAID = Nothing Or config.STAID = "") Then
            verifies = False
        End If
        If (config.CardConfig = Nothing) Then
            verifies = False
        End If
        Return verifies
    End Function
    ' As the name suggests, this is simply a wrapper for the System.DirectIO.WriteString method of the Ke37xx driver.
    Public Sub directIOWrapper(ByVal command As String)
        Try
            switchDriver.System.DirectIO.WriteString(command)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub Delay(ByVal milliseconds As Long)
        Try
            Dim watch As New Stopwatch
            watch.Start()
            Do

            Loop Until watch.ElapsedMilliseconds >= milliseconds
            watch.Stop()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Utility function to pad channel strings with leading zeroes
    Public Function strPad(ByVal input As String, ByVal padTo As Integer) As String
        Try
            If (input.Length < padTo) Then
                Do While (input.Length < padTo)
                    input = "0" & input
                Loop
            End If
            Return input
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Function
    Public Sub GenericExceptionHandler(ByVal theException As Exception)
        MsgBox(theException.Message & Environment.NewLine & theException.ToString)
        MsgBox(theException.GetType.ToString())
    End Sub
End Module
