' --------------------------------------------------------------------------------
' The AuditChannel class defines an object which contains the readings taken on a particular
' Switch matrix column during the Audit Check operation.
' The AuditCheck object contains an array of AuditChannel objects which each 
' contain an array of AuditReading objects which contain the actual voltage
' and current values measured
' --------------------------------------------------------------------------------
Option Explicit On
Public Class AuditChannel
    Dim intCard As Integer
    Dim intColumn As Integer
    Dim aryAuditReadings(0) As AuditReading
    ' -----------------------------------------------
    ' Getters and Setters
    ' -----------------------------------------------
    Public Property Card As Integer
        Get
            Return intCard
        End Get
        Set(intValue As Integer)
            intCard = intValue
        End Set
    End Property
    Public Property Column As Integer
        Get
            Return intColumn
        End Get
        Set(intValue As Integer)
            intColumn = intValue
        End Set
    End Property
    Public Property AuditReadings As AuditReading()
        Get
            Return aryAuditReadings
        End Get
        Set(aryValue As AuditReading())
            aryAuditReadings = aryValue
        End Set
    End Property
    ' ----------------------------------------------------------------
    ' Utility Functions
    ' ----------------------------------------------------------------
    ' Name: AddReading()
    ' Parameters: 
    '           ardReading: The AuditReading object to be added to the AuditReadings array
    ' Description: Adds the AuditReading object given as a parameter to the AuditReadings array
    Public Sub AddReading(ByVal ardReading As AuditReading)
        Try
            ' If the first element has not been assigned, assign the input to the first element
            If aryAuditReadings(0) Is Nothing Then
                ReDim aryAuditReadings(0)
                aryAuditReadings(0) = ardReading
            Else
                ' Otherwise, redimension the array to make room for the new object
                Dim lngUpper As Long = aryAuditReadings.GetUpperBound(0)
                ReDim Preserve aryAuditReadings(lngUpper + 1)
                aryAuditReadings(lngUpper + 1) = ardReading
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: ChannelFactory()
    ' Parameters:
    '           intCard: The card (1-6) in which the channel exists
    '           intColumn: The column in the card switch matrix
    ' Returns: AuditChannel object
    ' Description: Because, as noted below, the constructor must be an empty subroutine, we
    ' use this function to return an instance of the AuditChannel object with the given
    ' property values assigned.
    Public Function ChannelFactory(intCard As Integer, intColumn As Integer) As AuditChannel
        Try
            Dim acOutChannel As New AuditChannel
            acOutChannel.Card = intCard
            acOutChannel.Column = intColumn
            Return acOutChannel
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
    Public Sub New()
        ' In order for this object to be properly serialized by the XMLSerializer class, the constructor must be empty
    End Sub
End Class
