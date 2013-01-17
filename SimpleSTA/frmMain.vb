Public Class frmMain

    Private Sub FileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileToolStripMenuItem.Click

    End Sub

    Private Sub NewTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTestToolStripMenuItem.Click
        frmTestInfo.Show()
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub EditConfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditConfigurationToolStripMenuItem.Click
        frmConfig.Show()
    End Sub

    Private Sub OpenTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenTestToolStripMenuItem.Click
        ' Open the file select dialog to allow the user to find the file

        ' Validate the file against the XML schema to verify it is an STA file

        ' Open the testForm with boolOpenTest = true

        ' Set the testForm variable strOpenFileName to the file being opened

    End Sub
End Class