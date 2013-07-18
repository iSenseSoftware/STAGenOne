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