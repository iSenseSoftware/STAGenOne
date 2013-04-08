Public Class AuditCheck
    Dim boolPass As Boolean
    Dim aryAuditChannels(0) As AuditChannel
    Public Property Pass As Boolean
        Get
            Return boolPass
        End Get
        Set(value As Boolean)
            boolPass = value
        End Set
    End Property
    Public Property AuditChannels As AuditChannel()
        Get
            Return aryAuditChannels
        End Get
        Set(value As AuditChannel())
            aryAuditChannels = value
        End Set
    End Property
    Public Sub AddChannel(ByVal channel As AuditChannel)
        Try
            If aryAuditChannels(0) Is Nothing Then
                ReDim aryAuditChannels(0)
                aryAuditChannels(0) = channel
            Else
                Dim upper As Long = aryAuditChannels.GetUpperBound(0)
                ReDim Preserve aryAuditChannels(upper + 1)
                aryAuditChannels(upper + 1) = channel
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub Validate()
        Try
            Dim validates As Boolean = True
            For Each aChannel In aryAuditChannels
                For Each aReading In aChannel.AuditReadings
                    If (Math.Abs(((aReading.Voltage / aReading.NominalResistance) - aReading.Current) / (aReading.Voltage / aReading.NominalResistance)) > config.AuditTolerance) Then
                        validates = False
                    End If
                Next
            Next
            boolPass = validates
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
        
    End Sub
    Public Sub New()

    End Sub
End Class
