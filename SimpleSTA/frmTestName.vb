Public Class frmTestName

    
    Private Sub btnOk_Click() Handles btnOk.Click
        NewTest(txtTestID.Text, txtOperatorIntitials.Text)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub

    Private Sub txtOperatorIntitials_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOperatorIntitials.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If txtOperatorIntitials.Text = "" Then
                'do nothing
            ElseIf txtTestID.Text = "" Then
                txtTestID.Select()
            Else
                btnOk_Click()
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub txtTestID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTestID.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If txtTestID.Text = "" Then
                '
            ElseIf txtOperatorIntitials.Text = "" Then
                txtOperatorIntitials.Select()
            Else
                btnOk_Click()
            End If
            e.Handled = True
        End If
    End Sub
End Class