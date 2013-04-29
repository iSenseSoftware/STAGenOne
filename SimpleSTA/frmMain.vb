Public Class frmMain
    Private Sub NewTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If (boolIOEstablished And boolSystemInfoLoaded And boolConfigLoaded) Then
                frmTestInfo.Show()
                frmTestInfo.BringToFront()
            Else
                'If Not boolConfigLoaded Then
                If (System.IO.File.Exists(appDir & "\" & configFileName)) Then
                    Call loadConfiguration(appDir & "\" & configFileName)
                    If (verifyConfiguration(config)) Then
                        boolConfigLoaded = True
                        Me.chkConfigStatus.Checked = True
                        If (initializeDriver()) Then
                            boolIOEstablished = True
                            Me.chkIOStatus.Checked = True
                        Else
                            boolIOEstablished = False
                            Me.chkIOStatus.Checked = False
                        End If
                    Else
                        boolConfigLoaded = False
                        Me.chkConfigStatus.Checked = False
                        MsgBox("Configuration invalid or could not be found.  Verify configuration file")
                    End If
                Else
                    initializeConfiguration()
                    If (verifyConfiguration(config)) Then
                        Me.chkConfigStatus.Checked = True
                        boolConfigLoaded = True
                    Else
                        Me.chkConfigStatus.Checked = False
                        boolConfigLoaded = False
                        MsgBox("Configuration invalid or could not be found.  Verify configuration file")
                    End If
                End If
                'End If
                'If Not boolSystemInfoLoaded Then
                If (boolIOEstablished) Then
                    If (System.IO.File.Exists(config.SystemFileDirectory & "\" & systemInfoFileName)) Then
                        loadSystemInfo(config.SystemFileDirectory & "\" & systemInfoFileName)
                        PopulateSystemInfo()
                        If (verifySystemInfo(testSystemInfo)) Then
                            Me.chkSysInfoStatus.Checked = True
                            boolSystemInfoLoaded = True
                        Else
                            Me.chkSysInfoStatus.Checked = False
                            boolSystemInfoLoaded = False
                            MsgBox("System Info invalid or could not be found.  Verify System Info file")
                        End If
                    Else
                        initializeSystemInfo()
                        PopulateSystemInfo()
                        If (verifySystemInfo(testSystemInfo)) Then
                            Me.chkSysInfoStatus.Checked = True
                            boolSystemInfoLoaded = True
                        Else
                            Me.chkSysInfoStatus.Checked = False
                            boolSystemInfoLoaded = False
                            MsgBox("System Info invalid or could not be found.  Verify System Info file")
                        End If
                    End If
                    'End If
                    If boolIOEstablished And boolSystemInfoLoaded And boolConfigLoaded Then
                        frmTestInfo.Show()
                        frmTestInfo.BringToFront()
                    End If
                Else
                    Me.chkIOStatus.Checked = False
                End If

            End If

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

    Private Sub NewTestToolStripMenuItem_Click1(sender As Object, e As EventArgs) Handles NewTestToolStripMenuItem.Click
        frmTestInfo.Show()
    End Sub
End Class