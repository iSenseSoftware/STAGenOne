Option Explicit On
Imports System.Net


Public Module modMain

    Public boolConfigStatus As Boolean
    Public boolIOStatus As Boolean



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
        frmTestName.Hide()

        'Open communication with the measurement hardware
        EstablishKeithleyIO(frmConfig.txtAddress.Text)

        'Open data file
        If Not OpenDataFile(frmConfig.txtDataDir.Text, strBatch & ".csv") Then
            MsgBox("Batch Number already in use.  Please choose another.", MsgBoxStyle.OkOnly, "File already exists.")
            frmTestName.Show()
            Exit Sub
        End If

        'Write Test ID and User initials to data file
        WriteToDataFile("Batch Number:," & strBatch)
        WriteToDataFile("Operator Initials:," & strUserID)

        'Write Hardware Info to data file
        WriteToDataFile(vbCr & "STA ID:," & frmConfig.txtSTAID.Text & vbCr)
        WriteToDataFile(",Model,Serial Number,Firmware Revision,Calibration Date,Cal Due Date")
        strData = String.Format("Source Meter,{0},{1},{2},{3},{4}", _
                                SwitchIOWriteRead("print(node[2].model)"), _
                                SwitchIOWriteRead("print(node[2].serialno)"), _
                                SwitchIOWriteRead("print(node[2].revision)"), _
                                SwitchIOWriteRead("print(node[2].smua.cal.date)"), _
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
            WriteToDataFile(strData & vbCr)
        Next

        'configure hardware for verifcation
        ConfigureHardware()
        WriteToDataFile(",Voltage,Current Range,Filter Type,Filter Count,NPLC,Row3 (Ohms),Row3 Open (nA),Row4 (Ohms)," _
                        & "Row5 (Ohms),Row6 (Ohms),Tollerance (+/- %)")
        WriteToDataFile("Audit Config,0.65,1e-6,FILTER_REPEAT_AVG,1,1," & frmConfig.txtRow3Resistor.Text & "," _
                        & frmConfig.txtAuditZero.Text & "," & frmConfig.txtRow4Resistor.Text & "," _
                        & frmConfig.txtRow6Resistor.Text & "," & frmConfig.txtTolerance.Text)

        'hardware verification
        HardwareVerification()

        'Show test info form
        frmSensorID.Show()

    End Sub

End Module
