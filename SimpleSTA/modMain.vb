﻿Option Explicit On
Imports System.Net


Public Module modMain

    Public boolConfigStatus As Boolean
    Public boolIOStatus As Boolean

    Public Const strAuditVolt As String = "0.65"
    Public Const strAuditCurrentRange As String = "0.000001"
    Public Const strAuditFilterType As String = "FILTER_REPEAT_AVG"
    Public Const strAuditFilterCount As String = "1"
    Public Const strAuditNPLC As String = "1"


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

        'Open data file
        If Not OpenDataFile(cfgGlobal.DumpDirectory, strBatch & ".csv") Then
            MsgBox("Batch Number already in use.  Please choose another.", MsgBoxStyle.OkOnly, "File already exists.")
            frmTestName.Show()
            frmTestName.txtTestID.Select()
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
            'Show test info form
            frmSensorID.Show()
        Else
            Exit Sub
        End If

    End Sub

End Module
