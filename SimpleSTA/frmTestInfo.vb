Option Explicit On
Imports System.IO
'Imports Keithley.Ke37XX.Interop

' -----------------------------------------------------------------------------------
' frmTestInfo is the form displayed to the user to gather test-specific information
' such as operator name, save file name and sensor IDs for sensors being tested
' -----------------------------------------------------------------------------------
Public Class frmTestInfo
    Dim boolSameBatch As Boolean = False ' Reflects state of toggle that sets batch to be the same for all sensors
    ' txtOriginator is the text box on the page from which a change to a shared batch originates.  
    ' Used to prevent infinite loops when updating all text boxes at once
    Dim txtOriginator As TextBox
    'Dim switchDriver As New Ke37XX
    ' Name: frmTestInfo_Load
    ' Handles: Runs when the TestInfo form is first shown.
    ' Description: 
    ' 1. Attempts to load the configuration from file or defaults
    ' 2. Attempts to establish a connection with the measurement hardware
    ' 3. Loads existing system info file (or creates new) and updates with current hardware
    Private Sub frmTestInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' Attempt to load configuration
            If (LoadOrRefreshConfiguration()) Then
                If (EstablishIO()) Then
                    '                    tfCurrentTestFile = New TestFile
                    If (LoadAndUpdateSystemInfo()) Then
                        ' do nothing, hooray!
                    Else
                        MsgBox("Unable to read or update test system info file")
                        Me.Close()
                    End If
                Else
                    MsgBox("Unable to establish I/O with the system switch")
                    Me.Close()
                End If
            Else
                MsgBox("Unable to load configuration.")
                Me.Close()
            End If
            ' Set form control visibility based upon the card configuration 
            Select Case cfgGlobal.CardConfig
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
                Case Else
                    ' Do nothing
            End Select
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    ' Name: btnCreateTest_Click()
    ' Handles: User clicks 'Create Test' button
    ' Description:
    '   1. Checks that the filename chosen by the user does not already exist
    '   2. Updates the tfCurrentTestFile with the sensor info in the form
    '   3. Performs a continuity check using a set of resistors in matrix rows 3-6
    '   4. If the check passes, opens the TestForm
    '   5. If it fails, aborts and alerts the user
    Private Sub btnCreateTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateTest.Click
        Try
            If (ValidateForm()) Then
                If (File.Exists(cfgGlobal.DumpDirectory & Path.DirectorySeparatorChar & txtTestName.Text & ".xml")) Then
                    MsgBox("A file with that name already exists.  Choose a new name.")
                    txtTestName.SelectAll()
                Else
                    UpdateTestFile()
                    MsgBox("Preparing to perform system self check.  Make sure all fixtures are open before proceeding", vbOKOnly)
                    'tfCurrentTestFile.AuditCheck = New AuditCheck
                    RunAuditCheck()
                    tfCurrentTestFile.AuditCheck.Validate()
                    If (tfCurrentTestFile.AuditCheck.Pass) Then
                        tfCurrentTestFile.WriteToFile()
                        SwitchIOWrite("node[2].display.clear()")
                        SwitchIOWrite("node[2].display.settext('Ready to test')")
                        frmTestForm.Show()
                        Me.Close()
                    Else
                        ' Make sure the test file is created even with a failure so the user can see why the test failed
                        tfCurrentTestFile.WriteToFile()
                        tfCurrentTestFile = Nothing
                        MsgBox("Self check failed!  Contact instrument owner to determine course of action.")
                        Me.Close()
                    End If
                End If
            Else
                MsgBox("Form validation failed.  Check that all fields are complete.")
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: UpdateTestFile()
    ' Description: Updates the settings and creates the sensor objects in the tfCurrentTestFile global
    Private Sub UpdateTestFile()
        Try
            'tfCurrentTestFile = New TestFile
            tfCurrentTestFile.Config = cfgGlobal
            tfCurrentTestFile.OperatorID = Me.txtOperatorInitials.Text
            tfCurrentTestFile.Name = Me.txtTestName.Text
            tfCurrentTestFile.DumpFile = cfgGlobal.DumpDirectory & Path.DirectorySeparatorChar & txtTestName.Text & ".xml"
            ' Here's an instance where VB makes me long for PHP because variable interpolation and the
            ' capability to call variables with a string containing their name would reduce this down to 
            ' a simple loop.  To whit:
            ' function populateTestFileWithSensors($intCards, $intColumnsPerCard){
            '   for ($intCard=1;$intCards;$intCard++){
            '       for($intColumn=1;$intColumnsPerCard;$intColumn++){
            '           tfCurrentTestFile->addSensor(Sensor->sensorFactory($intCard, $intColumn, txtCard{$intCard}Batch->Text, txtCard{$intCard}Col{$intColumn}->Text))
            '       }
            '   }
            ' }
            'Select Case cfgGlobal.CardConfig
            '    Case CardConfiguration.ONE_CARD_SIXTEEN_SENSORS
            '        ' Add all sensors
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
            '        'tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))
            '    Case CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))
            '    Case CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))
            '    Case CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 1, txtCard4Batch.Text, txtCard4Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 2, txtCard4Batch.Text, txtCard4Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 3, txtCard4Batch.Text, txtCard4Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 4, txtCard4Batch.Text, txtCard4Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 5, txtCard4Batch.Text, txtCard4Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 6, txtCard4Batch.Text, txtCard4Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 7, txtCard4Batch.Text, txtCard4Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 8, txtCard4Batch.Text, txtCard4Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 9, txtCard4Batch.Text, txtCard4Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 10, txtCard4Batch.Text, txtCard4Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 11, txtCard4Batch.Text, txtCard4Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 12, txtCard4Batch.Text, txtCard4Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 13, txtCard4Batch.Text, txtCard4Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 14, txtCard4Batch.Text, txtCard4Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 15, txtCard4Batch.Text, txtCard4Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 16, txtCard4Batch.Text, txtCard4Col16.Text))
            '    Case CardConfiguration.FIVE_CARD_EIGHTY_SENSORS
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 1, txtCard4Batch.Text, txtCard4Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 2, txtCard4Batch.Text, txtCard4Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 3, txtCard4Batch.Text, txtCard4Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 4, txtCard4Batch.Text, txtCard4Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 5, txtCard4Batch.Text, txtCard4Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 6, txtCard4Batch.Text, txtCard4Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 7, txtCard4Batch.Text, txtCard4Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 8, txtCard4Batch.Text, txtCard4Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 9, txtCard4Batch.Text, txtCard4Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 10, txtCard4Batch.Text, txtCard4Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 11, txtCard4Batch.Text, txtCard4Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 12, txtCard4Batch.Text, txtCard4Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 13, txtCard4Batch.Text, txtCard4Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 14, txtCard4Batch.Text, txtCard4Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 15, txtCard4Batch.Text, txtCard4Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 16, txtCard4Batch.Text, txtCard4Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 1, txtCard5Batch.Text, txtCard5Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 2, txtCard5Batch.Text, txtCard5Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 3, txtCard5Batch.Text, txtCard5Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 4, txtCard5Batch.Text, txtCard5Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 5, txtCard5Batch.Text, txtCard5Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 6, txtCard5Batch.Text, txtCard5Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 7, txtCard5Batch.Text, txtCard5Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 8, txtCard5Batch.Text, txtCard5Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 9, txtCard5Batch.Text, txtCard5Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 10, txtCard5Batch.Text, txtCard5Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 11, txtCard5Batch.Text, txtCard5Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 12, txtCard5Batch.Text, txtCard5Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 13, txtCard5Batch.Text, txtCard5Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 14, txtCard5Batch.Text, txtCard5Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 15, txtCard5Batch.Text, txtCard5Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 16, txtCard5Batch.Text, txtCard5Col16.Text))
            '    Case CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 1, txtCard1Batch.Text, txtCard1Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 2, txtCard1Batch.Text, txtCard1Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 3, txtCard1Batch.Text, txtCard1Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 4, txtCard1Batch.Text, txtCard1Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 5, txtCard1Batch.Text, txtCard1Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 6, txtCard1Batch.Text, txtCard1Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 7, txtCard1Batch.Text, txtCard1Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 8, txtCard1Batch.Text, txtCard1Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 9, txtCard1Batch.Text, txtCard1Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 10, txtCard1Batch.Text, txtCard1Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 11, txtCard1Batch.Text, txtCard1Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 12, txtCard1Batch.Text, txtCard1Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 13, txtCard1Batch.Text, txtCard1Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 14, txtCard1Batch.Text, txtCard1Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 15, txtCard1Batch.Text, txtCard1Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(1, 16, txtCard1Batch.Text, txtCard1Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 1, txtCard2Batch.Text, txtCard2Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 2, txtCard2Batch.Text, txtCard2Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 3, txtCard2Batch.Text, txtCard2Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 4, txtCard2Batch.Text, txtCard2Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 5, txtCard2Batch.Text, txtCard2Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 6, txtCard2Batch.Text, txtCard2Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 7, txtCard2Batch.Text, txtCard2Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 8, txtCard2Batch.Text, txtCard2Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 9, txtCard2Batch.Text, txtCard2Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 10, txtCard2Batch.Text, txtCard2Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 11, txtCard2Batch.Text, txtCard2Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 12, txtCard2Batch.Text, txtCard2Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 13, txtCard2Batch.Text, txtCard2Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 14, txtCard2Batch.Text, txtCard2Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 15, txtCard2Batch.Text, txtCard2Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(2, 16, txtCard2Batch.Text, txtCard2Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 1, txtCard3Batch.Text, txtCard3Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 2, txtCard3Batch.Text, txtCard3Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 3, txtCard3Batch.Text, txtCard3Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 4, txtCard3Batch.Text, txtCard3Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 5, txtCard3Batch.Text, txtCard3Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 6, txtCard3Batch.Text, txtCard3Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 7, txtCard3Batch.Text, txtCard3Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 8, txtCard3Batch.Text, txtCard3Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 9, txtCard3Batch.Text, txtCard3Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 10, txtCard3Batch.Text, txtCard3Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 11, txtCard3Batch.Text, txtCard3Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 12, txtCard3Batch.Text, txtCard3Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 13, txtCard3Batch.Text, txtCard3Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 14, txtCard3Batch.Text, txtCard3Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 15, txtCard3Batch.Text, txtCard3Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(3, 16, txtCard3Batch.Text, txtCard3Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 1, txtCard4Batch.Text, txtCard4Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 2, txtCard4Batch.Text, txtCard4Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 3, txtCard4Batch.Text, txtCard4Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 4, txtCard4Batch.Text, txtCard4Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 5, txtCard4Batch.Text, txtCard4Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 6, txtCard4Batch.Text, txtCard4Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 7, txtCard4Batch.Text, txtCard4Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 8, txtCard4Batch.Text, txtCard4Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 9, txtCard4Batch.Text, txtCard4Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 10, txtCard4Batch.Text, txtCard4Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 11, txtCard4Batch.Text, txtCard4Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 12, txtCard4Batch.Text, txtCard4Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 13, txtCard4Batch.Text, txtCard4Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 14, txtCard4Batch.Text, txtCard4Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 15, txtCard4Batch.Text, txtCard4Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(4, 16, txtCard4Batch.Text, txtCard4Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 1, txtCard5Batch.Text, txtCard5Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 2, txtCard5Batch.Text, txtCard5Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 3, txtCard5Batch.Text, txtCard5Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 4, txtCard5Batch.Text, txtCard5Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 5, txtCard5Batch.Text, txtCard5Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 6, txtCard5Batch.Text, txtCard5Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 7, txtCard5Batch.Text, txtCard5Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 8, txtCard5Batch.Text, txtCard5Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 9, txtCard5Batch.Text, txtCard5Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 10, txtCard5Batch.Text, txtCard5Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 11, txtCard5Batch.Text, txtCard5Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 12, txtCard5Batch.Text, txtCard5Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 13, txtCard5Batch.Text, txtCard5Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 14, txtCard5Batch.Text, txtCard5Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 15, txtCard5Batch.Text, txtCard5Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(5, 16, txtCard5Batch.Text, txtCard5Col16.Text))

            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 1, txtCard6Batch.Text, txtCard6Col1.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 2, txtCard6Batch.Text, txtCard6Col2.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 3, txtCard6Batch.Text, txtCard6Col3.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 4, txtCard6Batch.Text, txtCard6Col4.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 5, txtCard6Batch.Text, txtCard6Col5.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 6, txtCard6Batch.Text, txtCard6Col6.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 7, txtCard6Batch.Text, txtCard6Col7.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 8, txtCard6Batch.Text, txtCard6Col8.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 9, txtCard6Batch.Text, txtCard6Col9.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 10, txtCard6Batch.Text, txtCard6Col10.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 11, txtCard6Batch.Text, txtCard6Col11.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 12, txtCard6Batch.Text, txtCard6Col12.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 13, txtCard6Batch.Text, txtCard6Col13.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 14, txtCard6Batch.Text, txtCard6Col14.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 15, txtCard6Batch.Text, txtCard6Col15.Text))
            '        tfCurrentTestFile.AddSensor(Sensor.SensorFactory(6, 16, txtCard6Batch.Text, txtCard6Col16.Text))
            'End Select
        Catch ex As Exception
            Throw
        End Try
    End Sub
    ' Name: ValidateForm()
    ' Returns: Boolean: Indicates success / failure
    ' Description: Checks user inputs against a set of formatting / data type rules
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
    ' Name: btnCancel_Click()
    ' Handles: User clicks 'Cancel'
    ' Description: Exactly what you think
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: SetTextForAll()
    ' Parameters: 
    '       strVal: The string value to be mirrored 
    ' Description: Mirrors the input string across all batch input boxes in the form
    Private Sub SetTextForAll(ByVal strVal As String)
        Try
            txtCard1Batch.Text = strVal
            txtCard2Batch.Text = strVal
            txtCard3Batch.Text = strVal
            txtCard4Batch.Text = strVal
            txtCard5Batch.Text = strVal
            txtCard6Batch.Text = strVal
            txtOriginator = Nothing
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: chkSameCard1_CheckedChanged()
    ' Handles: User clicks the 'All same batch' checkbox in the first card tab
    ' Description: Toggles the batch mirroring flag and mirrors the contents of the Card 1 batch field to all batch text boxes
    Private Sub chkSameCard1_CheckedChanged(sender As Object, e As EventArgs) Handles chkSameCard1.CheckedChanged
        Try
            If (chkSameCard1.Checked) Then
                SetTextForAll(txtCard1Batch.Text)
                boolSameBatch = True
            Else
                boolSameBatch = False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: txtCard[i]Batch_TextChanged()
    ' Handles: The user updates the text in one of the batch fields and, if the same batch flag is true, mirrors the value to all other batch text boxes
    Private Sub txtCard1Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard1Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        Try
            If (boolSameBatch And txtOriginator Is Nothing) Then
                txtOriginator = sender
                SetTextForAll(txtCard1Batch.Text)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub txtCard2Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard2Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        Try
            If (boolSameBatch And txtOriginator Is Nothing) Then
                txtOriginator = sender
                SetTextForAll(txtCard2Batch.Text)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub txtCard3Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard3Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        Try
            If (boolSameBatch And txtOriginator Is Nothing) Then
                txtOriginator = sender
                SetTextForAll(txtCard3Batch.Text)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub txtCard4Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard4Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        Try
            If (boolSameBatch And txtOriginator Is Nothing) Then
                txtOriginator = sender
                SetTextForAll(txtCard4Batch.Text)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub txtCard5Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard5Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        Try
            If (boolSameBatch And txtOriginator Is Nothing) Then
                txtOriginator = sender
                SetTextForAll(txtCard5Batch.Text)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Private Sub txtCard6Batch_TextChanged(sender As Object, e As EventArgs) Handles txtCard6Batch.TextChanged
        '' Check if boolSameBatch and if so copy the text to all other text boxes for batch
        Try
            If (boolSameBatch And txtOriginator Is Nothing) Then
                txtOriginator = sender
                SetTextForAll(txtCard6Batch.Text)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

End Class