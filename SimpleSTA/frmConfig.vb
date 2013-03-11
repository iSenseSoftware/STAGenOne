Imports System.IO
Imports System
Imports System.Xml.Serialization
Imports SimpleSTA.SharedModule
Public Class frmConfig
    Dim errors As String()
    Private boolLocked As Boolean = True
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If (boolLocked) Then
                frmPassword.Show()
            Else
                saveConfiguration()
                Me.Close()
                If (verifyConfiguration()) Then
                    frmMain.SystemStatusLabel.Text = "System Status: Configuration Loaded"
                Else
                    frmMain.SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                End If
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Function validatePassword(password As String) As Boolean
        If (password = strAdminPassword) Then
            boolLocked = False
            EnableControls()
            btnSave.Text = "Save"
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frmConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'populate comboboxes
            Dim rangeList As List(Of KeyValuePair(Of CurrentRange, String)) = New List(Of KeyValuePair(Of CurrentRange, String))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.one_uA, "0 - 1 uA"))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.ten_uA, "1 - 10 uA"))
            rangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.hundred_uA, "1 - 100 uA"))
            cmbRange.DataSource = rangeList
            cmbRange.ValueMember = "Key"
            cmbRange.DisplayMember = "Value"

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

            Dim filterTypeList As List(Of KeyValuePair(Of FilterType, String)) = New List(Of KeyValuePair(Of FilterType, String))
            filterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MEDIAN, "Median"))
            filterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MOVING_AVG, "Moving Average"))
            filterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_REPEAT_AVG, "Repeat Average"))
            cmbFilterType.DataSource = filterTypeList
            cmbFilterType.ValueMember = "Key"
            cmbFilterType.DisplayMember = "Value"

            populateConfigurationForm()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub populateConfigurationForm()
        Try
            loadConfiguration()
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
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub saveConfiguration()
        Try
            If (ValidateForm()) Then
                Dim serializer As New XmlSerializer(config.GetType)
                Dim writer As New StreamWriter(appDir & configFileName)
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
                serializer.Serialize(writer, config)
                writer.Close()
            Else
                MsgBox(errors)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        If (verifyConfiguration()) Then
            frmMain.SystemStatusLabel.Text = "System Status: Configuration Loaded"
        Else
            frmMain.SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
        End If
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
    Private Sub EnableControls()
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
        txtRow3Resistor.Enabled = True
        txtRow4Resistor.Enabled = True
        txtRow5Resistor.Enabled = True
        txtTolerance.Enabled = True
    End Sub
End Class
