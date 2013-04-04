Public Class SourceMeter
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
        Set(value As String)
            strRevision = value
        End Set
    End Property
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
End Class
