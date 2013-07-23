Public Class frmSensorID

    
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        'Verify boxes all filled
        MsgBox("Add code to verify that all boxes are filled")
        'Show testform
        frmTestForm.Show()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub

    Private Sub frmSensorID_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'hide form
        Me.Hide()
        'run end test routine
        EndTest()
    End Sub

    Private Sub frmSensorID_Load(sender As Object, e As EventArgs) Handles Me.Load
        PopulateSensorID()
    End Sub
End Class