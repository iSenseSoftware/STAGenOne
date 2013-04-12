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
        Try
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
            If (verifyConfiguration()) Then
                Me.SystemStatusLabel.Text = "System Status: Configuration Loaded"
                Dim options As String
                ' An option string must be explicitly declared or the driver throws a COMException.  This may be fixed by firmware upgrades
                options = "QueryInstStatus=true, RangeCheck=true, Cache=true, Simulate=false, RecordCoercions=false, InterchangeCheck=false"
                switchDriver.Initialize(config.Address, False, False, options)
                If (switchDriver.Initialized()) Then
                    Me.SystemStatusLabel.Text = "System Status: Standby; I/O established."
                    switchDriver.TspLink.Reset()
                    directIOWrapper("node[2].display.clear()")
                    directIOWrapper("node[2].display.settext('System I/O OK')")
                Else
                    MsgBox("Unable to establish connection to test system.  Verify connections and configuration settings and try again.")
                    Me.Close()
                End If
            Else
                Me.SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                frmConfig.Show()
                frmConfig.BringToFront()
            End If

            If (System.IO.File.Exists(config.SystemFileDirectory & "\SystemInfo.xml")) Then
                loadSystemInfo()
                PopulateSystemInfo()
                If (verifySystemInfo()) Then
                    'SystemStatusLabel.Text = "System Status: Configuration Loaded"

                Else
                    'SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                    frmConfig.Show()
                    frmConfig.BringToFront()
                End If
            Else
                initializeSystemInfo()
                PopulateSystemInfo()
                If (verifySystemInfo()) Then
                    'SystemStatusLabel.Text = "System Status: Configuration Loaded"
                Else
                    'SystemStatusLabel.Text = "System Status: Configuration could not be verified.  Update configuration."
                    frmConfig.Show()
                    frmConfig.BringToFront()
                End If
            End If
        Catch ex As Runtime.InteropServices.COMException
            ComExceptionHandler(ex)

        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
End Class