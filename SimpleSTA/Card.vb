Public Class Card
    Dim strSerial As String
    Dim strModel As String
    Dim dtFirstTest As DateTime
    Dim dtLastTest As DateTime
    Dim lngTotalSwitches As Long
    Dim boolActive As Boolean

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
    Public Property TotalSwitches As Long
        Get
            Return lngTotalSwitches
        End Get
        Set(value As Long)
            lngTotalSwitches = value
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
End Class
