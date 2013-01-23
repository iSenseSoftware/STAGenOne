Public Class frmMain
    Private Sub NewTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTestToolStripMenuItem.Click
        Try
            frmTestInfo.Show()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub EditConfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditConfigurationToolStripMenuItem.Click
        Try
            frmConfig.Show()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub OpenTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenTestToolStripMenuItem.Click
        Try
            ' Open the file select dialog to allow the user to find the file

            ' Validate the file against the XML schema to verify it is an STA file

            ' Open the testForm with boolOpenTest = true

            ' Set the testForm variable strOpenFileName to the file being opened
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        appDir = My.Application.Info.DirectoryPath
        If (System.IO.File.Exists(appDir & configFileName)) Then
            loadConfiguration()
        Else
            initializeConfiguration()
        End If

    End Sub
End Class