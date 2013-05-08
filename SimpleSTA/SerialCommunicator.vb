Imports System.IO.Ports
Public Class SerialCommunicator
    Dim srlSerialPort As New SerialPort()
    Private strCOMPort As String = "COM5"
    Private intBaudRate As Integer = 9600
    Private intTimeout As Integer = 1500
    Private strNothingReturned As String = "Nothing Returned"

    Public Sub New()
        Try
            srlSerialPort.BaudRate = intBaudRate
            srlSerialPort.PortName = strCOMPort
            srlSerialPort.ReadTimeout = intTimeout
            srlSerialPort.WriteTimeout = intTimeout
            srlSerialPort.StopBits = StopBits.One
            srlSerialPort.DataBits = 8
            srlSerialPort.Parity = Parity.None
            srlSerialPort.NewLine = "\n"
            srlSerialPort.Handshake = Handshake.RequestToSend
        Catch ex As Exception
            srlSerialPort.Close()
        End Try
        
    End Sub

    Public Function Initialize() As Boolean
        Try
            srlSerialPort.Open()
            Return True
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function

    Public Function Close() As Boolean
        Try
            If (srlSerialPort.IsOpen) Then
                srlSerialPort.Close()
            End If
            Return True
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function

    Public Sub SendCommand(ByVal strCommand As String)
        Try
            srlSerialPort.DiscardInBuffer()
            srlSerialPort.WriteLine(strCommand)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Public Function ReadLine() As String
        Try
            Return srlSerialPort.ReadLine()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return strNothingReturned
        End Try
    End Function

End Class
