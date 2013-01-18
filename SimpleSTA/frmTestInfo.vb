Public Class frmTestInfo

    Private Sub frmTestInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            loadConfiguration()
            ' Set form control visibility based upon the card configuration 
            Select Case config.CardConfig
                Case CardConfiguration.ONE_CARD_SIXTEEN_SENSORS
                    Me.Tabs.TabPages.Remove(SlotTwoTab)
                    Me.Tabs.TabPages.Remove(SlotThreeTab)
                    Me.Tabs.TabPages.Remove(SlotFourTab)
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                    Me.txtCardTwoSerial.Enabled = False
                    Me.txtCardThreeSerial.Enabled = False
                    Me.txtCardFourSerial.Enabled = False
                    Me.txtCardFiveSerial.Enabled = False
                    Me.txtCardSixSerial.Enabled = False
                Case CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS
                    Me.Tabs.TabPages.Remove(SlotThreeTab)
                    Me.Tabs.TabPages.Remove(SlotFourTab)
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                    Me.txtCardThreeSerial.Enabled = False
                    Me.txtCardFourSerial.Enabled = False
                    Me.txtCardFiveSerial.Enabled = False
                    Me.txtCardSixSerial.Enabled = False
                Case CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS
                    Me.Tabs.TabPages.Remove(SlotFourTab)
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                    Me.txtCardFourSerial.Enabled = False
                    Me.txtCardFiveSerial.Enabled = False
                    Me.txtCardSixSerial.Enabled = False
                Case CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS
                    Me.Tabs.TabPages.Remove(SlotFiveTab)
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                    Me.txtCardFiveSerial.Enabled = False
                    Me.txtCardSixSerial.Enabled = False
                Case CardConfiguration.FIVE_CARD_EIGHTY_SENSORS
                    Me.Tabs.TabPages.Remove(SlotSixTab)
                    Me.txtCardSixSerial.Enabled = False
            End Select
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub btnCreateTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTest.Click
        Try
            updateTestFile()
            frmTestForm.Show()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' This subroutine updates the settings and creates the sensor objects in the currentTestFile global
    Private Sub updateTestFile()
        Try
            config.DumpDir = txtTestFile.Text
            currentTestFile.Config = config
            currentTestFile.OperatorID = Me.txtOperatorInitials.Text
            currentTestFile.Name = Me.txtTestName.Text
            currentTestFile.DumpFile = Me.txtTestFile.Text
            currentTestFile.MatrixCardOneSerial = Me.txtCardOneSerial.Text
            currentTestFile.MatrixCardTwoSerial = Me.txtCardTwoSerial.Text
            currentTestFile.MatrixCardThreeSerial = Me.txtCardThreeSerial.Text
            currentTestFile.MatrixCardFourSerial = Me.txtCardFourSerial.Text
            currentTestFile.MatrixCardFiveSerial = Me.txtCardFiveSerial.Text
            currentTestFile.MatrixCardSixSerial = Me.txtCardSixSerial.Text

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
    ' Create a file select dialog so the user can choose the data dump location
    Private Sub btnSelectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectFile.Click
        Try
            SaveFileDialog1.Title = "Select location to save test file"
            SaveFileDialog1.InitialDirectory = "C:\"
            SaveFileDialog1.ValidateNames = True
            SaveFileDialog1.DefaultExt = ".xml"
            SaveFileDialog1.Filter = "XML Files (*.xml)|*.xml"
            SaveFileDialog1.ShowDialog()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub SaveFileDialog1_FileOK(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        Try
            txtTestFile.Text = SaveFileDialog1.FileName
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Function ValidateForm() As Boolean
        ' Check each field against its requirements and return true/false
        Try
            Dim boolValidates As Boolean = True

            ' Check save file field to insure it is non-empty and a valid file path
            If (System.IO.File.Exists(txtTestFile.Text)) Then
                ' do nothing
            Else
                System.IO.File.Create(txtTestFile.Text)
                If Not (System.IO.File.Exists(txtTestFile.Text)) Then
                    boolValidates = False
                End If
            End If

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
            ' Depending on the card configuration, check that the card serial field is non-null
            Select Case config.CardConfig
                Case CardConfiguration.ONE_CARD_SIXTEEN_SENSORS
                    If (txtCardOneSerial.Text = "") Then
                        boolValidates = False
                    End If
                Case CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS
                    If (txtCardOneSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardTwoSerial.Text = "") Then
                        boolValidates = False
                    End If
                Case CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS
                    If (txtCardOneSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardTwoSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardThreeSerial.Text = "") Then
                        boolValidates = False
                    End If
                Case CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS
                    If (txtCardOneSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardTwoSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardThreeSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardFourSerial.Text = "") Then
                        boolValidates = False
                    End If
                Case CardConfiguration.FIVE_CARD_EIGHTY_SENSORS
                    If (txtCardOneSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardTwoSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardThreeSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardFourSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardFiveSerial.Text = "") Then
                        boolValidates = False
                    End If
                Case CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS
                    If (txtCardOneSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardTwoSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardThreeSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardFourSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardFiveSerial.Text = "") Then
                        boolValidates = False
                    End If
                    If (txtCardSixSerial.Text = "") Then
                        boolValidates = False
                    End If
            End Select
            Return boolValidates
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class