Option Explicit On
Imports System.IO
Imports System.Xml.Serialization
'Imports Keithley.Ke37XX.Interop
Imports System.Runtime.InteropServices
'Imports Ivi.Driver.Interop
Imports System.Net.Sockets

' -----------------------------------------------------------------------------------------------------------
' The SharedModule is, as the name suggests, a collection of shared utlity functions, enumerated variables
' and global variables for use in all objects and forms
' -----------------------------------------------------------------------------------------------------------

Public Module SharedModule
    '------------------------------
    ' Define constants and global variables
    '------------------------------
    Public Const strConfigFileName As String = "Config.xml"
    Public Const strSystemInfoFileName As String = "SystemInfo.xml"
    Public Const strApplicationName As String = "GlucoMatrix"
    Public Const strApplicationVersion As String = "1.0"
    Public Const strApplicationDescription As String = "Software for CGM Sensor release testing"

    Public strAppDir As String = My.Application.Info.DirectoryPath ' The path to the application install directory
    Public cfgGlobal As New Configuration ' Global inexstance of the Configuration object that stores config information for the current session
    Public tsInfoFile As New TestSystem ' Global instance of the TestSystem object which tracks system ID info (serials, models) and switch counts per card
    Public tfCurrentTestFile As New TestFile ' Global instance of the TestFile object stores output data and writes itself to file
    Public aryCurrentCards() As Card ' Global array instance of Card objects which represents the Cards in the attached switch
    ' Keithley IVI-COM Driver for communicating with 3700 series system switches.  
    ' This instance is referenced by all functions which communicate with the measurement hardware
    'Public switchDriver As New Ke37XX 'commented out by DB 29May2013 for raw TCP communication
    Public switchDriver As New TcpClient
    Public switchStream As NetworkStream
    ' The admin password to unlock the configuration settings is hardcoded.  In the future
    ' it may be desireable to incorporate database-driven user authentication / authorization for granular permissions
    Public strAdminPassword As String = "C0balt22"
    ' -----------------------------------------------
    ' Define Enumerated variables
    ' ----------------------------------------------
    ' NOTE: Because of a quirk of VB, a value of 0 for an enumerated variable is equivalent to Nothing, making validation difficult
    ' NOTE: When the CurrentRange and FilterType enums are used for direct input to the test system switch we must subtract 1
    Public Enum CurrentRange
        one_uA = 1
        ten_uA = 2
        hundred_uA = 3
    End Enum
    Public Enum FilterType
        FILTER_MOVING_AVG = 1
        FILTER_REPEAT_AVG = 2
        FILTER_MEDIAN = 3
    End Enum
    Public Enum CardConfiguration
        ONE_CARD_SIXTEEN_SENSORS = 1
        TWO_CARD_THIRTY_TWO_SENSORS = 2
        THREE_CARD_FOURTY_EIGHT_SENSORS = 3
        FOUR_CARD_SIXTY_FOUR_SENSORS = 4
        FIVE_CARD_EIGHTY_SENSORS = 5
        SIX_CARD_NINETY_SIX_SENSORS = 6
    End Enum
    ' -----------------------------------------------------------------
    ' Device I/O and Configuration Functions
    ' -----------------------------------------------------------------
    ' Name: EstablishIO()
    ' Returns: Boolean: Indicates success / failure
    ' Description: Attempts to initialize the switchDriver with hard-coded configuration settings
    Public Function EstablishIO() As Boolean
        Try
            'If (switchDriver Is Nothing) Then
            ' switchDriver = New Ke37XX
            ' End If
            If (frmMain.chkConfigStatus.Checked) Then
                If (switchDriver.Connected) Then
                    frmMain.chkIOStatus.Checked = True
                    Return True
                Else
                    'Dim strOptions As String
                    ' An option string must be explicitly declared or the driver throws a COMException.
                    'strOptions = "QueryInstStatus=false, RangeCheck=false, Cache=false, Simulate=false, RecordCoercions=false, InterchangeCheck=false" '
                    'switchDriver.Initialize(cfgGlobal.Address, False, False, strOptions)
                    SwitchIOOpen(cfgGlobal.Address)
                    If (switchDriver.Connected) Then
                        ' reset the TSPLink so we can communicate with the source meter
                        SwitchIOWrite("tsplink.reset()")
                        SwitchIOWrite("node[2].display.clear()")
                        SwitchIOWrite("node[2].display.settext('TspLink Reset')")
                        'Update UI and return true
                        frmMain.chkIOStatus.Checked = True
                        Return True
                    Else
                        ' Update the UI and return false
                        frmMain.chkIOStatus.Checked = False
                        Return False
                    End If
                End If
            Else
                ' Update the UI and return false
                frmMain.chkIOStatus.Checked = False
                Return False
            End If
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            ' Update the UI and return false
            frmMain.chkIOStatus.Checked = False
            switchDriver.Close()
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            ' Update the UI and return false
            frmMain.chkIOStatus.Checked = False
            Return False
        End Try
    End Function
    ' Name: SwitchIOOpen()
    ' Parameters:
    '           strIPAddress: A string containing the IP Address of the measurement hardware
    ' Description: This command opens a TCP network stream to the Keithley 3700 and prepares it for communication
    Public Sub SwitchIOOpen(ByVal strIPAddress As String)
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

            SetSwitchPatterns(33)

        Catch ex As Exception
            ' Rethrow the exception to the calling function
            Throw
        End Try
    End Sub
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

            'Do
            '    switchStream.Read(byteReadBuffer, 0, 1)
            '    strReadBuffer = System.Text.Encoding.ASCII.GetString(byteReadBuffer, 0, 1)
            '    If strReadBuffer = vbLf Then
            '        Exit Do
            '    Else
            '        strMessage = strMessage + strReadBuffer
            '    End If
            'Loop

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

            'Dim bytes As Int32 = switchStream.Read(byteMessage, 0, byteMessage.Length)
            'Dim responseData As [String] = [String].Empty
            'strResponseData = System.Text.Encoding.ASCII.GetString(byteMessage, 0, bytes)

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

        'Try
        '    'Check for error message in the error queue
        'SwitchIOSend("print(errorqueue.count)")
        strErrorCheck = SwitchIOReceive()
        If strErrorCheck <> "TSP>" Then
            Throw New COMException(strErrorCheck)
        End If

        'Catch comex As COMException
        '    ' Rethrow the exception to the calling function
        '    Throw
        'End Try
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
            strMessage = Left(strMessage, strMessage.Length - 5)
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

    ' Name: SetSwitchPatterns()
    ' Parameters:
    '           strCommand: A string containing the tsp command to be sent to the measurement hardware
    ' Description: This command will send a command to the measurement hardware and check to see if an error is generated.
    Public Sub SetSwitchPatterns(intSensors As Integer)
        Dim strPattern As String
        Dim strPatternName As String
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
            strPatternName = "Sensor" & i
            SwitchIOWrite("channel.pattern.setimage(" & strPattern & ", '" & strPatternName & "')")
        Next

        'generate the switch closure patterns for all closed on row 1
        strPattern = "'"
        For j = 1 To intSensors - 1
            strPattern = strPattern + SwitchNumberGenerator(1, j) + ","
        Next
        strPattern = strPattern + "1911,1912,2911,2912'"
        strPatternName = "Sensor" & intSensors
        SwitchIOWrite("channel.pattern.setimage(" & strPattern & ", '" & strPatternName & "')")


        'generate the all closed


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
    ' Name: RunAuditCheck()
    ' Returns: Boolean: Indicates success of failure (Note: This indicates whether the check has been successfully performed, not whether or not it passed!)
    ' Description: Cycles through all columns in attached switch matrix cards and connects each to a series of resistors, measuring i and v
    '           and storing the readings in an AuditCheck object
    Public Function RunAuditCheck() As Boolean
        Try
            ' Start with all intersections open
            SwitchIOWrite("channel.open('allslots')")
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[2].display.settext('Running Self Check')")
            ' set both SMU channels to DC volts
            ' Note: The 2602A does not appear to understand the enum variables spelled out in the user manual.  Integers are used instead
            SwitchIOWrite("node[2].smub.source.func = 1")
            ' Set the bias for both channels based on the value in cfgGlobal
            SwitchIOWrite("node[2].smub.source.levelv = " & cfgGlobal.Bias)
            ' Range is hard-coded to 1.  Does this need to be a Configuration setting in the future?
            SwitchIOWrite("node[2].smub.source.rangev = 1")
            ' disable autorange for both output channels
            SwitchIOWrite("node[2].smub.source.autorangei = 0")
            ' Populate the AuditCheck object in the tfCurrentTestFile with empty AuditChannel objects
            Dim i As Integer
            Dim z As Integer
            Dim acChannel As New AuditChannel
            'For i = cfgGlobal.CardConfig To 1 Step -1
            For i = 1 To cfgGlobal.CardConfig
                For z = 1 To 16
                    tfCurrentTestFile.AuditCheck.AddChannel(acChannel.ChannelFactory(i, z))
                Next
            Next
            SwitchIOWrite("node[2].smub.source.output = 1")

            ' Set connection rule to "make before break"
            ' @TODO: This is the setting from the old software.  Should this be changed?
            SwitchIOWrite("node[1].channel.connectrule = 2")
            'SwitchIOWrite("node[2].display.clear()")
            ' Configure the DMM
            SwitchIOWrite("node[2].smua.measure.filter.type = " & cfgGlobal.Filter - 1)
            SwitchIOWrite("node[2].smub.measure.filter.type = " & cfgGlobal.Filter - 1)
            SwitchIOWrite("node[2].smua.measure.filter.count = " & cfgGlobal.Samples)
            SwitchIOWrite("node[2].smub.measure.filter.count = " & cfgGlobal.Samples)
            SwitchIOWrite("node[2].smua.measure.filter.enable = 1")
            SwitchIOWrite("node[2].smub.measure.filter.enable = 1")
            SwitchIOWrite("node[2].smua.measure.nplc = " & cfgGlobal.NPLC)
            SwitchIOWrite("node[2].smub.measure.nplc = " & cfgGlobal.NPLC)
            ' Set output off mode to OUTPUT_HIGH_Z
            SwitchIOWrite("node[2].smua.source.offmode = 2")
            SwitchIOWrite("node[2].smub.source.offmode = 2")
            'DirectIOWrapper("node[2].smua.source.output = 0")
            ' Clear the non-volatile measurement buffers
            SwitchIOWrite("node[2].smub.nvbuffer1.clear()")
            SwitchIOWrite("node[2].smub.nvbuffer2.clear()")
            ' NOTE: Should this be changed in the future to be adaptive rather than hard coded?
            SwitchIOWrite("node[2].smub.source.rangei = .000001")
            SwitchIOWrite("node[2].smub.measure.autozero = 1") 'autozero once

            For Each acChannel In tfCurrentTestFile.AuditCheck.AuditChannels
                'Take readings from first resistor
                GatherAuditReading(4, acChannel)
                GatherAuditReading(5, acChannel)
                GatherAuditReading(6, acChannel)
                GatherAuditReading(3, acChannel)
                GatherAuditReading(3, acChannel, True)
                ' Add three switches to total
                tsInfoFile.AddSwitchEvent(acChannel.Card, 6)
            Next
            ' Turn off output to source meter channels
            SwitchIOWrite("node[2].smub.source.output = 0 node[2].smua.source.output = 0")
            Return True
        Catch ex As COMException
            ComExceptionHandler(ex)
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    ' Name: GatherAuditReading()
    ' Parameters:
    '           intRow: The switch matrix row in which the resistor to be used for testing is wired
    '           acChannel: The AuditChannel object to which the AuditReading generated by this sub is added
    ' Description: 
    Public Sub GatherAuditReading(ByVal intRow As Integer, ByRef acChannel As AuditChannel, Optional ByVal boolLastCheck As Boolean = False)
        Try
            If (intRow < 3 Or intRow > 6) Then
                Throw New Exception("Row value given as first parameter to GatherAuditReading was not a valid auditing channel")
            End If
            Dim dblCurrent As Double
            Dim dblVoltage As Double
            'switchDriver.System.DirectIO.FlushRead()
            If boolLastCheck Then
                SwitchIOWrite("node[1].channel.exclusiveclose('" & acChannel.Card & intRow & StrPad(acChannel.Column, 2) & "," & acChannel.Card & "91" & intRow & "')")
            Else
                SwitchIOWrite("node[1].channel.exclusiveclose('" & acChannel.Card & 2 & StrPad(acChannel.Column, 2) & "," & acChannel.Card & intRow & StrPad(acChannel.Column, 2) & "," & acChannel.Card & "912," & acChannel.Card & "91" & intRow & "')")
            End If
            Delay(cfgGlobal.SettlingTime)
            Debug.Print("Start of Audit Reading: " & DateTime.Now.Second & ":" & DateTime.Now.Millisecond)
            SwitchIOWrite("node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2)")
            'node[1].channel.exclusiveclose('1101,1911') node[2].smub.measure.iv(node[2].smub.nvbuffer1, node[2].smub.nvbuffer2) printbuffer(1, 1, node[2].smub.nvbuffer1)
            dblCurrent = CDbl(SwitchIOWriteRead("printbuffer(1, 1, node[2].smub.nvbuffer1)"))
            'switchDriver.System.DirectIO.FlushRead()
            dblVoltage = CDbl(SwitchIOWriteRead("printbuffer(1, 1, node[2].smub.nvbuffer2)"))
            'switchDriver.System.DirectIO.FlushRead()
            Debug.Print("End of Audit Reading: " & DateTime.Now.Second & ":" & DateTime.Now.Millisecond & vbLf)
            acChannel.AddReading(AuditReading.ReadingFactory(dblVoltage, dblCurrent, cfgGlobal.ResistorNominalValues(intRow - 3), intRow, boolLastCheck))
        Catch ex As Exception
            Throw
        End Try
    End Sub
    ' Name: LoadOrRefreshConfiguration()
    ' Returns: Boolean: Indicates success or failure
    ' Description: This function attempts to load cfgGlobal from the configuration file location in memory.  If this is
    '           unsuccessful, it will generate and attempt to save a new configuration object.
    Public Function LoadOrRefreshConfiguration() As Boolean
        Try
            If Not File.Exists(strAppDir & Path.DirectorySeparatorChar & strConfigFileName) Then
                ' If the config file cannot be found, update the UI and create a new object from defaults
                frmMain.chkConfigStatus.Checked = False
                cfgGlobal = New Configuration
            Else
                ' If the file exists, attempt to serialize it to the cfgGlobal object
                Dim srReader As New StreamReader(strAppDir & Path.DirectorySeparatorChar & strConfigFileName)
                Dim xsSerializer As New XmlSerializer(cfgGlobal.GetType)
                cfgGlobal = xsSerializer.Deserialize(srReader)
                srReader.Close()
            End If
            ' Attempt to validate the object
            If cfgGlobal.Validate() Then
                ' If it passes, attempt to write it to file
                If (cfgGlobal.WriteToFile(strAppDir & Path.DirectorySeparatorChar & strConfigFileName)) Then
                    ' Update the UI and return true
                    frmMain.chkConfigStatus.Checked = True
                    Return True
                Else
                    ' update the UI and return false
                    frmMain.chkConfigStatus.Checked = False
                    Return False
                End If
            Else
                ' update the UI and return false
                frmMain.chkConfigStatus.Checked = False
                Return False
            End If
        Catch parseException As InvalidOperationException
            MsgBox("Invalid configuration file.  Delete " & strAppDir & Path.DirectorySeparatorChar & strConfigFileName & " and reload.")
            ' update the UI and return false
            frmMain.chkConfigStatus.Checked = False
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            ' update the UI and return false
            frmMain.chkConfigStatus.Checked = False
            Return False
        End Try
    End Function
    ' Name: LoadAndUpdateSystemInfo()
    ' Returns: Boolean: Indicates success / failure
    ' Description: Attempts to load tsInfoFile from file or, if file does not exist, initializes from defaults.  The function then
    '               attempts to update the object with the current measurement hardware setup and writes the object to file.
    Public Function LoadAndUpdateSystemInfo() As Boolean
        Dim boolOut As Boolean = False
        Dim srReader As StreamReader
        Try
            If (frmMain.chkConfigStatus.Checked) Then
                If (switchDriver.Connected) Then
                    If (File.Exists(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)) Then
                        srReader = New StreamReader(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)
                        Dim serializer As New XmlSerializer(tsInfoFile.GetType)
                        tsInfoFile = serializer.Deserialize(srReader)
                        srReader.Close()
                    Else
                        ' Generate a new one
                        If Not InitializeSystemInfo() Then
                            frmMain.chkSysInfoStatus.Checked = boolOut
                            Return boolOut
                        End If
                    End If
                    If (tsInfoFile.Validate()) Then
                        If (UpdateSystemInfo()) Then
                            If (tsInfoFile.writeToFile(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)) Then
                                boolOut = True
                            End If
                        End If
                    End If
                End If
            End If
            frmMain.chkSysInfoStatus.Checked = boolOut
            Return boolOut
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            frmMain.chkSysInfoStatus.Checked = False
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            frmMain.chkSysInfoStatus.Checked = False
            Return False
        End Try
    End Function
    ' Name: InitializeSystemInfo()
    ' Returns: Boolean: Indicates success or failure
    ' Description: Attempts to initialize a new instance of TestSystem to tsInfoFile and serialize the object to file
    Public Function InitializeSystemInfo() As Boolean
        Try
            System.IO.Directory.CreateDirectory(cfgGlobal.SystemFileDirectory)
            tsInfoFile = New TestSystem
            If (tsInfoFile.writeToFile(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    ' Name: UpdateSystemInfo()
    ' Returns: Boolean: Indicates success of failure
    ' Description:
    ' Updates the tsInfoFile and tfCurrentTestFilewith identifying information for the current connected hardware
    ' and toggles the Active attribute to denote which hardware is current vs historical
    Public Function UpdateSystemInfo() As Boolean
        Try
            Dim serialNo As String
            ' Set all existing SystemInfo objects (switches and sources) to inactive
            If Not tsInfoFile.Sources Is Nothing Then
                For Each aSource In tsInfoFile.Sources
                    aSource.Active = False
                Next
            End If
            If Not tsInfoFile.Switches Is Nothing Then
                For Each aSwitch In tsInfoFile.Switches
                    aSwitch.Active = False
                    For Each aCard In aSwitch.Cards
                        aCard.Active = False
                    Next
                Next
            End If
            ' Collect the Switch(localnode) serial number
            'switchDriver.System.DirectIO.FlushRead()
            serialNo = SwitchIOWriteRead("print(localnode.serialno)")
            'switchDriver.System.DirectIO.FlushRead()
            ' If the switch has already been registered in the TestSystem info file, set the value for the swtCurrentSwitch in the 
            ' test file to the existing value and set it to active
            If Not tsInfoFile.GetSwitchBySerial(serialNo) Is Nothing Then
                tfCurrentTestFile.Switch = tsInfoFile.GetSwitchBySerial(serialNo)
                tfCurrentTestFile.Switch.Active = True
            Else
                ' otherwise create a new entry for the current switch
                tfCurrentTestFile.Switch = New Switch
                tfCurrentTestFile.Switch.SerialNumber = serialNo
                tfCurrentTestFile.Switch.Active = True
                tfCurrentTestFile.Switch.FirstTest = Now()
                ' Collect the switch model #
                tfCurrentTestFile.Switch.ModelNumber = SwitchIOWriteRead("print(localnode.model)")
                'switchDriver.System.DirectIO.FlushRead()
                ' Collect the model revision for the switch
                tfCurrentTestFile.Switch.Revision = SwitchIOWriteRead("print(localnode.model)")
                'switchDriver.System.DirectIO.FlushRead()
                ' add the new switch to the tsInfoFile object / file
                tsInfoFile.AddSwitch(tfCurrentTestFile.Switch)
            End If
            ' Collect the serial number for the connected SourceMeter
            'switchDriver.System.DirectIO.FlushRead()
            serialNo = SwitchIOWriteRead("print(node[2].serialno)")
            tfCurrentTestFile.SourceMeterSerial = serialNo
            'switchDriver.System.DirectIO.FlushRead()
            ' If the source has already been registered in the TestSystem info file, set the value for the tfCurrentTestFile.Source in the 
            ' test file to the existing value and set it to active
            If Not tsInfoFile.GetSourceBySerial(serialNo) Is Nothing Then
                tfCurrentTestFile.Source = tsInfoFile.GetSourceBySerial(serialNo)
                tfCurrentTestFile.Source.Active = True
            Else
                ' otherwise create a new entry for the current source meter
                tfCurrentTestFile.Source = New SourceMeter
                tfCurrentTestFile.Source.SerialNumber = serialNo
                tfCurrentTestFile.Source.Active = True
                tfCurrentTestFile.Source.FirstTest = Now()
                ' Collect the source model
                tfCurrentTestFile.Source.ModelNumber = SwitchIOWriteRead("print(node[2].model)")
                'switchDriver.System.DirectIO.FlushRead()
                ' collect the source model revision
                tfCurrentTestFile.Source.Revision = SwitchIOWriteRead("print(node[2].revision)")
                'switchDriver.System.DirectIO.FlushRead()
                ' add the new sourcemeter to the test system info file / object
                tsInfoFile.AddSource(tfCurrentTestFile.Source)
            End If
            aryCurrentCards = Nothing
            ' Gather identifying information for all cards currently installed
            For i = 1 To 6
                AddCardInfo(i)
            Next
            ' Note: We set the Cards array for the swtCurrentSwitch object which, because we already assigned swtCurrentSwitch to the tsInfoFile
            ' Switches array, will be reflected in the tsInfoFile as well
            tfCurrentTestFile.Switch.Cards = aryCurrentCards
            tsInfoFile.writeToFile(cfgGlobal.SystemFileDirectory & Path.DirectorySeparatorChar & strSystemInfoFileName)
            Return True
        Catch comEx As COMException
            ComExceptionHandler(comEx)
            tsInfoFile = Nothing
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            tsInfoFile = Nothing
            Return False
        End Try
    End Function
    ' Name: AddCardInfo()
    ' Parameters:
    '           intSlot: The system switch slot in which the card is installed
    ' Description: Adds the identifying information for the card in the system switch slot given by intSlot
    Public Sub AddCardInfo(ByVal intSlot As Integer)
        Try
            ' Gather identifying information for all cards currently installed
            Dim strIDNString As String
            Dim strSerialNo As String
            Dim intCardIndex As Integer
            'switchDriver.System.DirectIO.FlushRead()
            strIDNString = SwitchIOWriteRead("print(slot[" & intSlot & "].idn)")
            ' The line below can be removed once the currentSwitch object properly serializes in the test file
            strSerialNo = ParseIDN(strIDNString, "Serial")
            'switchDriver.System.DirectIO.FlushRead()
            If Not strIDNString.Contains("Empty Slot") Then
                ' Check to see if the card has already been registered with the swtCurrentSwitch
                If Not tfCurrentTestFile.Switch.GetCardBySerial(strSerialNo) Is Nothing Then
                    ' if it has, add it to the aryCurrentCards array
                    If aryCurrentCards Is Nothing Then
                        ReDim aryCurrentCards(0)
                        intCardIndex = 0
                    Else
                        Dim lngUpper As Long = aryCurrentCards.GetUpperBound(0)
                        ReDim Preserve aryCurrentCards(lngUpper + 1)
                        intCardIndex = lngUpper + 1
                    End If
                    aryCurrentCards(intCardIndex) = tfCurrentTestFile.Switch.GetCardBySerial(strSerialNo)
                    aryCurrentCards(intCardIndex).Active = True
                    aryCurrentCards(intCardIndex).Slot = intSlot
                Else
                    Dim cdNewCard As New Card
                    cdNewCard.SerialNumber = strSerialNo
                    cdNewCard.ModelNumber = ParseIDN(strIDNString, "Model")
                    cdNewCard.Revision = ParseIDN(strIDNString, "Revision")
                    cdNewCard.FirstTest = Now()
                    cdNewCard.Slot = intSlot
                    cdNewCard.Active = True
                    If aryCurrentCards Is Nothing Then
                        ReDim aryCurrentCards(0)
                        intCardIndex = 0
                    Else
                        Dim lngUpper As Long = aryCurrentCards.GetUpperBound(0)
                        ReDim Preserve aryCurrentCards(lngUpper + 1)
                        intCardIndex = lngUpper + 1
                    End If
                    aryCurrentCards(intCardIndex) = cdNewCard
                End If
            Else
                ' Do nothing if there is no card in the slot
            End If
        Catch ex As Exception
            ' Re-throw the exception to the calling function
            Throw
        End Try
    End Sub
    ' ------------------------------------------------------------
    ' Exception Handlers
    ' ------------------------------------------------------------
    ' Name: GenericExceptionHandler()
    ' Parameters:
    '           theException: the generic Exception object from which a (hopefully) useful error message is generated
    ' Description: Generates a generic error message for the input exception
    Public Sub GenericExceptionHandler(ByVal theException As Exception)
        MsgBox(theException.GetType.ToString() & Environment.NewLine & theException.Message & Environment.NewLine & theException.ToString)
    End Sub
    ' Name: ComExceptionHandler()
    ' Parameters:
    '           theException: the COMException object from which a (hopefully) useful error message is generated
    ' Description: COM Exceptions are thrown by COM-based drivers, in this case the Ke37xx driver.
    '           This function queries the instrument for details about the error and generates and error message
    Public Sub ComExceptionHandler(ByRef theException As COMException)
        If theException.ErrorCode <> 0 Then '= IviDriver_ErrorCodes.E_IVI_INSTRUMENT_STATUS Then
            ' ErrorQuery should give us more information
            Dim intErrCode As Integer = 0
            Dim strErrMsg As String = ""
            'switchDriver.Utility.ErrorQuery(intErrCode, strErrMsg)
            SwitchIOSend("errorcode, message = errorqueue.next")
            SwitchIOSend("print(errorcode)")
            intErrCode = CInt(SwitchIOReceive())
            SwitchIOSend("print(message)")
            SwitchIOSend("errorqueue.clear()")
            strErrMsg = SwitchIOReceive()
            If (intErrCode = 0 And strErrMsg = "") Then
                MsgBox("Unknown instrument error occurred")
            Else
                MsgBox("Instrument Error: " & intErrCode & Environment.NewLine & strErrMsg)
            End If
        Else
            ' Print the exception
            If (theException.Message.Contains("Unknown resource")) Then
                MsgBox("Could not establish communication with the System Switch")
            Else
                MsgBox(theException.Message)
            End If
        End If
    End Sub
    ' -------------------------------------------------------------
    ' Utility Functions
    ' -------------------------------------------------------------
    ' Name: Delay()
    ' Parameters:
    '           lngMilliseconds: The length of time to delay execution of further code, in milliseconds (duh!)
    ' Description: Starts a stop watch then launches an empty loop that executes until the stopwatch has reached the specified time.
    Public Sub Delay(ByVal lngMilliseconds As Long)
        Try
            Dim stpWatch As New Stopwatch
            stpWatch.Start()
            Do
                ' Twiddle thumbs
            Loop Until stpWatch.ElapsedMilliseconds >= lngMilliseconds
            stpWatch.Stop()
        Catch ex As Exception
            ' Re-throw the exception to the calling function
            Throw
        End Try
    End Sub
    ' Name: StrPad()
    ' Parameters:
    '           strInput: The string to be padded
    '           intPadTo: The desired length of the padded output, in characters
    ' Returns: String: The input string padded to the desired length with leading zeroes
    Public Function StrPad(ByVal strInput As String, ByVal intPadTo As Integer) As String
        Try
            If (strInput.Length < intPadTo) Then
                Do While (strInput.Length < intPadTo)
                    strInput = "0" & strInput
                Loop
            End If
            Return strInput
        Catch ex As Exception
            ' Re-throw the excpeption to the calling function
            Throw
        End Try
    End Function
    ' Name: IsDouble()
    ' Returns: Boolean: Indicates whether the string taken as input could be coerced to a double
    Public Function IsDouble(ByVal strInput As String) As Boolean
        Try
            ' Attempt to cast the string to a double
            Dim dblDouble As Double = CDbl(strInput)
            ' If no exception is thrown, return true
            Return True
        Catch castEx As InvalidCastException
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    ' Name: IsInteger()
    ' Returns: Boolean: Indicates whether the string taken as input could be coerced to an integer
    Public Function isInteger(ByVal input As String) As Boolean
        Try
            ' Attempt to cast the string as an integer
            Dim aDouble As Integer = CInt(input)
            ' if no exception is thrown, return true
            Return True
        Catch castEx As InvalidCastException
            Return False
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    ' Name: ParseIDN()
    ' Parameters:
    '           strIDNString: The input string to be parsed
    '           strValueDesired: The value to be extracted from the IDNString
    ' Returns: String: The extracted value, or an empty string on failure
    ' Description:
    ' The ParseIDN function parses a string returned from the Keithley instrument command slot[x].idn
    ' to extract the requested identification information
    Public Function ParseIDN(ByVal strIDNString As String, strValueDesired As String) As String
        Try
            ' If the IDN string indicates an empty slot, return n/a
            If (strIDNString.Contains("Empty Slot")) Then
                Return "N/A"
            End If
            Dim arySplitString As String() = Split(strIDNString, ",")
            Dim intIndex As Integer
            Select Case strValueDesired
                Case "Serial"
                    intIndex = 3
                Case "Model"
                    intIndex = 0
                Case "Revision"
                    intIndex = 2
                Case Else
                    Return strIDNString
            End Select
            ' Check that the idn string contained the expected number of elements
            If (arySplitString.Length < 4) Then
                Throw New Exception("The IDN String given was invalid")
            Else
                Return arySplitString(intIndex)
            End If
        Catch ex As Exception
            ' Re-throw the exception to the calling function
            Throw
        End Try
    End Function
    Public Sub SetLastTestForSysInfo()
        Try
            For Each aSource In tsInfoFile.Sources
                If aSource.Active Then
                    aSource.LastTest = DateTime.Now()
                End If
            Next
            tfCurrentTestFile.Source.LastTest = DateTime.Now()
            tfCurrentTestFile.Switch.LastTest = DateTime.Now()
            For Each aSwitch In tsInfoFile.Switches
                If aSwitch.Active Then
                    aSwitch.LastTest = DateTime.Now()
                    For Each aCard In aSwitch.Cards
                        If (aCard.Active) Then
                            aCard.LastTest = DateTime.Now()
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Module
