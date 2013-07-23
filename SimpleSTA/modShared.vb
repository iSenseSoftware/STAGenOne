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
    Public boolAuditPassFail() As Boolean
    Public boolAuditVerificationFailure As Boolean


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
        SwitchIOWrite("node[2].smua.measure.filter.type = node[2].smua." & strFilter)
        SwitchIOWrite("node[2].smub.measure.filter.type = node[2].smub." & strFilter)
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
        SwitchIOWrite("node[2].smua.measure.autozero = 1") 'autozero once
        SwitchIOWrite("node[2].smub.measure.autozero = 1") 'autozero once

        'Turn on SourceMeter
        SwitchIOWrite("node[2].smua.source.output = 1")
        SwitchIOWrite("node[2].smub.source.output = 1")


    End Sub

    Public Function HardwareVerification() As Boolean
        'Display Message to "Open all Fixtures"
        MsgBox("Open all Fixtures")

        Dim dblPassHigh As Double
        Dim dblPassLow As Double
        Dim strPassFail As String
        Dim strSwitchPattern As String
        strHardwareErrorList = ""

        'Set Pass/Fail Array
        ReDim boolAuditPassFail(cfgGlobal.CardConfig * 16)
        For i = 1 To 32
            boolAuditPassFail(i) = True
        Next
        boolAuditVerificationFailure = False

        'Verification of Row 3 Open
        dblPassHigh = CDbl(cfgGlobal.AuditZero) * 10 ^ 9
        dblPassLow = 0
        RowVerification(3, True, dblPassHigh, dblPassLow)

        'Close all switches in a single row to dissipate any stored charge
        strSwitchPattern = ""
        For i = 1 To 32
            strSwitchPattern = strSwitchPattern + SwitchNumberGenerator(1, i) + ","
        Next
        strSwitchPattern = strSwitchPattern + "1911,1913,2911,2913"
        SwitchIOWrite("node[1].channel.exclusiveclose('" & strSwitchPattern & "')")
        SwitchIOWrite("node[2].smua.source.output = 1")
        Delay(100)
        SwitchIOWrite("channel.open('allslots')")

        'Verification of Row 3 Closed
        dblPassHigh = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(0)) * (1 + (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        dblPassLow = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(0)) * (1 - (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        RowVerification(3, False, dblPassHigh, dblPassLow)

        'Verification of Row 4 Closed
        dblPassHigh = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(1)) * (1 + (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        dblPassLow = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(1)) * (1 - (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        RowVerification(4, False, dblPassHigh, dblPassLow)

        'Verificaiton of Row 5 Closed
        dblPassHigh = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(2)) * (1 + (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        dblPassLow = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(2)) * (1 - (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        RowVerification(5, False, dblPassHigh, dblPassLow)

        'Verification of Row 6 Closed
        dblPassHigh = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(3)) * (1 + (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        dblPassLow = CDbl(strAuditVolt) / CDbl(cfgGlobal.ResistorNominalValues(3)) * (1 - (CDbl(cfgGlobal.AuditTolerance))) * 10 ^ 9
        RowVerification(6, False, dblPassHigh, dblPassLow)


        'Write Pass/Fail Data to file
        strPassFail = "Pass/Fail"
        For i = 1 To 32
            If boolAuditPassFail(i) Then
                strPassFail = strPassFail + ",Pass"
            Else
                strPassFail = strPassFail + ",Fail"
            End If
        Next

        WriteToDataFile(strPassFail)

        'Errors decision (yes- display error message box & save error list to file & end test, no- open all switches)
        If boolAuditVerificationFailure = True Then
            MsgBox("Hardware Verification Failed" + vbCr + strHardwareErrorList)

            EndTest()
            Return False
        Else
            'Open all switches
            SwitchIOWrite("channel.open('allslots')")
            Return True
        End If

    End Function
    ' Name: RowVerification()
    ' Parameters:
    '           intRow: The switch matrix row in which the resistor to be used for testing is wired
    '           boolOpen: Whether the switches are open or closed True=Switches Open, False=Switches closed
    '           dblPassHigh: High end of the Pass criteria for the resistor value
    '           dblPassLow: Low end of the pass criteria for the resistor value
    ' Description: 


    Public Sub RowVerification(ByVal intRow As Integer, ByVal boolOpen As Boolean, ByVal dblHigh As Double, ByVal dblLow As Double)

        'Local Variables
        Dim intColumnCounter As Integer
        Dim dblCurrentReading As Double
        Dim strMeasurements As String
        Dim strSwitchPattern As String
        Dim strOpenClosed As String
        Dim strAuditPassFail As String
        Dim intSourceMeter As Integer
        Dim strSourceMeter As String

        'When boolOpen = True will List "Open" in the Audit Configuration Row Identififier about the Switches
        'When boolOpen = False will List "Close" in the Audit Configuration Row Identififier about the Switches
        If boolOpen = True Then
            strOpenClosed = "Open"
        Else
            strOpenClosed = "Close"
        End If

        'Nested Loop to perform row verification
        'Outer Loop sets the Source Meter 1= SMUA, 2= SMUB (the 2 meausement rows)
        'Inner Loop, loops through the 32 sensor channels (the 32 sensor columns)
        For intSourceMeter = 1 To 2


            'When intSourceMeter = 1 will List "SMUA" in the Audit Configuration Row Identififier about the SourceMeter
            'When intSourceMeter = 2 will List "SMUB" in the Audit Configuration Row Identififier about the SourceMeter
            If intSourceMeter = 1 Then
                SwitchIOWrite("node[2].smua.source.output = 1")
                SwitchIOWrite("node[2].smub.source.output = 0")
                strSourceMeter = "SMUA"
            Else
                SwitchIOWrite("node[2].smub.source.output = 1")
                SwitchIOWrite("node[2].smua.source.output = 0")
                strSourceMeter = "SMUB"
            End If

            'String that provides the Audit Configuration Row Identifier
            strMeasurements = "Row" + CStr(intRow) + "_" + strOpenClosed + "_(nA)_" + strSourceMeter

            'When boolOpen = True then the switches on Row need to be open, this loops sets the row under investigation (3-6 of Cards)
            'to be equal to the SourceMeter value (2 measurements rows)


            'Column Counter loop to run through all 32 sensor channels
            For intColumnCounter = 1 To cfgGlobal.CardConfig * 16

                'Special case of Row 3 Open
                'Generate Switch Pattern 
                'intSourceMeter determines which measurement row will be analyzed SMUA or SMUB
                'intRow passed from Hardware Verification Subroutine determines which verification row will be analyzed (rows 3-6, which contain different resistors)
                'intColumn determines wihch sensor channel is being analyzed (sensor channels 1-32)
                strSwitchPattern = AuditPatternGenerator(intSourceMeter, intRow, intColumnCounter, boolOpen)

                'Close Switches based on AuditPatternGenerator Function
                SwitchIOWrite("node[1].channel.exclusiveclose('" & strSwitchPattern & "')")
                Debug.Print("node[1].channel.exclusiveclose('" & strSwitchPattern & "')")

                If intColumnCounter = 1 Then
                    Delay(50)
                End If
                'Record I Reading
                If intSourceMeter = 1 Then
                    dblCurrentReading = CDbl(SwitchIOWriteRead("print(node[2].smua.measure.i())")) * 10 ^ 9
                Else
                    dblCurrentReading = CDbl(SwitchIOWriteRead("print(node[2].smub.measure.i())")) * 10 ^ 9
                End If


                'Add Reading to String
                strMeasurements = strMeasurements + "," + CStr(dblCurrentReading)

                'Verify Measurement against theoretically expected current based on the resistor used
                If dblCurrentReading > dblHigh Or dblCurrentReading < dblLow Then
                    boolAuditPassFail(intColumnCounter) = False

                    boolAuditVerificationFailure = True

                    strAuditPassFail = "Row" + CStr(intRow) + "_" + CStr(intColumnCounter) + "_" + strOpenClosed + "_:" + CStr(dblCurrentReading) + "nA" + vbCr
                    strHardwareErrorList = strHardwareErrorList + "," + strAuditPassFail
                End If

            Next 'intColumnCounter 

            'Write String to Data File
            WriteToDataFile(strMeasurements)

        Next 'intSourceMeter

    End Sub

    Function AuditPatternGenerator(ByVal intRowA As Integer, ByVal intRowB As Integer, ByVal intColumn As Integer, ByVal boolOpen As Boolean) As String
        Dim strSwitchPattern As String

        'Generates the switch pattern needed to be closed to run Row Verification
        '191x and 291x are the backplanes to read across the 6 cards int the STA (x corresponds to which card)
        If boolOpen = False Then
            strSwitchPattern = SwitchNumberGenerator(intRowA, intColumn)
        End If

        strSwitchPattern = strSwitchPattern + "," + SwitchNumberGenerator(intRowB, intColumn)
        strSwitchPattern = strSwitchPattern + ",191" + CStr(intRowA) + ",291" + CStr(intRowA) + ",191" + CStr(intRowB) + ",291" + CStr(intRowB)
        Return strSwitchPattern

    End Function

    Public Sub PopulateSensorID()
        Dim txtTextBoxItem As Control
        For Each txtTextBoxItem In frmSensorID.Controls
            If TypeOf txtTextBoxItem Is TextBox Then
                txtTextBoxItem.Text = frmTestName.txtTestID.Text + "-" + Right(txtTextBoxItem.Name, 2)
            End If

        Next
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
End Module
