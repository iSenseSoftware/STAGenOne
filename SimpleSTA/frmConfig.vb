Imports System.IO
Imports System
Imports System.Xml.Serialization
Imports SimpleSTA.SharedModule
Public Class frmConfig
    Dim errors As String()
    Private boolLocked As Boolean = True
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' If the form is currently locked, display the password form
            If (boolLocked) Then
                frmPassword.Show()
            Else
                ' If the form is unlocked, attempt to validate the inputs and save new configuration
                If (ValidateForm()) Then
                    ' Set property values for config object
                    config.Range = cmbRange.SelectedValue
                    config.Address = txtAddress.Text
                    config.STAID = txtSTAID.Text
                    config.CardConfig = cmbCardConfig.SelectedValue
                    config.Bias = CDbl(txtBias.Text)
                    config.RecordInterval = CDbl(txtInterval.Text)
                    config.Samples = txtSamples.Text
                    config.DumpDirectory = txtDumpDir.Text
                    config.NPLC = txtNPLC.Text
                    config.Filter = cmbFilterType.SelectedValue
                    config.SettlingTime = txtSettlingTime.Text
                    config.Resistor1Resistance = CDbl(txtRow3Resistor.Text) * 10 ^ 6
                    config.Resistor2Resistance = CDbl(txtRow4Resistor.Text) * 10 ^ 6
                    config.Resistor3Resistance = CDbl(txtRow5Resistor.Text) * 10 ^ 6
                    config.AuditTolerance = CDbl(txtTolerance.Text) / 100
                    config.SystemFileDirectory = txtSystemInfoFile.Text
                    ' Perform secondary validation on the config object
                    If verifyConfiguration(config) Then
                        ' Attempt to write the configuration to file
                        If (config.WriteToFile(appDir & "\" & configFileName)) Then
                            boolConfigLoaded = True
                            frmMain.chkConfigStatus.Checked = True
                            Me.Close()
                        Else
                            MsgBox("Could not write configuration to file.")
                        End If
                    Else
                        MsgBox("Could not validate configuration.  Make sure all settings entered are valid.")
                        config = Nothing
                        boolConfigLoaded = False
                        frmMain.chkConfigStatus.Checked = False
                    End If
                Else
                    ' Do nothing.  The validateForm method will generate value-specific error messages if there is a failure
                End If
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            config = Nothing
            boolConfigLoaded = False
            frmMain.chkConfigStatus.Checked = False
        End Try
    End Sub
    Public Function validatePassword(password As String) As Boolean
        If (password = strAdminPassword) Then
            boolLocked = False
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub frmConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Populate CurrentRange combo box
            Dim rangeList As List(Of KeyValuePair(Of CurrentRange, String)) = New List(Of KeyValuePair(Of CurrentRange, String))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.one_uA, "0 - 1 uA"))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.ten_uA, "1 - 10 uA"))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.hundred_uA, "1 - 100 uA"))
            cmbRange.DataSource = rangeList
            cmbRange.ValueMember = "Key"
            cmbRange.DisplayMember = "Value"
            ' Populate CardConfiguration combo box
            Dim cardConfigList As List(Of KeyValuePair(Of CardConfiguration, String)) = New List(Of KeyValuePair(Of CardConfiguration, String))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.ONE_CARD_SIXTEEN_SENSORS, "1 Card, 16 Sensors"))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS, "2 Card, 32 Sensors"))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS, "3 Card, 48 Sensors"))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS, "4 Card, 64 Sensors"))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FIVE_CARD_EIGHTY_SENSORS, "5 Card, 80 Sensors"))
            cardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS, "6 Card, 96 Sensors"))
            cmbCardConfig.DataSource = cardConfigList
            cmbCardConfig.ValueMember = "Key"
            cmbCardConfig.DisplayMember = "Value"
            ' Populate FilterType combo box
            Dim filterTypeList As List(Of KeyValuePair(Of FilterType, String)) = New List(Of KeyValuePair(Of FilterType, String))
            filterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MEDIAN, "Median"))
            filterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MOVING_AVG, "Moving Average"))
            filterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_REPEAT_AVG, "Repeat Average"))
            cmbFilterType.DataSource = filterTypeList
            cmbFilterType.ValueMember = "Key"
            cmbFilterType.DisplayMember = "Value"

            '-----------------
            ' Attempt to load the configuration from file and set form values
            '-----------------
            ' Check to see if the configuration file is present at the expected location
            If (System.IO.File.Exists(appDir & "\" & configFileName)) Then
                config = loadConfiguration(appDir & "\" & configFileName)

                If Not config Is Nothing Then
                    ' If the config was successfully loaded, set the status indicators
                    If verifyConfiguration(config) Then
                        boolConfigLoaded = True
                        frmMain.chkConfigStatus.Checked = True
                    Else
                        MsgBox("Configuration file is invalid.  Resetting to defaults...")
                        config = Nothing
                        boolConfigLoaded = False
                        frmMain.chkConfigStatus.Checked = False
                        config = New Configuration
                        If (verifyConfiguration(config)) Then
                            If config.WriteToFile(appDir & "\" & configFileName) Then
                                ' Do nothing
                            Else
                                ' Warn the user that the configuration could not be written to file
                                MsgBox("Could not write configuration to file.  Verify your user account has read/write access to " & appDir & "\" & configFileName)
                            End If
                        Else
                            ' Do nothing.  The user will have to create a valid config to save.
                        End If
                    End If
                Else
                    ' What do we do when the config cannot be loaded from file?
                End If
            Else
                ' Alert user and attempt to load configuration from default values
                MsgBox(appDir & "\" & configFileName & " could not be found.  Attempting to load configuration from defaults...")
                ' Update program state:
                ' Clear existing config data and update status flag and icon
                config = Nothing
                boolConfigLoaded = False
                frmMain.chkConfigStatus.Checked = False
                config = New Configuration
                ' If defaults are set for all values, write the config to file
                ' and populate the config form with its values
                If (verifyConfiguration(config)) Then
                    If config.WriteToFile(appDir & "\" & configFileName) Then
                        ' Do nothing
                    Else
                        ' Warn the user that the configuration could not be written to file
                        MsgBox("Could not write configuration to file.  Verify your user account has read/write access to " & appDir & "\" & configFileName)
                    End If
                Else
                    ' Do nothing.  The user will have to create a valid config to save.
                End If
            End If
            populateConfigurationForm()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub populateConfigurationForm()
        Try
            cmbRange.SelectedValue = config.Range
            cmbCardConfig.SelectedValue = config.CardConfig
            cmbFilterType.SelectedValue = config.Filter
            txtAddress.Text = config.Address
            txtSTAID.Text = config.STAID
            txtBias.Text = config.Bias
            txtInterval.Text = config.RecordInterval
            txtNPLC.Text = config.NPLC
            txtSamples.Text = config.Samples
            txtDumpDir.Text = config.DumpDirectory
            txtSettlingTime.Text = config.SettlingTime
            txtRow3Resistor.Text = config.Resistor1Resistance / 10 ^ 6
            txtRow4Resistor.Text = config.Resistor2Resistance / 10 ^ 6
            txtRow5Resistor.Text = config.Resistor3Resistance / 10 ^ 6
            txtTolerance.Text = config.AuditTolerance * 100
            txtSystemInfoFile.Text = config.SystemFileDirectory
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub saveConfiguration()
        Try
            If (ValidateForm()) Then
                Dim serializer As New XmlSerializer(config.GetType)
                Dim writer As New StreamWriter(appDir & "\" & configFileName)
                config.Range = cmbRange.SelectedValue
                config.Address = txtAddress.Text
                config.STAID = txtSTAID.Text
                config.CardConfig = cmbCardConfig.SelectedValue
                config.Bias = CDbl(txtBias.Text)
                config.RecordInterval = CDbl(txtInterval.Text)
                config.Samples = txtSamples.Text
                config.DumpDirectory = txtDumpDir.Text
                config.NPLC = txtNPLC.Text
                config.Filter = cmbFilterType.SelectedValue
                config.SettlingTime = txtSettlingTime.Text
                config.Resistor1Resistance = CDbl(txtRow3Resistor.Text) * 10 ^ 6
                config.Resistor2Resistance = CDbl(txtRow4Resistor.Text) * 10 ^ 6
                config.Resistor3Resistance = CDbl(txtRow5Resistor.Text) * 10 ^ 6
                config.AuditTolerance = CDbl(txtTolerance.Text) / 100
                config.SystemFileDirectory = txtSystemInfoFile.Text
                serializer.Serialize(writer, config)
                writer.Close()
                boolConfigLoaded = True
                frmMain.chkConfigStatus.Checked = True
            Else
                MsgBox("Configuration data entered could not be validated.  Correct and try again")
                boolConfigLoaded = False
                frmMain.chkConfigStatus.Checked = False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If (verifyConfiguration(config)) Then
            frmMain.chkConfigStatus.Checked = True
        Else
            frmMain.chkConfigStatus.Checked = False
        End If
        Me.Close()
    End Sub
    Private Function ValidateForm() As Boolean
        Try
            ' Check each field against it's requirements and return true/false
            Dim boolValidates As Boolean = True
            ' Check record interval: integer non-null

            ' Check Bias: double, non-null

            ' Check Current Range: CURRENT_RANGE, non-null

            ' Check filter type: FILTER_TYPE, non-null

            ' Check samples: integer, non-null

            ' Check NPLC: integer, non-null

            ' Check address: non-null

            ' STA ID: non-null

            ' Card Configuration: CARD_CONFIGURATION, non-null
            Return boolValidates
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    Private Sub btnSelectFile_Click(sender As Object, e As EventArgs) Handles btnSelectFile.Click
        Try
            FolderBrowserDialog1.Description = "Select test data default directory"
            FolderBrowserDialog1.ShowNewFolderButton = True
            Dim dlgResult As DialogResult = FolderBrowserDialog1.ShowDialog()

            If dlgResult = Windows.Forms.DialogResult.OK Then
                txtDumpDir.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub btnSelectInfoFile_Click(sender As Object, e As EventArgs) Handles btnSelectInfoFile.Click
        Try
            FolderBrowserDialog1.Description = "Select System Info data default directory"
            FolderBrowserDialog1.ShowNewFolderButton = True
            Dim dlgResult As DialogResult = FolderBrowserDialog1.ShowDialog()

            If dlgResult = Windows.Forms.DialogResult.OK Then
                txtSystemInfoFile.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub EnableControls()
        ' enable all controls after password check has been passed
        txtAddress.Enabled = True
        txtBias.Enabled = True
        txtDumpDir.Enabled = True
        txtInterval.Enabled = True
        txtNPLC.Enabled = True
        txtSamples.Enabled = True
        txtSTAID.Enabled = True
        txtSettlingTime.Enabled = True
        cmbCardConfig.Enabled = True
        cmbFilterType.Enabled = True
        cmbRange.Enabled = True
        btnSelectFile.Enabled = True
        btnSelectInfoFile.Enabled = True
        txtRow3Resistor.Enabled = True
        txtRow4Resistor.Enabled = True
        txtRow5Resistor.Enabled = True
        txtTolerance.Enabled = True
        txtSystemInfoFile.Enabled = True
    End Sub
    
End Class
