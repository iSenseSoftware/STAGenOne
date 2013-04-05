Imports System.Xml.Serialization
Imports System.IO

Public Class TestSystem
    Dim arySwitches() As Switch
    Dim arySources() As SourceMeter
    Public Property Switches As Switch()
        Get
            Return arySwitches
        End Get
        Set(value As Switch())
            arySwitches = value
        End Set
    End Property
    Public Property Sources As SourceMeter()
        Get
            Return arySources
        End Get
        Set(value As SourceMeter())
            arySources = value
        End Set
    End Property
    Public Sub AddSwitch(ByRef newSwitch As Switch)
        Try
            If arySwitches Is Nothing Then
                ReDim arySwitches(0)
                arySwitches(0) = newSwitch
            Else
                Dim upper As Long = arySwitches.GetUpperBound(0)
                ReDim Preserve arySwitches(upper + 1)
                arySwitches(upper + 1) = newSwitch
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub AddSource(ByRef newSource As SourceMeter)
        Try
            If arySources Is Nothing Then
                ReDim arySources(0)
                arySources(0) = newSource
            Else
                Dim upper As Long = arySources.GetUpperBound(0)
                ReDim Preserve arySources(upper + 1)
                arySources(upper + 1) = newSource
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Function GetSourceBySerial(ByVal strSerial As String) As SourceMeter
        If arySources Is Nothing Then Return Nothing
        For Each aSource In arySources
            If aSource.SerialNumber = strSerial Then
                Return aSource
            End If
        Next
        Return Nothing
    End Function
    Public Function GetSwitchBySerial(ByVal strSerial As String) As Switch
        If arySwitches Is Nothing Then Return Nothing
        For Each aSwitch In arySwitches
            If aSwitch.SerialNumber = strSerial Then
                Return aSwitch
            End If
        Next
        Return Nothing
    End Function
    Public Sub writeToFile()
        ' Serializes the instance to file, using the file path defined by strDumpFile
        Try
            Dim serializer As New XmlSerializer(Me.GetType)
            Dim writer As New StreamWriter(config.SystemFileDirectory & "\SystemInfo.xml")
            serializer.Serialize(writer, Me)
            writer.Close()
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
            MsgBox("Unable to dump test data to file")
        End Try
    End Sub
    Public Sub addSwitchEvent(intSlot As Integer, Optional intSwitchCount As Integer = 1)
        Try
            Dim lngOldCounts As Long
            Dim intCardsFound As Integer
            Dim updateCard As Card
            intCardsFound = 0
            For Each theCard In currentCards
                If theCard.Slot = intSlot Then
                    updateCard = theCard
                    intCardsFound = intCardsFound + 1
                End If
            Next
            If (intCardsFound = 1) Then
                lngOldCounts = updateCard.TotalSwitches
                updateCard.TotalSwitches = lngOldCounts + intSwitchCount
            Else
                Throw New Exception("Duplicate active Cards found in SystemInfo file")
            End If
        Catch ex As Exception
            MsgBox("An exception occurred:" & Environment.NewLine & ex.Message & Environment.NewLine & ex.ToString)
        End Try
    End Sub

End Class
