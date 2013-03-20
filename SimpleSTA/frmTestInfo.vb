﻿Imports System.IO

Public Class frmTestInfo
    Dim boolSameBatch As Boolean = False
    Private Sub frmTestInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'loadConfiguration()
            If (verifyConfiguration()) Then
                frmMain.SystemStatusLabel.Text = "System Status: Configuration Loaded"
                Dim options As String
                ' An option string must be explicitly declared or the driver throws a COMException.  This may be fixed by firmware upgrades
                options = "QueryInstStatus=true, RangeCheck=true, Cache=true, Simulate=false, RecordCoercions=false, InterchangeCheck=false"
                switchDriver.Initialize(config.Address, False, False, options)
                If (switchDriver.Initialized()) Then
                    MsgBox("System Drivers loaded.  I/O with test system established.")
                    frmMain.SystemStatusLabel.Text = "System Status: Standby; I/O established."
                Else
                    MsgBox("Unable to establish connection to test system.  Verify connections and configuration settings and try again.")
                    Me.Close()
                End If
            Else
                frmMain.SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                Me.Close()
                frmConfig.Show()
                frmConfig.BringToFront()
            End If
            ' Set form control visibility based upon the card configuration 
            Select Case config.CardConfig
                Case CardConfiguration.ONE_CARD_SIXTEEN_SENSORS
                    Me.Tabs.TabPages.Remove(SlotTwoTab)
                    Me.Tabs.TabPages.Remove(SlotThreeTab)
                    Me.Tabs.TabPages.Remove(SlotFourTab)
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                Case CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS
                    Me.Tabs.TabPages.Remove(SlotThreeTab)
                    Me.Tabs.TabPages.Remove(SlotFourTab)
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                Case CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS
                    Me.Tabs.TabPages.Remove(SlotFourTab)
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                Case CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                Case CardConfiguration.FIVE_CARD_EIGHTY_SENSORS
                    Me.Tabs.TabPages.Remove(SlotSixTab)
            End Select
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub btnCreateTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTest.Click
        Try
            If (File.Exists(config.DumpDirectory & Path.DirectorySeparatorChar & txtTestName.Text & ".xml")) Then
                MsgBox("A file with that name already exists.  Choose a new name.")
                txtTestName.SelectAll()
            Else
                updateTestFile()
                frmTestForm.Show()
                Me.Close()
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' This subroutine updates the settings and creates the sensor objects in the currentTestFile global
    Private Sub updateTestFile()
        Try
            'config.DumpDir = txtTestFile.Text
            currentTestFile.Config = config
            currentTestFile.OperatorID = Me.txtOperatorInitials.Text
            currentTestFile.Name = Me.txtTestName.Text
            currentTestFile.DumpFile = config.DumpDirectory & Path.DirectorySeparatorChar & txtTestName.Text & ".xml"
            Select Case config.CardConfig
                Case CardConfiguration.ONE_CARD_SIXTEEN_SENSORS
                    ' Add all sensors
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))
                Case CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))
                Case CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))
                Case CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(4, 1, txtCard4Batch.Text, txtCard4Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 2, txtCard4Batch.Text, txtCard4Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 3, txtCard4Batch.Text, txtCard4Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 4, txtCard4Batch.Text, txtCard4Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 5, txtCard4Batch.Text, txtCard4Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 6, txtCard4Batch.Text, txtCard4Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 7, txtCard4Batch.Text, txtCard4Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 8, txtCard4Batch.Text, txtCard4Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 9, txtCard4Batch.Text, txtCard4Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 10, txtCard4Batch.Text, txtCard4Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 11, txtCard4Batch.Text, txtCard4Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 12, txtCard4Batch.Text, txtCard4Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 13, txtCard4Batch.Text, txtCard4Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 14, txtCard4Batch.Text, txtCard4Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 15, txtCard4Batch.Text, txtCard4Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 16, txtCard4Batch.Text, txtCard4Col16.Text))
                Case CardConfiguration.FIVE_CARD_EIGHTY_SENSORS
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(4, 1, txtCard4Batch.Text, txtCard4Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 2, txtCard4Batch.Text, txtCard4Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 3, txtCard4Batch.Text, txtCard4Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 4, txtCard4Batch.Text, txtCard4Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 5, txtCard4Batch.Text, txtCard4Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 6, txtCard4Batch.Text, txtCard4Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 7, txtCard4Batch.Text, txtCard4Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 8, txtCard4Batch.Text, txtCard4Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 9, txtCard4Batch.Text, txtCard4Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 10, txtCard4Batch.Text, txtCard4Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 11, txtCard4Batch.Text, txtCard4Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 12, txtCard4Batch.Text, txtCard4Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 13, txtCard4Batch.Text, txtCard4Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 14, txtCard4Batch.Text, txtCard4Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 15, txtCard4Batch.Text, txtCard4Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 16, txtCard4Batch.Text, txtCard4Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(5, 1, txtCard5Batch.Text, txtCard5Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 2, txtCard5Batch.Text, txtCard5Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 3, txtCard5Batch.Text, txtCard5Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 4, txtCard5Batch.Text, txtCard5Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 5, txtCard5Batch.Text, txtCard5Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 6, txtCard5Batch.Text, txtCard5Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 7, txtCard5Batch.Text, txtCard5Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 8, txtCard5Batch.Text, txtCard5Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 9, txtCard5Batch.Text, txtCard5Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 10, txtCard5Batch.Text, txtCard5Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 11, txtCard5Batch.Text, txtCard5Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 12, txtCard5Batch.Text, txtCard5Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 13, txtCard5Batch.Text, txtCard5Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 14, txtCard5Batch.Text, txtCard5Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 15, txtCard5Batch.Text, txtCard5Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 16, txtCard5Batch.Text, txtCard5Col16.Text))
                Case CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(4, 1, txtCard4Batch.Text, txtCard4Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 2, txtCard4Batch.Text, txtCard4Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 3, txtCard4Batch.Text, txtCard4Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 4, txtCard4Batch.Text, txtCard4Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 5, txtCard4Batch.Text, txtCard4Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 6, txtCard4Batch.Text, txtCard4Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 7, txtCard4Batch.Text, txtCard4Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 8, txtCard4Batch.Text, txtCard4Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 9, txtCard4Batch.Text, txtCard4Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 10, txtCard4Batch.Text, txtCard4Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 11, txtCard4Batch.Text, txtCard4Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 12, txtCard4Batch.Text, txtCard4Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 13, txtCard4Batch.Text, txtCard4Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 14, txtCard4Batch.Text, txtCard4Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 15, txtCard4Batch.Text, txtCard4Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(4, 16, txtCard4Batch.Text, txtCard4Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(5, 1, txtCard5Batch.Text, txtCard5Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 2, txtCard5Batch.Text, txtCard5Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 3, txtCard5Batch.Text, txtCard5Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 4, txtCard5Batch.Text, txtCard5Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 5, txtCard5Batch.Text, txtCard5Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 6, txtCard5Batch.Text, txtCard5Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 7, txtCard5Batch.Text, txtCard5Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 8, txtCard5Batch.Text, txtCard5Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 9, txtCard5Batch.Text, txtCard5Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 10, txtCard5Batch.Text, txtCard5Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 11, txtCard5Batch.Text, txtCard5Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 12, txtCard5Batch.Text, txtCard5Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 13, txtCard5Batch.Text, txtCard5Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 14, txtCard5Batch.Text, txtCard5Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 15, txtCard5Batch.Text, txtCard5Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(5, 16, txtCard5Batch.Text, txtCard5Col16.Text))

                    currentTestFile.addSensor(Sensor.sensorFactory(6, 1, txtCard6Batch.Text, txtCard6Col1.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 2, txtCard6Batch.Text, txtCard6Col2.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 3, txtCard6Batch.Text, txtCard6Col3.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 4, txtCard6Batch.Text, txtCard6Col4.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 5, txtCard6Batch.Text, txtCard6Col5.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 6, txtCard6Batch.Text, txtCard6Col6.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 7, txtCard6Batch.Text, txtCard6Col7.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 8, txtCard6Batch.Text, txtCard6Col8.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 9, txtCard6Batch.Text, txtCard6Col9.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 10, txtCard6Batch.Text, txtCard6Col10.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 11, txtCard6Batch.Text, txtCard6Col11.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 12, txtCard6Batch.Text, txtCard6Col12.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 13, txtCard6Batch.Text, txtCard6Col13.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 14, txtCard6Batch.Text, txtCard6Col14.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 15, txtCard6Batch.Text, txtCard6Col15.Text))
                    currentTestFile.addSensor(Sensor.sensorFactory(6, 16, txtCard6Batch.Text, txtCard6Col16.Text))
            End Select
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Function ValidateForm() As Boolean
        ' Check each field against its requirements and return true/false
        Try
            Dim boolValidates As Boolean = True

            ' Check that operator initials are alpha-only and non-empty
            Dim regexObj As New System.Text.RegularExpressions.Regex("^[a-zA-Z][a-zA-Z]*$")
            If Not (regexObj.IsMatch(txtOperatorInitials.Text)) Then
                boolValidates = False
            End If

            ' Check Test Name.  Must be non-null
            If (txtTestName.Text = "") Then
                boolValidates = False
            End If
            ' Check Card tabs.  Must be non-null
            For Each aTab In Tabs.TabPages
                If (aTab.Enabled) Then
                    ' check all fields for blankness
                    For Each aControl In aTab.Controls
                        If (aControl.GetType() = GetType(System.Windows.Forms.TextBox) And aControl.Text = "") Then
                            boolValidates = False
                        End If
                    Next
                End If
            Next
            Return boolValidates
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub setCheckBoxes(ByVal val As Boolean)
        chkSameCard1.Checked = val
        chkSameCard2.Checked = val
        chkSameCard3.Checked = val
        chkSameCard4.Checked = val
        chkSameCard5.Checked = val
        chkSameCard6.Checked = val
        boolSameBatch = val
    End Sub
    Private Sub setTextForAll(ByVal val As String)
        txtCard1Batch.Text = val
        txtCard2Batch.Text = val
        txtCard3Batch.Text = val
        txtCard4Batch.Text = val
        txtCard5Batch.Text = val
        txtCard6Batch.Text = val
    End Sub

    Private Sub chkSameCard1_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard1.CheckedChanged
        '' If enabled, set the boolSameBatch = true and copy the contents of the associated text box to all text boxes
        If (chkSameCard1.Checked) Then
            setCheckBoxes(True)
            setTextForAll(txtCard1Batch.Text)
        Else
            setCheckBoxes(False)
        End If
    End Sub
    Private Sub chkSameCard2_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard2.CheckedChanged
        If (chkSameCard2.Checked) Then
            setCheckBoxes(True)
            setTextForAll(txtCard2Batch.Text)
        Else
            setCheckBoxes(False)
        End If
    End Sub
    Private Sub chkSameCard3_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard3.CheckedChanged
        If (chkSameCard3.Checked) Then
            setCheckBoxes(True)
            setTextForAll(txtCard3Batch.Text)
        Else
            setCheckBoxes(False)
        End If
    End Sub
    Private Sub chkSameCard4_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard4.CheckedChanged
        If (chkSameCard4.Checked) Then
            setCheckBoxes(True)
            setTextForAll(txtCard4Batch.Text)
        Else
            setCheckBoxes(False)
        End If
    End Sub
    Private Sub chkSameCard5_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard5.CheckedChanged
        If (chkSameCard5.Checked) Then
            setCheckBoxes(True)
            setTextForAll(txtCard5Batch.Text)
        Else
            setCheckBoxes(False)
        End If
    End Sub
    Private Sub chkSameCard6_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard6.CheckedChanged
        If (chkSameCard6.Checked) Then
            setCheckBoxes(True)
            setTextForAll(txtCard6Batch.Text)
        Else
            setCheckBoxes(False)
        End If
    End Sub

    Private Sub txtCard1Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard1Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        If (boolSameBatch) Then
            setTextForAll(txtCard1Batch.Text)
        End If
    End Sub
    Private Sub txtCard2Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard2Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        If (boolSameBatch) Then
            setTextForAll(txtCard1Batch.Text)
        End If
    End Sub
    Private Sub txtCard3Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard3Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        If (boolSameBatch) Then
            setTextForAll(txtCard1Batch.Text)
        End If
    End Sub
    Private Sub txtCard4Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard4Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        If (boolSameBatch) Then
            setTextForAll(txtCard1Batch.Text)
        End If
    End Sub
    Private Sub txtCard5Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard5Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        If (boolSameBatch) Then
            setTextForAll(txtCard1Batch.Text)
        End If
    End Sub
    Private Sub txtCard6Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard6Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        If (boolSameBatch) Then
            setTextForAll(txtCard1Batch.Text)
        End If
    End Sub
End Class