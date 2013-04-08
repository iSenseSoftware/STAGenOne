Public Class AuditChannel
    Dim intCard As Integer
    Dim intColumn As Integer
    Dim aryAuditReadings(0) As AuditReading
    Public Property Card As Integer
        Get
            Return intCard
        End Get
        Set(value As Integer)
            intCard = value
        End Set
    End Property
    Public Property Column As Integer
        Get
            Return intColumn
        End Get
        Set(value As Integer)
            intColumn = value
        End Set
    End Property
    Public Property AuditReadings As AuditReading()
        Get
            Return aryAuditReadings
        End Get
        Set(value As AuditReading())
            aryAuditReadings = value
        End Set
    End Property
    Public Sub AddReading(ByVal reading As AuditReading)
        Try
            If aryAuditReadings(0) Is Nothing Then
                ReDim aryAuditReadings(0)
                aryAuditReadings(0) = reading
            Else
                Dim upper As Long = aryAuditReadings.GetUpperBound(0)
                ReDim Preserve aryAuditReadings(upper + 1)
                aryAuditReadings(upper + 1) = reading
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Function ChannelFactory(card As Integer, column As Integer)
        Try
            Dim outChannel As New AuditChannel
            outChannel.Card = card
            outChannel.Column = column
            Return outChannel
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Function
    Public Sub New()

    End Sub
End Class
