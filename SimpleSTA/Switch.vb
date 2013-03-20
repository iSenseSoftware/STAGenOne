Public Class Switch
    Dim aryCards(0) As Card
    Dim strSerial As String
    Dim strModel As String
    Dim boolActive As Boolean
    Dim dtFirstTest As DateTime
    Dim dtLastTest As DateTime
    Public Property SerialNumber As String
        Get
            Return strSerial
        End Get
        Set(value As String)
            strSerial = value
        End Set
    End Property
    Public Property ModelNumber As String
        Get
            Return strModel
        End Get
        Set(value As String)
            strModel = value
        End Set
    End Property
    Public Property FirstTest As DateTime
        Get
            Return dtFirstTest
        End Get
        Set(value As DateTime)
            dtFirstTest = value
        End Set
    End Property
    Public Property LastTest As DateTime
        Get
            Return dtLastTest
        End Get
        Set(value As DateTime)
            dtLastTest = value
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return boolActive
        End Get
        Set(value As Boolean)
            boolActive = value
        End Set
    End Property
    Public Property Cards As Card()
        Get
            Return aryCards
        End Get
        Set(value As Card())
            aryCards = value
        End Set
    End Property
    Public Sub AddCard(ByVal newCard As Card)
        Try
            If aryCards(0) Is Nothing Then
                ReDim aryCards(0)
                aryCards(0) = newCard
            Else
                Dim upper As Long = aryCards.GetUpperBound(0)
                ReDim Preserve aryCards(upper + 1)
                aryCards(upper + 1) = newCard
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
End Class
