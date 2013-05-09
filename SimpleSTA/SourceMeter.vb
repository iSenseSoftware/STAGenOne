Option Explicit On
' ------------------------------------------------------------------------------------------------------
' The SourceMeter class defines an object used for storing indentifying information for a particular
' source meter used in the test system measurement hardware component of the STA system.  
' ------------------------------------------------------------------------------------------------------
Public Class SourceMeter
    Dim strSerial As String
    Dim strModel As String
    Dim strRevision As String
    Dim boolActive As Boolean ' Is this the source meter currently in use?
    Dim dtFirstTest As DateTime
    Dim dtLastTest As DateTime
    Public Property Revision As String
        Get
            Return strRevision
        End Get
        Set(ByVal strValue As String)
            strRevision = strValue
        End Set
    End Property
    Public Property SerialNumber As String
        Get
            Return strSerial
        End Get
        Set(ByVal strValue As String)
            strSerial = strValue
        End Set
    End Property
    Public Property ModelNumber As String
        Get
            Return strModel
        End Get
        Set(ByVal strValue As String)
            strModel = strValue
        End Set
    End Property
    Public Property FirstTest As DateTime
        Get
            Return dtFirstTest
        End Get
        Set(ByVal dtValue As DateTime)
            dtFirstTest = dtValue
        End Set
    End Property
    Public Property LastTest As DateTime
        Get
            Return dtLastTest
        End Get
        Set(ByVal dtValue As DateTime)
            dtLastTest = dtValue
        End Set
    End Property
    Public Property Active As Boolean
        Get
            Return boolActive
        End Get
        Set(ByVal boolValue As Boolean)
            boolActive = boolValue
        End Set
    End Property
    Public Sub New()

    End Sub
End Class
