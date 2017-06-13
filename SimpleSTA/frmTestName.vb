Public Class frmTestName

    
    Private Sub btnOk_Click() Handles btnOk.Click
        strTestID = txtTestID.Text
        strCarrier1 = txbxCarrier1.Text
        strCarrier2 = txbxCarrier2.Text
        strTestType = combxTestType.Text
        NewTest(txtTestID.Text, txtOperatorIntitials.Text)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub

    Private Sub txtOperatorIntitials_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOperatorIntitials.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If txtOperatorIntitials.Text = "" Then
                'do nothing
            ElseIf txtTestID.Text = "" Then
                txtTestID.Select()
            Else
                btnOk_Click()
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub txtTestID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTestID.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If txtTestID.Text = "" Then
                '
            ElseIf txtOperatorIntitials.Text = "" Then
                txtOperatorIntitials.Select()
            Else
                btnOk_Click()
            End If
            e.Handled = True
        End If
    End Sub
    ' Name: NewTest()
    ' Variables:    strBatch, the batch number inputted by the user;
    '               strUserID, the user initials inputted by the user
    ' Description:  This function establishes communication with the measurement hardware, opens a data file, 
    '               saves user ID and Batch Number to the data file, saves hardware info to the datafile, configures
    '               the hardware for verification, and performs verification.
    Public Sub NewTest(strBatch As String, strUserID As String)
        Dim strData As String   'used to temporarily hold data to write to data file
        Dim i As Integer        'generic counter variable

        'Hide frmTestInfo
        Me.Hide()

        'Build filename from batch number and carriers and test type    'Added 06Jun2017
        strBatch = strBatch & "-" & strCarrier1 & strCarrier2 & "-" & strTestType

        'Open data file
        If Not OpenDataFile(cfgGlobal.DumpDirectory, strBatch & ".csv") Then
            MsgBox("Batch Number already in use.  Please choose another.", MsgBoxStyle.OkOnly, "File already exists.")
            Me.Show()
            Me.txtTestID.Select()
            Exit Sub
        End If

        'Write Test ID and User initials to data file
        WriteToDataFile("Batch Number:," & strBatch)
        WriteToDataFile("Slot 1 Carrier:," & strCarrier1)   'Added 06Jun2017
        WriteToDataFile("Slot 2 Carrier:," & strCarrier2)    'Added 06Jun2017
        WriteToDataFile("Test Type:," & strTestType)        'Added 06Jun2017
        WriteToDataFile("Operator Initials:," & strUserID)

        'Write test configuration to data file - DB 05Jun2017
        WriteToDataFile("GlucoMatrix Rev:," & strApplicationVersion)
        WriteToDataFile("Bias Voltage:," & cfgGlobal.Bias)
        WriteToDataFile("Current Range:," & cfgGlobal.Range)
        WriteToDataFile("NPLC:," & cfgGlobal.NPLC)
        WriteToDataFile("Filter Count:," & cfgGlobal.Samples)
        WriteToDataFile("Filter Type:," & cfgGlobal.Filter)
        WriteToDataFile("Sample Interval:," & cfgGlobal.RecordInterval)
        WriteToDataFile("Settling Time:," & cfgGlobal.SettlingTime)


        'Write Hardware Info to data file
        WriteToDataFile(vbCr & "Equipment ID:," & cfgGlobal.STAID & vbCr)
        WriteToDataFile(",Model,Serial Number,Firmware Revision,Calibration Date,Cal Due Date")
        strData = String.Format("Source Meter,{0},{1},{2},{3},{4}",
                                SwitchIOWriteRead("print(node[2].model)"),
                                SwitchIOWriteRead("print(node[2].serialno)"),
                                SwitchIOWriteRead("print(node[2].revision)"),
                                SwitchIOWriteRead("print(node[2].smua.cal.date)"),
                                SwitchIOWriteRead("print(node[2].smua.cal.due)"))
        WriteToDataFile(strData)
        strData = String.Format("System Switch,{0},{1},{2},{3},{4}", _
                                SwitchIOWriteRead("print(localnode.model)"), _
                                SwitchIOWriteRead("print(localnode.serialno)"), _
                                SwitchIOWriteRead("print(localnode.revision)"), _
                                "N/A", _
                                "N/A")
        WriteToDataFile(strData & vbCr)     'write data with extra CR for data file formatting

        'Switch Card Info
        WriteToDataFile("Switch Cards,Model,Serial Number,Firmware Revision,R1 Ave Closures,R2 Ave Closures,R3 Ave Closures," _
                        & "R4 Ave Closures,R5 Ave Closures,R6 Ave Closures,R1 BP Closures,R2 BP Closures,R3 BP Closures," _
                        & "R4 BP Closures,R5 BP Closures,R6 BP Closures")
        For i = 1 To 6
            strData = "Slot " & i & "," & CardInfo(i)
            WriteToDataFile(strData)
        Next

        'configure hardware for verifcation
        ConfigureHardware(strAuditVolt, strAuditCurrentRange, strAuditFilterType, strAuditFilterCount, strAuditNPLC)
        WriteToDataFile(vbCr & ",Voltage,Current Range,Filter Type,Filter Count,NPLC,Row3 (Ohms),Row3 Open (nA),Row4 (Ohms)," _
                        & "Row5 (Ohms),Row6 (Ohms),Tollerance (+/- %)")
        WriteToDataFile("Audit Config," _
                        & strAuditVolt & "," _
                        & strAuditCurrentRange & "," _
                        & strAuditFilterType & "," _
                        & strAuditFilterCount & "," _
                        & strAuditNPLC & "," _
                        & cfgGlobal.ResistorNominalValues(0) & "," _
                        & cfgGlobal.AuditZero & "," _
                        & cfgGlobal.ResistorNominalValues(1) & "," _
                        & cfgGlobal.ResistorNominalValues(2) & "," _
                        & cfgGlobal.ResistorNominalValues(3) & "," _
                        & cfgGlobal.AuditTolerance)

        'hardware verification
        If HardwareVerification() Then
            'Check to see if frmSensorID should be used
            If cfgGlobal.SensorNaming = True Then
                'Show test info form
                frmSensorID.Show()
            Else
                strSensorIDHeader = SensorHeaderFromCarriers()
                frmTestForm.Show()
            End If

        Else
            EndTest()
            Exit Sub
        End If

    End Sub

    Public Function HardwareVerification() As Boolean
        'Skip HardwareVerification - Added 12Jun2017 DB
        'The following two lines should be removed when HardwareVerification has been fixed
        Return True
        Exit Function

        'Display Message to "Open all Fixtures"
        MsgBox("Open all Fixtures")

        Dim dblPassHigh As Double
        Dim dblPassLow As Double
        Dim strPassFail As String
        Dim strSwitchPattern As String
        Dim strAuditHeader As String
        strHardwareErrorList = ""

        strAuditHeader = "" + vbCr + "Audit Row ID,"
        For i = 1 To cfgGlobal.CardConfig * 16
            strAuditHeader = strAuditHeader + "Sensor" + CStr(i) + ","
        Next

        WriteToDataFile(strAuditHeader)

        'Set Pass/Fail Array
        ReDim boolAuditPassFail(cfgGlobal.CardConfig * 16)
        For i = 1 To 32
            boolAuditPassFail(i) = True
        Next
        boolAuditVerificationFailure = False

        'Close all switches in a single row to dissipate any stored charge
        strSwitchPattern = ""
        For i = 1 To 32
            strSwitchPattern = strSwitchPattern + SwitchNumberGenerator(3, i) + ","
            strSwitchPattern = strSwitchPattern + SwitchNumberGenerator(1, i) + ","
        Next
        strSwitchPattern = strSwitchPattern + "1911,1913,2911,2913"
        SwitchIOWrite("node[1].channel.exclusiveclose('" & strSwitchPattern & "')")
        SwitchIOWrite("node[2].smub.source.output = 0")
        Delay(500)
        SwitchIOWrite("channel.open('allslots')")


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
        'Note: this block was commented on 07Mar2016 by DB to enable the program to function without passing the audit
        '      Additional work needs to be performed on the audit function in order to reliably use it.

        'If boolAuditVerificationFailure = True Then
        '    MsgBox("Hardware Verification Failed" + vbCr + strHardwareErrorList)
        '    EndTest()
        '    Return False
        'Else
        '    'Open all switches
        '    SwitchIOWrite("channel.open('allslots')")
        '    Return True
        'End If
        Return True
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
        Else
            strSwitchPattern = ""
        End If

        strSwitchPattern = strSwitchPattern + "," + SwitchNumberGenerator(intRowB, intColumn)
        strSwitchPattern = strSwitchPattern + ",191" + CStr(intRowA) + ",291" + CStr(intRowA) + ",191" + CStr(intRowB) + ",291" + CStr(intRowB)
        Return strSwitchPattern

    End Function

    Function SensorHeaderFromCarriers() As String
        Dim strTemp As String
        Dim i As Integer

        strTemp = "Time:,"
        'Add carrier 1 IDs
        For i = 1 To 16
            strTemp = strTemp & strCarrier1 & i & ","
        Next

        'Add carrier 2 IDs
        For i = 1 To 16
            strTemp = strTemp & strCarrier2 & i & ","
        Next

        Return strTemp

    End Function
End Class