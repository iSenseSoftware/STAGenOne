'Imports System.Data
'Imports System.Data.SqlClient
'' The CrudBuddy class (so named for the Create, Read, Update and Delete database operations it facilitates) is a pseudo-abstraction
'' layer for database access that contains an instance of SqlConnection and includes wrapper functions
'' for retrieving, creating and updating records
'' Because this encapsulates an instance of SqlConnection, of which only one is needed per database, for best performance
'' this should be implemented as a singleton
'Public Class CrudBuddy
'    Dim theConnection As SqlConnection
'    Dim theCommand As SqlCommand
'    Dim theReader As SqlDataReader
'    Dim strConnectionString As String
'    Public Property ConnectionString As String
'        Get
'            Return strConnectionString
'        End Get
'        Set(ByVal value As String)
'            strConnectionString = value
'        End Set
'    End Property
'    Public ReadOnly Property LastID As Integer
'        Get
'            Dim returnVal As Integer
'            theCommand = New SqlCommand("SELECT @@Indentity")
'            returnVal = theCommand.ExecuteScalar
'            Return returnVal
'        End Get
'    End Property
'    Public ReadOnly Property Results As SqlDataReader
'        Get
'            Return theCommand.ExecuteReader
'        End Get
'    End Property

'    Public Sub New(Optional ByVal connectionString As String = "server=tcp:\\huswivc0219\SQLEXPRESS;User ID=jmckenzie;Password=kollani;Database=STA;")
'        ' initialize the instance and set defaults
'        strConnectionString = connectionString
'        theConnection = New SqlConnection(strConnectionString)
'        theConnection.Open()
'    End Sub
'    ' This sub is called to close the connection when it is not needed, saving resources
'    Public Sub Kill()
'        theConnection.Close()
'    End Sub
'    ' This sub is called to re-open the connection
'    Public Sub Initialize()
'        theConnection.Open()
'    End Sub
'    ' 
'    Public Function Query(ByVal strQuery As String) As SqlDataReader
'        theCommand = New SqlCommand(strQuery)
'        theReader = theCommand.ExecuteReader
'        Return theReader
'    End Function

'    Public Function Execute(ByVal strQuery As String) As Boolean
'        theCommand = New SqlCommand(strQuery)
'        Dim rowCount As Integer = theCommand.ExecuteNonQuery
'        If (rowCount > 0) Then
'            Return True
'        Else
'            Return False
'        End If

'    End Function

'End Class
