Public Class frmAbout

    Private Sub frmAbout_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtTitle.Text = strApplicationName
        txtVersion.Text = "Version: " & strApplicationVersion
        txtDescription.Text = strApplicationDescription
    End Sub
End Class