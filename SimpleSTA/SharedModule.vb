﻿Imports System.IO
Imports System.Xml.Serialization
Imports Keithley.Ke37XX.Interop

' The SharedModule is, as the name suggests, a collection of shared utlity functions, enumerated variables
' and global variables for use in all objects and forms
Public Module SharedModule
    Public Const configFileName As String = "\Config.xml"
    'Public dataBuddy As New CrudBuddy
    Public config As New Configuration
    Public currentTestFile As New TestFile
    Public switchDriver As New Ke37XX
    Public appDir As String
    ' Declare Enums for configuration settings
    Public Enum CurrentRange
        one_uA = 0
        ten_uA = 1
        hundred_uA = 2
    End Enum
    Public Enum FilterType
        FILTER_REPEAT_AVG = 0
        FILTER_MOVING_AVG = 1
        FILTER_MEDIAN = 2
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
    End Sub
End Module
