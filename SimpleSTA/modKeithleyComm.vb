Option Explicit On

Imports System.Net.Sockets
Imports System.Runtime.InteropServices


Public Module modKeithleyComm
    'Variables for network communication
    Public switchDriver As New TcpClient
    Public switchStream As NetworkStream

    ' Device I/O and Configuration Functions
    ' -----------------------------------------------------------------
    ' Name: EstablishIO()
    ' Returns: Boolean: Indicates success / failure
    ' Description: Attempts to initialize the switchDriver with hard-coded configuration settings
    Public Function EstablishIO() As Boolean
        Try
            If (boolConfigStatus) Then
                If (switchDriver.Connected) Then
                    boolIOStatus = True
                    Return True
                Else
                    'Dim strOptions As String
                    ' An option string must be explicitly declared or the driver throws a COMException.
                    EstablishKeithleyIO(cfgGlobal.Address)
                    If (switchDriver.Connected) Then
                        ' reset the TSPLink so we can communicate with the source meter
                        SwitchIOWrite("tsplink.reset()")
                        SwitchIOWrite("node[2].display.clear()")
                        SwitchIOWrite("node[2].display.settext('TspLink Reset')")
                        'Update UI and return true
                        boolIOStatus = True
                        Return True
                    Else
                        ' Update the UI and return false
                        boolIOStatus = False
                        Return False
                    End If
                End If
            Else
                ' Update the UI and return false
                boolIOStatus = False
                Return False
            End If
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            ' Update the UI and return false
            boolIOStatus = False
            switchDriver.Close()
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            ' Update the UI and return false
            boolIOStatus = False
            Return False
        End Try
    End Function
    ' Name: EstablishKeithleyIO()
    ' Parameters:
    '           strIPAddress: A string containing the IP Address of the measurement hardware
    ' Description: This command opens a TCP network stream to the Keithley 3700 and prepares it for communication
    Public Function EstablishKeithleyIO(ByVal strIPAddress As String) As Boolean
        Dim switchIOReset As New TcpClient
        Dim byteReadBuffer(256) As Byte

        Try
            'Connect to the Dead Socket Termination port to close any previous sessions
            'then close.  Dead sockets are closed when the DST port closes
            switchIOReset.Connect(strIPAddress, 5030)
            switchIOReset.Close()

            'Connect to the Raw port
            switchDriver.Connect(strIPAddress, 5025)
            switchStream = switchDriver.GetStream
            switchStream.ReadTimeout = 1000


            'Clear any existing errors in the error queue of the 3700
            SwitchIOSend("errorqueue.clear()")

            'Set the 3700 to return prompts after each command sent.  This ensures the timely return
            'of data when it is requested, and also provides a status update with each command.  When
            'the error queue is empty "TSP>" is returned, and when there is an error "TSP?" is returned.
            SwitchIOSend("localnode.prompts = 1")

            'Read the IO buffer to remove any prompts have accumulated
            Delay(10)
            Do
                switchStream.Read(byteReadBuffer, 0, byteReadBuffer.Length)
            Loop While switchStream.DataAvailable

            'Reset TSP Link
            SwitchIOWrite("tsplink.reset()")
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[2].display.settext('TspLink Reset')")

            'Upload Sensor Switch patterns
            SetSwitchPatterns(33)

            'Update the Comm status flag
            boolIOStatus = True
            Return True

        Catch SException As SocketException
            MsgBox("Communication could not be established with the communication hardware.  " _
            & "Please check the IP Address in the configuration and try again.  Error code " & SException.ErrorCode, _
            MsgBoxStyle.OkOnly, "Communication not established.")
            EndTest()
            Return False

        Catch ex As Exception
            ' Rethrow the exception to the calling function
            Throw
        End Try
    End Function
    ' Name: SwitchIOSend()
    ' Parameters:
    '           strCommand: A string containing the tsp command to be sent to the measurement hardware
    ' Description: As the name suggests, this sends commands to the raw IO interface of the measurement hardware.
    '              This command does not handle the command prompt, and should be used with care.
    Public Sub SwitchIOSend(ByVal strCommand As String)
        Dim byteMessage As [Byte]() = System.Text.Encoding.ASCII.GetBytes(strCommand & vbLf)
        Try
            'Send the strCommand to the measurement system
            switchStream.Write(byteMessage, 0, byteMessage.Length)
        Catch ex As Exception
            ' Rethrow the exception to the calling function
            Throw
        End Try
    End Sub

    ' Name: SwitchIOReceive()
    ' Parameters:
    '           Returns the TCP network buffer as a string. 
    ' Description: As the name suggests, this receives information from the raw IO interface of the measurement hardware.
    '              This command does not handle the command prompt, and should be used with care.
    Public Function SwitchIOReceive() As String
        Dim byteReadBuffer(1024) As Byte
        'Dim strReadBuffer As String
        Dim strMessage As String = ""
        Dim intBytesRead As Integer

        Try
            'Wait until there is data in the IO buffer
            While switchStream.DataAvailable = False
            End While

            'Read the data in the buffer
            intBytesRead = switchStream.Read(byteReadBuffer, 0, byteReadBuffer.Length)
            strMessage = System.Text.Encoding.ASCII.GetString(byteReadBuffer, 0, intBytesRead)

            'check for command prompt
            If strMessage.Contains("TSP>") Or strMessage.Contains("TSP?") Then

            Else
                While switchStream.DataAvailable = False
                End While
                intBytesRead = switchStream.Read(byteReadBuffer, 0, byteReadBuffer.Length)
                strMessage = strMessage + System.Text.Encoding.ASCII.GetString(byteReadBuffer, 0, intBytesRead)
            End If

            Return strMessage

        Catch ex As Exception
            ' Rethrow the exception to the calling function
            Throw
        End Try

    End Function

    ' Name: SwitchIOCheckError()
    ' Parameters:
    '           strCommand: A string containing the tsp command to be sent to the measurement hardware
    ' Description: This command will send a command to the measurement hardware and check to see if an error is generated.
    Public Sub SwitchIOCheckError()
        Dim strErrorCheck As String

        'Check for error message in the error queue
        strErrorCheck = SwitchIOReceive()
        If strErrorCheck <> "TSP>" Then
            Throw New COMException(strErrorCheck)
        End If

    End Sub

    ' Name: SwitchIOWrite()
    ' Parameters:
    '           strCommand: A string containing the tsp command to be sent to the measurement hardware
    ' Description: This command will send a command to the measurement hardware and check to see if an error is generated.
    Public Sub SwitchIOWrite(ByVal strCommand As String)
        Dim strCommandPrompt As String

        Try
            'Send the strCommand to the measurement system
            SwitchIOSend(strCommand)

            'Check for error message in the error queue
            strCommandPrompt = SwitchIOReceive()
            If strCommandPrompt <> "TSP>" & vbLf Then
                Throw New COMException(strCommandPrompt)
            End If
        Catch comex As COMException
            ' Rethrow the exception to the calling function
            Throw
        End Try
    End Sub


    ' Name: SwitchIOWriteRead()
    ' Parameters:
    '           strCommand: A string containing the tsp command to be sent to the measurement hardware
    ' Description: This command will send a command to the measurement hardware, receive data, and then check to see if an error was generated.
    Public Function SwitchIOWriteRead(ByVal strCommand As String)
        Dim strMessage As String
        Dim strCommandPrompt As String

        Try
            'Send the strCommand to the measurement system
            SwitchIOSend(strCommand)

            'Receive the reply
            strMessage = SwitchIOReceive()

            '
            strCommandPrompt = Right(strMessage, 5)
            strMessage = Left(strMessage, strMessage.Length - 6)
            'Check the 
            If strCommandPrompt <> "TSP>" & vbLf Then
                Throw New COMException(strCommandPrompt)
            End If

        Catch comex As COMException
            ' Rethrow the exception to the calling function
            Throw
        End Try

        Return strMessage

    End Function
    ' Name: CloseKeithleyIO()
    ' Parameters: None
    ' Description: This command will close the communication port.

    Public Sub CloseKeithleyIO()
        'Close the TCP client
        switchDriver.Close()

        'Set the IO flag to false
        boolIOStatus = False

    End Sub

    ' Name: SetSwitchPatterns()
    ' Parameters:
    '           intSensors: The number of sensors + 1 to generate switch patterns for
    ' Description: This command will generate switch patterns for easy switching during sensor scans

    Public Sub SetSwitchPatterns(intSensors As Integer)
        Dim strPattern As String
        Dim i As Integer
        Dim j As Integer

        'Generate the switch closure patterns for all 32 measurement patterns
        For i = 1 To intSensors - 1
            strPattern = "'"
            For j = 1 To intSensors - 1
                If j = i Then
                    strPattern = strPattern + SwitchNumberGenerator(2, j) + ","
                Else
                    strPattern = strPattern + SwitchNumberGenerator(1, j) + ","
                End If
            Next
            strPattern = strPattern + "1911,1912,2911,2912'"
            SwitchIOWrite("channel.pattern.setimage(" & strPattern & ", 'Sensor" & i & "')")
        Next

        'generate the switch closure patterns for all closed on row 1
        strPattern = "'"
        For j = 1 To intSensors - 1
            strPattern = strPattern + SwitchNumberGenerator(1, j) + ","
        Next
        strPattern = strPattern + "1911,1912,2911,2912'"
        SwitchIOWrite("channel.pattern.setimage(" & strPattern & ", 'Sensor" & intSensors & "')")

    End Sub
    ' Name: SwitchNumberGenerator()
    ' Parameters:
    '           strCommand: A string containing the tsp command to be sent to the measurement hardware
    ' Description: This command will send a command to the measurement hardware and check to see if an error is generated.
    Public Function SwitchNumberGenerator(intRow As Integer, intColumn As Integer)
        Dim strSwitchNumber As String

        'Matrix card notation: SRCC, S=slot, R=row, CC=column
        'Determine Slot (card)
        If intColumn Mod 16 = 0 Then
            strSwitchNumber = CStr(intColumn \ 16)
        Else
            strSwitchNumber = CStr(intColumn \ 16 + 1) 'interger division; 15\16 = 0, which is card 1, so +1
        End If

        'Determine row
        strSwitchNumber = strSwitchNumber + CStr(intRow)

        'Determine column
        intColumn = intColumn Mod 16 'the mod function returns the remainder of a division operation (ie, 17 becomes 1)
        If intColumn = 0 Then
            strSwitchNumber = strSwitchNumber + "16"
        Else
            strSwitchNumber = strSwitchNumber + Format(intColumn, "00")
        End If

        Return strSwitchNumber
    End Function
    ' Name: CardInfo()
    ' Parameters: intSlot, the slot number of the System Switch to interrogate
    '           
    ' Description: This command gets information for the card installed in slot 'intSlot' of the keithley system switch.
    '               It returns a comma separated string of the model, serial number, Rev, and switch closures.  Each row of
    '               switch closures is averaged; backplane closures are reported individually.
    Public Function CardInfo(intCard As Integer) As String
        Dim strCardInfo As String
        Dim strIDNString As String
        Dim aryIDN() As String
        Dim intRow As Integer       'row counter
        Dim intCol As Integer       'col counter
        Dim intCounts As Integer    'closure counts

        'Get Card IDN, check if empty
        strIDNString = SwitchIOWriteRead("print(slot[" & intCard & "].idn)")
        If strIDNString.Contains("Empty Slot") Then
            strCardInfo = "Empty Slot"
            Return strCardInfo
        Else
            'Add Model, SN, and Rev to strCardInfo
            aryIDN = Split(strIDNString, ",")
            strCardInfo = aryIDN(0) + "," + aryIDN(3) + "," + aryIDN(2)

            'Calculate Ave closure counts for each row
            For intRow = 1 To 6
                intCounts = 0
                For intCol = 1 To 16
                    intCounts = intCounts + SwitchIOWriteRead("print(channel.getcount('" & intCard & intRow & Format(intCol, "00") & "'))")
                Next
                intCounts = intCounts / 16
                strCardInfo = strCardInfo + "," + CStr(intCounts)
            Next

            'Get backplan closure counts for each row
            For intRow = 1 To 6
                strCardInfo = strCardInfo + "," + SwitchIOWriteRead("print(channel.getcount('" & intCard & "91" & intRow & "'))")
            Next

            Return strCardInfo

        End If

    End Function


End Module
