Public Class frmPassword
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If (frmConfig.validatePassword(txtPassword.Text)) Then
            Me.Close()
        Else
            MsgBox("Incorrect password")
        End If
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If (e.KeyChar = ChrW(Keys.Return)) Then
            If (frmConfig.validatePassword(txtPassword.Text)) Then
                Me.Close()
            Else
                MsgBox("Incorrect password")
            End If
        End If
    End Sub
End Class