Public Class frmMain
    Private Sub NewTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTestToolStripMenuItem.Click
        Try
            frmTestInfo.Show()
            frmTestInfo.BringToFront()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub EditConfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditConfigurationToolStripMenuItem.Click
        Try
            frmConfig.Show()
            frmConfig.BringToFront()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        appDir = My.Application.Info.DirectoryPath
        If (System.IO.File.Exists(appDir & configFileName)) Then
            loadConfiguration()
            If (verifyConfiguration()) Then
                SystemStatusLabel.Text = "System Status: Configuration Loaded"
            Else
                SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                frmConfig.Show()
                frmConfig.BringToFront()
            End If
        Else
            initializeConfiguration()
            If (verifyConfiguration()) Then
                SystemStatusLabel.Text = "System Status: Configuration Loaded"
            Else
                SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                frmConfig.Show()
                frmConfig.BringToFront()
            End If
        End If
        If (System.IO.File.Exists(config.SystemFileDirectory & "\SystemInfo.xml")) Then
            loadSystemInfo()
            If (verifySystemInfo()) Then
                'SystemStatusLabel.Text = "System Status: Configuration Loaded"
            Else
                'SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                frmConfig.Show()
                frmConfig.BringToFront()
            End If
        Else
            initializeSystemInfo()
            If (verifySystemInfo()) Then
                'SystemStatusLabel.Text = "System Status: Configuration Loaded"
            Else
                'SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                frmConfig.Show()
                frmConfig.BringToFront()
            End If
        End If

    End Sub
End Class