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

        ' Set Buffer Storage-Mode to Overwrite - Added 12Jun2016 DB
        SwitchIOWrite("node[2].smua.nvbuffer1.appendmode = 0")
        SwitchIOWrite("node[2].smua.nvbuffer2.appendmode = 0")
        SwitchIOWrite("node[2].smub.nvbuffer1.appendmode = 0")
        SwitchIOWrite("node[2].smub.nvbuffer2.appendmode = 0")

        ' Set the Autozero for both channels to autozero once
        SwitchIOWrite("node[2].smua.measure.autozero = 1") 'autozero once
        SwitchIOWrite("node[2].smub.measure.autozero = 1") 'autozero once

        'Turn on SourceMeter
        SwitchIOWrite("node[2].smua.source.output = 1")
        SwitchIOWrite("node[2].smub.source.output = 1")
    End Sub
    Public Sub EndTest()

        'Things to do if communication is open:
        If boolIOStatus Then
            'Open all switches
            SwitchIOWrite("channel.open('allslots')")

            'Turn off the sourcemeters
            SwitchIOWrite("node[2].smua.source.output = 0")
            SwitchIOWrite("node[2].smub.source.output = 0")

            'Close communication
            CloseKeithleyIO()
        End If

        'Things to do if data file is open
        If boolDataFileOpen Then
            'Close the data file
            CloseDataFile()
        End If

        'Re-enable the frmMainForm buttons
        With frmMain
            .btnConfig.Enabled = True
            .btnNewTest.Enabled = True
        End With

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

    '' Description: Checks user inputs against a set of formatting / data type rules
    'Private Function ValidateForm() As Boolean
    '    ' Check each field against its requirements and return true/false
    '    Try
    '        Dim boolValidates As Boolean = True
    '        ' Check that operator initials are alpha-only and non-empty
    '        Dim regexObj As New System.Text.RegularExpressions.Regex("^[a-zA-Z][a-zA-Z]*$")
    '        If Not (regexObj.IsMatch(txtOperatorInitials.Text)) Then
    '            boolValidates = False
    '        End If

    '        ' Check Test Name.  Must be non-null
    '        If (txtTestName.Text = "") Then
    '            boolValidates = False
    '        End If
    '        ' Check Card tabs.  Must be non-null
    '        For Each aTab In Tabs.TabPages
    '            If (aTab.Enabled) Then
    '                ' check all fields for blankness
    '                For Each aControl In aTab.Controls
    '                    If (aControl.GetType() = GetType(System.Windows.Forms.TextBox) And aControl.Text = "") Then
    '                        boolValidates = False
    '                    End If
    '                Next
    '            End If
    '        Next
    '        Return boolValidates
    '    Catch ex As Exception
    '        GenericExceptionHandler(ex)
    '        Return False
    '    End Try
    'End Function
End Module
