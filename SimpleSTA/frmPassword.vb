Public Class frmPassword
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Try
            If (frmConfig.validatePassword(txtPassword.Text)) Then
                Me.Close()
            Else
                MsgBox("Incorrect password")
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

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