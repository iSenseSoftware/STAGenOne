Public Class frmMain
    Private Sub NewTestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTestToolStripMenuItem.Click
        Try
            If (boolIOEstablished And boolSystemInfoLoaded And boolConfigLoaded) Then
                frmTestInfo.Show()
                frmTestInfo.BringToFront()
            Else
                'If Not boolConfigLoaded Then
                If (System.IO.File.Exists(appDir & configFileName)) Then
                    loadConfiguration()
                    If (verifyConfiguration()) Then
                        boolConfigLoaded = True
                        If (initializeDriver()) Then
                            boolIOEstablished = True
                        Else
                            MsgBox("System I/O could not be established.  Verify config settings")
                            Exit Sub
                        End If
                    Else
                        boolConfigLoaded = False
                        MsgBox("Configuration invalid or could not be found.  Verify configuration file")
                    End If
                Else
                    initializeConfiguration()
                    If (verifyConfiguration()) Then
                        boolConfigLoaded = True
                    Else
                        boolConfigLoaded = False
                        MsgBox("Configuration invalid or could not be found.  Verify configuration file")
                    End If
                End If
                'End If
                'If Not boolSystemInfoLoaded Then
                If (System.IO.File.Exists(config.SystemFileDirectory & "\SystemInfo.xml") And boolIOEstablished) Then
                    loadSystemInfo()
                    PopulateSystemInfo()
                    If (verifySystemInfo()) Then
                        boolSystemInfoLoaded = True
                    Else
                        boolSystemInfoLoaded = False
                        MsgBox("System Info invalid or could not be found.  Verify System Info file")
                    End If
                Else
                    initializeSystemInfo()
                    PopulateSystemInfo()
                    If (verifySystemInfo()) Then
                        boolSystemInfoLoaded = True
                    Else
                        boolSystemInfoLoaded = False
                        MsgBox("System Info invalid or could not be found.  Verify System Info file")
                    End If
                End If
                'End If
                If boolIOEstablished And boolSystemInfoLoaded And boolConfigLoaded Then
                    frmTestInfo.Show()
                    frmTestInfo.BringToFront()
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
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            appDir = My.Application.Info.DirectoryPath
            'If (System.IO.File.Exists(appDir & configFileName)) Then
            ' loadConfiguration()
            ' If (verifyConfiguration()) Then
            ' boolConfigLoaded = True
            ' Else
            ' boolConfigLoaded = False
            ' End If
            ' Else
            ' initializeConfiguration()
            ' If (verifyConfiguration()) Then
            ' boolConfigLoaded = True
            ' Else
            ' boolConfigLoaded = False
            ' End If
            ' End If
            ' If (System.IO.File.Exists(config.SystemFileDirectory & "\SystemInfo.xml") And checkIOStatus()) Then
            ' loadSystemInfo()
            ' PopulateSystemInfo()
            ' If (verifySystemInfo()) Then
            ' boolSystemInfoLoaded = True
            ' Else
            ' boolSystemInfoLoaded = False
            ' End If
            ' Else
            ' initializeSystemInfo()
            ' PopulateSystemInfo()
            ' If (verifySystemInfo()) Then
            ' boolSystemInfoLoaded = True
            ' Else
            ' boolSystemInfoLoaded = False
            ' End If
            ' End If
        Catch ex As Runtime.InteropServices.COMException
            ComExceptionHandler(ex)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
End Class