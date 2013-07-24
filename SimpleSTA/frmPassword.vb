' ------------------------------------------------------------------------------
' The frmPassword form is used only for verifying the administrator password required
' to unlock the frmConfig form.
' --------------------------------------------------------------------------------
Option Explicit On
Public Class frmPassword
    ' --------------------------------------------------------
    ' Event Handlers
    ' --------------------------------------------------------
    ' Name: btnOK_Click()
    ' Handles: User clicks 'OK' button
    ' Description: Validates the password entered.  Alerts the user if invalid and closes itself if valid
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Try
            If (frmConfig.validatePassword(txtPassword.Text)) Then
                frmConfig.EnableControls()
                frmConfig.btnSave.Text = "Save"
                Me.Close()
            Else
                MsgBox("Incorrect password")
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: btnCancel_Click()
    ' Handles: User clicks 'Cancel' button
    ' Description: Closes the form
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: txtPassword_KeyPress()
    ' Handles: User presses any key while targetting the password text field
    ' Description: Checks the key pressed by the user and if it is the 'Enter' key attempts to validate the password.
    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        Try
            If (e.KeyChar = ChrW(Keys.Return)) Then
                If (frmConfig.validatePassword(txtPassword.Text)) Then
                    frmConfig.EnableControls()
                    frmConfig.btnSave.Text = "Save"
                    Me.Close()
                Else
                    MsgBox("Incorrect password")
                End If
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
End Class