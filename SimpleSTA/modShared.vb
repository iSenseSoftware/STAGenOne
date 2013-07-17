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

Public Module modShared
    '------------------------------
    ' Define constants and global variables
    '------------------------------
    Public Const strConfigFileName As String = "Config.xml"
    Public Const strSystemInfoFileName As String = "SystemInfo.xml"
    Public Const strApplicationName As String = "GlucoMatrix"
    Public Const strApplicationVersion As String = "1.0"
    Public Const strApplicationDescription As String = "Software for CGM Sensor release testing"

    Public strHardwareErrorList As String 'Lists Hardware Errors 

    Public strAppDir As String = My.Application.Info.DirectoryPath ' The path to the application install directory
    Public cfgGlobal As New clsConfiguration ' Global inexstance of the Configuration object that stores config information for the current session
    'Public tsInfoFile As New TestSystem ' Global instance of the TestSystem object which tracks system ID info (serials, models) and switch counts per card
    Public fCurrentTestFile As File
    'Public fCurrentTestFile As New TestFile ' Global instance of the TestFile object stores output data and writes itself to file
    'Public aryCurrentCards() As Card ' Global array instance of Card objects which represents the Cards in the attached switch
    ' Keithley IVI-COM Driver for communicating with 3700 series system switches.  
    ' This instance is referenced by all functions which communicate with the measurement hardware
    'Public switchDriver As New Ke37XX 'commented out by DB 29May2013 for raw TCP communication
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
            ' Populate the AuditCheck object in the fCurrentTestFile with empty AuditChannel objects
            Dim i As Integer
            Dim z As Integer
            Dim acChannel As New AuditChannel
            'For i = cfgGlobal.CardConfig To 1 Step -1
            For i = 1 To cfgGlobal.CardConfig
                For z = 1 To 16
                    fCurrentTestFile.AuditCheck.AddChannel(acChannel.ChannelFactory(i, z))
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

            For Each acChannel In fCurrentTestFile.AuditCheck.AuditChannels
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
    ' Name: ConfigureHardware()
    ' Parameters:
    '           strVolts: the voltage to set the sourcemeter channels to
    '           strCurrent: the current range and compliance limit to set the sourcemeter to
    '           strFilter: the filter type to set the sourcemeter to
    '           strCount: the number of readings for the filter to use
    '           strNPLC: the Number of PowerLine Cycles for each reading
    ' Description: This subroutine will configure both channels of the SourceMeter to the parameters passed to the subroutine.

    Public Sub ConfigureHardware(strVolts As String, strCurrent As String, strFilter As String, _
                                    strCount As String, strNPLC As String)
        ' Start with all intersections open
        SwitchIOWrite("channel.open('allslots')")
        ' Set connection rule to "make before break"
        SwitchIOWrite("node[1].channel.connectrule = 2")

        ' set both SMU channels to DC volts
        ' Note: The 2602A does not appear to understand the enum variables spelled out in the user manual.  Integers are used instead
        SwitchIOWrite("node[2].smua.source.func = 1")
        SwitchIOWrite("node[2].smub.source.func = 1")

        ' Set the bias for both channels based on the passed value
        SwitchIOWrite("node[2].smua.source.levelv = " & strVolts)
        SwitchIOWrite("node[2].smub.source.levelv = " & strVolts)

        ' Range is hard-coded to 1.  
        SwitchIOWrite("node[2].smua.source.rangev = 1")
        SwitchIOWrite("node[2].smub.source.rangev = 1")

        'Set the current measurement range based on the passed value
        SwitchIOWrite("node[2].smua.source.rangei = " & strCurrent)
        SwitchIOWrite("node[2].smub.source.rangei = " & strCurrent)

        ' disable autorange for both output channels
        SwitchIOWrite("node[2].smua.source.autorangei = 0")
        SwitchIOWrite("node[2].smub.source.autorangei = 0")

        ' Configure the DMM
        SwitchIOWrite("node[2].smua.measure.filter.type = " & strFilter)
        SwitchIOWrite("node[2].smub.measure.filter.type = " & strFilter)
        SwitchIOWrite("node[2].smua.measure.filter.count = " & strCount)
        SwitchIOWrite("node[2].smub.measure.filter.count = " & strCount)
        SwitchIOWrite("node[2].smua.measure.filter.enable = 1")
        SwitchIOWrite("node[2].smub.measure.filter.enable = 1")
        SwitchIOWrite("node[2].smua.measure.nplc = " & strNPLC)
        SwitchIOWrite("node[2].smub.measure.nplc = " & strNPLC)

        ' Set output off mode to OUTPUT_HIGH_Z
        SwitchIOWrite("node[2].smua.source.offmode = 2")
        SwitchIOWrite("node[2].smub.source.offmode = 2")

        ' Clear the non-volatile measurement buffers
        SwitchIOWrite("node[2].smua.nvbuffer1.clear()")
        SwitchIOWrite("node[2].smua.nvbuffer2.clear()")
        SwitchIOWrite("node[2].smub.nvbuffer1.clear()")
        SwitchIOWrite("node[2].smub.nvbuffer2.clear()")

        ' Set the Autozero for both channels to autozero once
        SwitchIOWrite("node[2].smub.measure.autozero = 1") 'autozero once
        SwitchIOWrite("node[2].smub.measure.autozero = 1") 'autozero once

        'Turn on SourceMeter
        SwitchIOWrite("node[2].smua.source.output = 1")
        SwitchIOWrite("node[2].smub.source.output = 1")


    End Sub

    Public Sub HardwareVerification()
        'Display Message to "Open all Fixtures"
        MsgBox("Open all Fixtures")

        Dim dblPassHigh As Double
        Dim dblPassLow As Double


        'Verification of Row 3 Open
        dblPassHigh = CDbl(frmConfig.txtRow3Resistor.Text) * (1 + CDbl(frmConfig.txtTolerance.Text))
        dblPassLow = CDbl(frmConfig.txtRow3Resistor.Text) * (1 - CDbl(frmConfig.txtTolerance.Text))
        RowVerification(3, True, dblPassHigh, dblPassLow)

        'Verification of Row 3 Closed
        dblPassHigh = CDbl(frmConfig.txtRow3Resistor.Text) * (1 + CDbl(frmConfig.txtTolerance.Text))
        dblPassLow = CDbl(frmConfig.txtRow3Resistor.Text) * (1 - CDbl(frmConfig.txtTolerance.Text))
        RowVerification(3, False, dblPassHigh, dblPassLow)

        'Verification of Row 4 Closed
        dblPassHigh = CDbl(frmConfig.txtRow4Resistor.Text) * (1 + CDbl(frmConfig.txtTolerance.Text))
        dblPassLow = CDbl(frmConfig.txtRow4Resistor.Text) * (1 - CDbl(frmConfig.txtTolerance.Text))
        RowVerification(4, False, dblPassHigh, dblPassLow)

        'Verificaiton of Row 5 Closed
        dblPassHigh = CDbl(frmConfig.txtRow5Resistor.Text) * (1 + CDbl(frmConfig.txtTolerance.Text))
        dblPassLow = CDbl(frmConfig.txtRow5Resistor.Text) * (1 - CDbl(frmConfig.txtTolerance.Text))
        RowVerification(5, False, dblPassHigh, dblPassLow)

        'Verification of Row 6 Closed
        dblPassHigh = CDbl(frmConfig.txtRow6Resistor.Text) * (1 + CDbl(frmConfig.txtTolerance.Text))
        dblPassLow = CDbl(frmConfig.txtRow6Resistor.Text) * (1 - CDbl(frmConfig.txtTolerance.Text))
        RowVerification(6, False, dblPassHigh, dblPassLow)

        'Errors decision (yes- display error message box & save error list to file & end test, no- open all switches)
        If boolHardwareError Then
            MsgBox("Hardware Verification Failed" + txtHardwareErrorList) 'List All Errors  NEED TO COMPLETE MSG BOX

            ' Write string to data file
            WriteToDataFile(txtHardwareErrorList)

            EndTest()
        Else
            'Open all switches
            SwitchIOWrite("channel.open('allslots')")
        End If

    End Sub
    ' Name: RowVerification()
    ' Parameters:
    '           intRow: The switch matrix row in which the resistor to be used for testing is wired
    '           boolOpen: Whether the switches are open or closed True=Switches Open, False=Switches closed
    '           dblPassHigh: High end of the Pass criteria for the resistor value
    '           dblPassLow: Low end of the pass criteria for the resistor value
    ' Description: 
    
    Public Sub RowVerification(ByVal intRow As Integer, ByVal boolOpen As Boolean, ByVal dblPassHigh As Double, ByVal dblPassLow As Double)
        'Set Column Counter to 1


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
            fCurrentTestFile.Source.LastTest = DateTime.Now()
            fCurrentTestFile.Switch.LastTest = DateTime.Now()
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
