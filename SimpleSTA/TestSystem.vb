Option Explicit On
Imports System.Xml.Serialization
Imports System.IO
' ---------------------------------------------------------------------------------
' The TestSystem class defines an object used to maintain identifying and usage
' history information for the hardware components of the STA test system
' -----------------------------------------------------------------------------------
Public Class TestSystem
    Dim arySwitches() As Switch
    Dim arySources() As SourceMeter
    Public Property Switches As Switch()
        Get
            Return arySwitches
        End Get
        Set(aryValue As Switch())
            arySwitches = aryValue
        End Set
    End Property
    Public Property Sources As SourceMeter()
        Get
            Return arySources
        End Get
        Set(aryValue As SourceMeter())
            arySources = aryValue
        End Set
    End Property
    ' Name: AddSwitch()
    ' Parameters:
    '           swtNewSwitch: The instance of Switch to be added to the Switches array
    ' Description: Adds the given Switch instance to the Switches array, redimensioning as necessary
    Public Sub AddSwitch(ByRef swtNewSwitch As Switch)
        Try
            If arySwitches Is Nothing Then
                ReDim arySwitches(0)
                arySwitches(0) = swtNewSwitch
            Else
                Dim lngUpper As Long = arySwitches.GetUpperBound(0)
                ReDim Preserve arySwitches(lngUpper + 1)
                arySwitches(lngUpper + 1) = swtNewSwitch
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    ' Name: AddSourceMeter()
    ' Parameters:
    '           swtNewSourceMeter: The instance of SourceMeter to be added to the Sources array
    ' Description: Adds the given SourceMeter instance to the Sources array, redimensioning as necessary
    Public Sub AddSource(ByRef smNewSource As SourceMeter)
        Try
            If arySources Is Nothing Then
                ReDim arySources(0)
                arySources(0) = smNewSource
            Else
                Dim lngUpper As Long = arySources.GetUpperBound(0)
                ReDim Preserve arySources(lngUpper + 1)
                arySources(lngUpper + 1) = smNewSource
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    ' Name: GetSourceBySerial()
    ' Parameters:
    '           strSerial: The serial no for the SourceMeter object to be found
    ' Returns: SourceMeter: The SourceMeter instance with the serial number given as input
    ' Description: Loops through all members of the Sources array and checks for a serial number match
    '               if a match is found, returns the match, otherwise returns Nothing
    Public Function GetSourceBySerial(ByVal strSerial As String) As SourceMeter
        Try
            If arySources Is Nothing Then Return Nothing
            For Each smSource In arySources
                If smSource.SerialNumber = strSerial Then
                    Return smSource
                End If
            Next
            Return Nothing
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    ' Name: GetSwitchBySerial()
    ' Parameters:
    '           strSerial: The serial no for the Switch object to be found
    ' Returns: Switch: The Switch instance with the serial number given as input
    ' Description: Loops through all members of the Switches array and checks for a serial number match
    '               if a match is found, returns the match, otherwise returns Nothing
    Public Function GetSwitchBySerial(ByVal strSerial As String) As Switch
        Try
            If arySwitches Is Nothing Then Return Nothing
            For Each aSwitch In arySwitches
                If aSwitch.SerialNumber = strSerial Then
                    Return aSwitch
                End If
            Next
            Return Nothing
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    ' Name: WriteToFile()
    ' Parameters:
    '           strFilePath: The full path to the location to which the file is saved
    ' Return: Boolean: Indicates success or failure
    ' Description: Attempts to serialize the current instance to an xml file at the path given as a parameter
    Public Function writeToFile(ByVal strFilePath As String) As Boolean
        ' Serializes the instance to file, using the file path defined by strDumpFile
        Try
            Dim xsSerializer As New XmlSerializer(Me.GetType)
            Dim swWriter As New StreamWriter(strFilePath)
            xsSerializer.Serialize(swWriter, Me)
            swWriter.Close()
            Return True
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    Public Function Validate() As Boolean
        Return True
    End Function
    ' Name: AddSwitchEvent()
    ' Parameters:
    '           intslot: The slot in which the card is installed
    '           intSwitchCount: The number of switches to be added to the total
    ' Description: Updates the given card's TotalSwitches count by incrementing with the given value
    Public Sub AddSwitchEvent(intSlot As Integer, Optional intSwitchCount As Integer = 1)
        Try
            Dim lngOldCounts As Long
            'Dim intCardsFound As Integer ' Counts how many matching cards have been found.  If there are multiple active cards in the same slot we have a problem!
            'Dim cdUpdateCard As Card
            'intCardsFound = 0
            For Each cdCard In aryCurrentCards
                If cdCard.Slot = intSlot And cdCard.Active Then
                    lngOldCounts = cdCard.TotalSwitches
                    cdCard.TotalSwitches = lngOldCounts + intSwitchCount
                End If
            Next
            'For Each cdTheCard In aryCurrentCards
            ' If cdTheCard.Slot = intSlot And cdTheCard.Active Then
            ' cdUpdateCard = cdTheCard
            ' intCardsFound = intCardsFound + 1
            ' End If
            ' Next
            ' If (intCardsFound = 1) Then
            ' lngOldCounts = cdUpdateCard.TotalSwitches
            ' cdUpdateCard.TotalSwitches = lngOldCounts + intSwitchCount
            ' Else
            ' Throw New Exception("Duplicate active Cards found in SystemInfo file")
            ' End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
