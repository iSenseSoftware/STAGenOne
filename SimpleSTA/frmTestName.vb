Public Class frmTestName

    
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        NewTest(txtTestID.Text, txtOperatorIntitials.Text)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub
End Class