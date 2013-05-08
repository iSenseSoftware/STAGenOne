Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System
Imports System.IO
Imports Microsoft.VisualBasic

Public Class Communicator
    Dim boolConnected As Boolean ' Indicates whether a connection is active
    Dim sckSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) ' The socket object used to communicate with the measurement hardware
    Public ReadOnly Property Connected As Boolean
        Get
            Return sckSocket.Connected
        End Get
    End Property
    Public Function Initialize() As Boolean
        Try
            If (sckSocket.Connected) Then
                Return True
            End If
            sckSocket.Connect(lngIPAddress, 5025)
            If (sckSocket.Connected) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function
    Public Function Close() As Boolean
        Try
            If Not (sckSocket.Connected) Then
                Return True
            Else
                sckSocket.Close()
                Return True
            End If
        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function
    Public Function SendCommand(ByVal strCommand As String) As Boolean
        Try
            Dim bytes(255) As Byte
            Dim sent As Integer
            sent = sckSocket.Send(Encoding.ASCII.GetBytes(strCommand))
            sckSocket.Receive(bytes)
            Return True
        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function
    Private Shared Function ConnectSocket(server As String, port As Integer) As Socket
        Dim s As Socket = Nothing
        Dim hostEntry As IPHostEntry = Nothing

        ' Get host related information.
        hostEntry = Dns.GetHostEntry(server)

        ' Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        ' an exception that occurs when the host host IP Address is not compatible with the address family
        ' (typical in the IPv6 case).
        Dim address As IPAddress

        For Each address In hostEntry.AddressList
            Dim endPoint As New IPEndPoint(address, port)
            Dim tempSocket As New Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

            tempSocket.Connect(endPoint)

            If tempSocket.Connected Then
                s = tempSocket
                Exit For
            End If

        Next address

        Return s
    End Function


    ' This method requests the home page content for the specified server.

    Private Shared Function SocketSendReceive(server As String, port As Integer) As String
        'Set up variables and String to write to the server.
        Dim ascii As Encoding = Encoding.ASCII
        Dim request As String = "GET / HTTP/1.1" + ControlChars.Cr + ControlChars.Lf + "Host: " + server + ControlChars.Cr + ControlChars.Lf + "Connection: Close" + ControlChars.Cr + ControlChars.Lf + ControlChars.Cr + ControlChars.Lf
        Dim bytesSent As [Byte]() = ascii.GetBytes(request)
        Dim bytesReceived(255) As [Byte]

        ' Create a socket connection with the specified server and port.
        Dim s As Socket = ConnectSocket(server, port)

        If s Is Nothing Then
            Return "Connection failed"
        End If
        ' Send request to the server.
        s.Send(bytesSent, bytesSent.Length, 0)

        ' Receive the server  home page content.
        Dim bytes As Int32

        ' Read the first 256 bytes.
        Dim page As [String] = "Default HTML page on " + server + ":" + ControlChars.Cr + ControlChars.Lf

        ' The following will block until the page is transmitted.
        Do
            bytes = s.Receive(bytesReceived, bytesReceived.Length, 0)
            page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes)
        Loop While bytes > 0

        Return page
    End Function

    'Entry point which delegates to C-style main Private Function
    Public Overloads Shared Sub Main()
        Main(System.Environment.GetCommandLineArgs())
    End Sub


    Private Overloads Shared Sub Main(args() As String)
        Dim host As String
        Dim port As Integer = 80

        If args.Length = 1 Then
            ' If no server name is passed as argument to this program, 
            ' use the current host name as default.
            host = Dns.GetHostName()
        Else
            host = args(1)
        End If

        Dim result As String = SocketSendReceive(host, port)

        Console.WriteLine(result)
    End Sub 'Main
End Class
