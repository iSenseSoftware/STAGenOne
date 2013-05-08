Option Explicit On
' -----------------------------------------------------------------------------------------------------------------
' The Switch class defines an object used for storing indentification and usage history information
' or a particular system switch component of the STA test system measurement hardware component.
' Of note is the .Cards array which contains an array of Card objects representing the cards present in each
' of the switch's expansion slots.
' -----------------------------------------------------------------------------------------------------------------
Public Class Switch
    Dim aryCards() As Card
    Dim strSerial As String
    Dim strModel As String
    Dim strRevision As String
    Dim boolActive As Boolean
    Dim dtFirstTest As DateTime
    Dim dtLastTest As DateTime
    Public Property Revision As String
        Get
            Return strRevision
        End Get
        Set(strValue As String)
            strRevision = strValue
        End Set
    End Property
    Public Property SerialNumber As String
        Get
            Return strSerial
        End Get
        Set(strValue As String)
            strSerial = strValue
        End Set
    End Property
    Public Property ModelNumber As String
        Get
            Return strModel
        End Get
        Set(strValue As String)
            strModel = strValue
        End Set
    End Property
    Public Property FirstTest As DateTime
        Get
            Return dtFirstTest
        End Get
        Set(dtValue As DateTime)
            dtFirstTest = dtValue
        End Set
    End Property
    Public Property LastTest As DateTime
        Get
            Return dtLastTest
        End Get
        Set(dtValue As DateTime)
            dtLastTest = dtValue
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return boolActive
        End Get
        Set(boolValue As Boolean)
            boolActive = boolValue
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
    ' Name: AddCard()
    ' Parameters: 
    '           cdNewCard: The Card object to be added to the Cards array
    ' Description: Adds the Card object given as a parameter to the Cards array
    Public Sub AddCard(ByRef cdNewCard As Card)
        Try
            If aryCards(0) Is Nothing Then
                ReDim aryCards(0)
                aryCards(0) = cdNewCard
            Else
                Dim lngUpper As Long = aryCards.GetUpperBound(0)
                ReDim Preserve aryCards(lngUpper + 1)
                aryCards(lngUpper + 1) = cdNewCard
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: GetCardBySerial()
    ' Parameters:
    '           strSerial: The serial number to be located
    ' Returns: Card: The Card object identified by the given serial, or nothing if it cannot be found
    ' Description: Iterates through all object in the Cards array and searches for one with the given serial number
    Public Function GetCardBySerial(ByVal strSerial As String) As Card
        Try
            If aryCards Is Nothing Then Return Nothing
            For Each cdCard In aryCards
                If cdCard.SerialNumber = strSerial Then
                    Return cdCard
                End If
            Next
            Return Nothing
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return Nothing
        End Try
    End Function
End Class
