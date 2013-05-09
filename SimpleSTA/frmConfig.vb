Option Explicit On
Imports System.IO
Imports System
Imports System.Xml.Serialization
Imports SimpleSTA.SharedModule
' ------------------------------------------------------------------------------------------------
' frmConfig is the form used to provide read-only access to configuration settings for test users
' and read/write access to administative users possessing a hard-coded password.
' ------------------------------------------------------------------------------------------------
Public Class frmConfig
    Private boolLocked As Boolean = True ' Indicates whether form has been unlocked by an administrator.  Used for conditional formatting.
    ' -----------------------------------------------
    ' Utility Functions
    ' ------------------------------------------------
    ' Name: populateConfigurationForm()
    ' Description: Populates the configuration form fields based on values set in the config object
    Private Sub PopulateConfigurationForm()
        Try
            cmbRange.SelectedValue = cfgGlobal.Range
            cmbCardConfig.SelectedValue = cfgGlobal.CardConfig
            cmbFilterType.SelectedValue = cfgGlobal.Filter
            txtAddress.Text = cfgGlobal.Address
            txtSTAID.Text = cfgGlobal.STAID
            txtBias.Text = cfgGlobal.Bias
            txtInterval.Text = cfgGlobal.RecordInterval
            txtNPLC.Text = cfgGlobal.NPLC
            txtSamples.Text = cfgGlobal.Samples
            txtDumpDir.Text = cfgGlobal.DumpDirectory
            txtSettlingTime.Text = cfgGlobal.SettlingTime
            txtRow3Resistor.Text = cfgGlobal.ResistorNominalValues(0) \ 10 ^ 6
            txtRow4Resistor.Text = cfgGlobal.ResistorNominalValues(1) \ 10 ^ 6
            txtRow5Resistor.Text = cfgGlobal.ResistorNominalValues(2) \ 10 ^ 6
            txtRow6Resistor.Text = cfgGlobal.ResistorNominalValues(3) \ 10 ^ 6
            txtTolerance.Text = cfgGlobal.AuditTolerance * 100
            txtAuditZero.Text = cfgGlobal.AuditZero * 10 ^ 9
            txtSystemInfoFile.Text = cfgGlobal.SystemFileDirectory
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    ' Name: ValidateForm()
    ' Returns: Boolean: Indicates success of failure
    ' Description:
    ' Checks each field in the config form to confirm the input is:
    ' 1. Present
    ' 2. The correct data type
    ' 3. Within the acceptable range of values and contains only acceptable characters
    Private Function ValidateForm() As Boolean
        Try
            ' Check each field against it's requirements and return true/false
            Dim boolValidates As Boolean = True
            If (txtAddress.Text = "") Then
                MsgBox("STA Address field cannot be blank")
                boolValidates = False
            End If
            If (txtSTAID.Text = "") Then
                MsgBox("STA ID field cannot be blank")
                boolValidates = False
            End If
            If (cmbCardConfig.Text = "") Then
                MsgBox("You must select a valid card configuration")
                boolValidates = False
            End If
            If (cmbRange.Text = "") Then
                MsgBox("You must select a valid current range")
                boolValidates = False
            End If
            If (txtNPLC.Text = "") Then
                MsgBox("The NPLC field cannot be blank")
                boolValidates = False
            End If
            If Not (isDouble(txtNPLC.Text)) Then
                MsgBox("NPLC must be a positive number")
                boolValidates = False
            Else
                If (CDbl(txtNPLC.Text) <= 0) Then
                    MsgBox("NPLC must be a positive number")
                    boolValidates = False
                End If
            End If
            If txtSamples.Text = "" Then
                MsgBox("Sample Count field cannot be left blank")
                boolValidates = False
            End If
            If Not (isInteger(txtSamples.Text)) Then
                MsgBox("Sample Count must be a positive integer")
                boolValidates = False
            Else
                If (CInt(txtSamples.Text) <= 0) Then
                    MsgBox("Sample Count must be a positive integer")
                    boolValidates = False
                End If
            End If
            If (cmbFilterType.Text = "") Then
                MsgBox("You must select a Filter Type")
                boolValidates = False
            End If
            If (txtInterval.Text = "") Then
                MsgBox("The Sample Interval field cannot be left blank")
                boolValidates = False
            End If
            If Not (isInteger(txtInterval.Text)) Then
                MsgBox("The Sample Interval must be a positive integer")
                boolValidates = False
            Else
                If (CInt(txtInterval.Text) <= 0) Then
                    MsgBox("The Sample Interval must be a positive integer")
                    boolValidates = False
                End If
            End If
            If (txtDumpDir.Text = "") Then
                MsgBox("Data file directory field cannot be blank")
                boolValidates = False
            End If
            If Not (System.IO.Directory.Exists(txtDumpDir.Text)) Then
                MsgBox("Directory selected for data file does not exist")
                boolValidates = False
            End If
            If (txtSystemInfoFile.Text = "") Then
                MsgBox("System Info file directory field cannot be left blank")
                boolValidates = False
            End If
            If Not System.IO.Directory.Exists(txtSystemInfoFile.Text) Then
                MsgBox("System info file directory  selected could not be found")
                boolValidates = False
            End If
            If (txtSettlingTime.Text = "") Then
                MsgBox("Settling Time field cannot be blank")
                boolValidates = False
            End If
            If Not (isInteger(txtSettlingTime.Text)) Then
                MsgBox("Settling Time must be a positive integer")
                boolValidates = False
            Else
                If (CInt(txtSettlingTime.Text) <= 0) Then
                    MsgBox("Settling Time must be a positive integer")
                    boolValidates = False
                End If
            End If
            If (txtRow3Resistor.Text = "") Then
                MsgBox("Row 3 Resistor Nominal Value field cannot be left blank")
                boolValidates = False
            End If
            If (txtRow4Resistor.Text = "") Then
                MsgBox("Row 4 Resistor Nominal Value field cannot be left blank")
                boolValidates = False
            End If
            If (txtRow5Resistor.Text = "") Then
                MsgBox("Row 5 Resistor Nominal Value field cannot be left blank")
                boolValidates = False
            End If
            If Not (isDouble(txtRow3Resistor.Text)) Then
                MsgBox("Row 3 Resistor Nominal Value must be a positive number")
                boolValidates = False
            Else
                If (CDbl(txtRow3Resistor.Text) <= 0) Then
                    MsgBox("Row 3 Resistor Nominal Value must be a positive number")
                    boolValidates = False
                End If
            End If
            If Not (isDouble(txtRow4Resistor.Text)) Then
                MsgBox("Row 4 Resistor Nominal Value must be a positive number")
                boolValidates = False
            Else
                If (CDbl(txtRow4Resistor.Text) <= 0) Then
                    MsgBox("Row 4 Resistor Nominal Value must be a positive number")
                    boolValidates = False
                End If
            End If
            If Not (isDouble(txtRow5Resistor.Text)) Then
                MsgBox("Row 5 Resistor Nominal Value must be a positive number")
                boolValidates = False
            Else
                If (CDbl(txtRow5Resistor.Text) <= 0) Then
                    MsgBox("Row 5 Resistor Nominal Value must be a positive number")
                    boolValidates = False
                End If
            End If
            If (txtTolerance.Text = "") Then
                MsgBox("Self Test Tolerance field cannot be left blank")
                boolValidates = False
            End If
            If Not (isDouble(txtTolerance.Text)) Then
                MsgBox("Self Test Tolerance must be a positive number less than 100")
                boolValidates = False
            Else
                If (CDbl(txtTolerance.Text) <= 0 Or CDbl(txtTolerance.Text) >= 100) Then
                    MsgBox("Self Test Tolerance must be a positive number less than 100")
                    boolValidates = False
                End If
            End If
            If (txtAuditZero.Text = "") Then
                MsgBox("Self Test Zero field cannot be left blank")
                boolValidates = False
            End If
            If Not (IsDouble(txtAuditZero.Text)) Then
                MsgBox("Self Test Zero must be a positive number")
                boolValidates = False
            Else
                ' nothing
            End If
            Return boolValidates
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Return False
        End Try
    End Function
    ' Name: EnableControls()
    ' Description: Upon validating an admin password, this sub is called to enable all form controls
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
        txtRow6Resistor.Enabled = True
        txtTolerance.Enabled = True
        txtSystemInfoFile.Enabled = True
        txtAuditZero.Enabled = True
    End Sub
    ' Name: ValidatePassword
    ' Returns: True/False
    ' Checks the input password against strAdminPassword
    Public Function ValidatePassword(strPassword As String) As Boolean
        If (strPassword = strAdminPassword) Then
            boolLocked = False
            Return True
        Else
            Return False
        End If
    End Function
    ' -----------------------------------------------
    ' Event Handlers
    ' -----------------------------------------------
    ' Name: frmConfig_Load()
    ' Handles: Form load event.  Fires first when the form is shown.
    ' Description:
    ' 1. Populates combo boxes with appropriate enumerated values
    ' 2. Attempt to load configuration from file or create from defaults
    ' 3. Set display values in config form
    Private Sub frmConfig_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Populate CurrentRange combo box
            Dim lstRangeList As List(Of KeyValuePair(Of CurrentRange, String)) = New List(Of KeyValuePair(Of CurrentRange, String))
            lstRangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.one_uA, "0 - 1 uA"))
            lstRangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.ten_uA, "1 - 10 uA"))
            lstRangeList.Add(New KeyValuePair(Of CurrentRange, String)(CurrentRange.hundred_uA, "1 - 100 uA"))
            cmbRange.DataSource = lstRangeList
            cmbRange.ValueMember = "Key"
            cmbRange.DisplayMember = "Value"
            ' Populate CardConfiguration combo box
            Dim lstCardConfigList As List(Of KeyValuePair(Of CardConfiguration, String)) = New List(Of KeyValuePair(Of CardConfiguration, String))
            lstCardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.ONE_CARD_SIXTEEN_SENSORS, "1 Card, 16 Sensors"))
            lstCardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.TWO_CARD_THIRTY_TWO_SENSORS, "2 Card, 32 Sensors"))
            lstCardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.THREE_CARD_FOURTY_EIGHT_SENSORS, "3 Card, 48 Sensors"))
            lstCardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FOUR_CARD_SIXTY_FOUR_SENSORS, "4 Card, 64 Sensors"))
            lstCardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.FIVE_CARD_EIGHTY_SENSORS, "5 Card, 80 Sensors"))
            lstCardConfigList.Add(New KeyValuePair(Of CardConfiguration, String)(CardConfiguration.SIX_CARD_NINETY_SIX_SENSORS, "6 Card, 96 Sensors"))
            cmbCardConfig.DataSource = lstCardConfigList
            cmbCardConfig.ValueMember = "Key"
            cmbCardConfig.DisplayMember = "Value"
            ' Populate FilterType combo box
            Dim ftFilterTypeList As List(Of KeyValuePair(Of FilterType, String)) = New List(Of KeyValuePair(Of FilterType, String))
            ftFilterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MEDIAN, "Median"))
            ftFilterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_MOVING_AVG, "Moving Average"))
            ftFilterTypeList.Add(New KeyValuePair(Of FilterType, String)(FilterType.FILTER_REPEAT_AVG, "Repeat Average"))
            cmbFilterType.DataSource = ftFilterTypeList
            cmbFilterType.ValueMember = "Key"
            cmbFilterType.DisplayMember = "Value"

            '-----------------
            ' Attempt to load the configuration from file and set form values
            '-----------------
            ' Check to see if the configuration file is present at the expected location and valid
            If (loadOrRefreshConfiguration()) Then
                populateConfigurationForm()
            Else
                ' Alert the user that the configuration could not be loaded
                ' And populate the form with whatever values are available
                MsgBox("Unable to load configuration.")
                populateConfigurationForm()
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    ' Name: btnSave_Click()
    ' Handles: User clicks "Save"/"Edit" button
    ' Description:
    ' State-dependent handler shows the password dialog if the form is locked, or attempts to save
    ' or update the config object with the user-inputted values if it is unlocked.
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ' If the form is currently locked, display the password form
            If (boolLocked) Then
                frmPassword.Show()
            Else
                ' If the form is unlocked, attempt to validate the inputs and save new configuration
                If (ValidateForm()) Then
                    ' Set property values for config object
                    cfgGlobal.Range = cmbRange.SelectedValue
                    cfgGlobal.Address = txtAddress.Text
                    cfgGlobal.STAID = txtSTAID.Text
                    cfgGlobal.CardConfig = cmbCardConfig.SelectedValue
                    cfgGlobal.Bias = CDbl(txtBias.Text)
                    cfgGlobal.RecordInterval = CDbl(txtInterval.Text)
                    cfgGlobal.Samples = txtSamples.Text
                    cfgGlobal.DumpDirectory = txtDumpDir.Text
                    cfgGlobal.NPLC = txtNPLC.Text
                    cfgGlobal.Filter = cmbFilterType.SelectedValue
                    cfgGlobal.SettlingTime = txtSettlingTime.Text
                    cfgGlobal.ResistorNominalValues(0) = CDbl(txtRow3Resistor.Text) * 10 ^ 6
                    cfgGlobal.ResistorNominalValues(1) = CDbl(txtRow4Resistor.Text) * 10 ^ 6
                    cfgGlobal.ResistorNominalValues(2) = CDbl(txtRow5Resistor.Text) * 10 ^ 6
                    cfgGlobal.ResistorNominalValues(3) = CDbl(txtRow6Resistor.Text) * 10 ^ 6
                    cfgGlobal.AuditTolerance = CDbl(txtTolerance.Text) / 100
                    cfgGlobal.AuditZero = CDbl(txtAuditZero.Text) * 10 ^ -9
                    cfgGlobal.SystemFileDirectory = txtSystemInfoFile.Text
                    ' Perform secondary validation on the cfgGlobal object
                    If cfgGlobal.Validate() Then
                        ' Attempt to write the configuration to file
                        If (cfgGlobal.WriteToFile(strAppDir & Path.DirectorySeparatorChar & strConfigFileName)) Then
                            frmMain.chkConfigStatus.Checked = True
                            Me.Close()
                        Else
                            MsgBox("Could not write configuration to file.")
                        End If
                    Else
                        MsgBox("Could not validate configuration.  Make sure all settings entered are valid.")
                        cfgGlobal = Nothing
                        frmMain.chkConfigStatus.Checked = False
                    End If
                Else
                    ' Do nothing.  The validateForm method will generate value-specific error messages if there is a failure
                End If
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
            cfgGlobal = Nothing
            frmMain.chkConfigStatus.Checked = False
        End Try
    End Sub
    ' Name: btnCancel_Click
    ' Handles: User clicks "Cancel"
    ' Description: Closes the form and update the status indicator based on whether the current config object can be validated
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If cfgGlobal Is Nothing Then
            frmMain.chkConfigStatus.Checked = False
            Me.Close()
            Exit Sub
        End If
        If cfgGlobal.Validate() Then
            frmMain.chkConfigStatus.Checked = True
        Else
            frmMain.chkConfigStatus.Checked = False
        End If
        Me.Close()
    End Sub
    ' Name: btnSelectFile_Click()
    ' Handles: User clicks select file button "..." for the save directory field
    ' Description: Opens the dialog allowing the user to navigate to the directory to which test data will be saved
    Private Sub btnSelectFile_Click(sender As Object, e As EventArgs) Handles btnSelectFile.Click
        Try
            FolderBrowserDialog1.Description = "Select test data default directory"
            FolderBrowserDialog1.ShowNewFolderButton = True
            Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()

            If result = Windows.Forms.DialogResult.OK Then
                txtDumpDir.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: btnSelectInfoFile_Click()
    ' Handles: User clicks select file button "..." for the system info file directory field
    ' Description:
    ' Opens the dialog allowing the user to navigate to the directory to which 
    ' TestSystemInfo file is saved
    Private Sub btnSelectInfoFile_Click(sender As Object, e As EventArgs) Handles btnSelectInfoFile.Click
        Try
            FolderBrowserDialog1.Description = "Select System Info data default directory"
            FolderBrowserDialog1.ShowNewFolderButton = True
            Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()

            If result = Windows.Forms.DialogResult.OK Then
                txtSystemInfoFile.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

End Class
