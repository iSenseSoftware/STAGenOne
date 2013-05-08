' -------------------------------------------------------------------------
' frmMain is, as the name indicates, the main form from which all functions
' are available.  Nothing too interesting happens here.
' ------------------------------------------------------------------------
Option Explicit On
Public Class frmMain
    ' ------------------------------------------
    ' Event Handlers
    ' -------------------------------------------
    ' Name: EditConfigurationToolStripMenuItem_Click()
    ' Handles: User clicks "Edit Configuration" button
    ' Description: Opens the config form! (frmConfig)
    Private Sub EditConfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditConfigurationToolStripMenuItem.Click
        Try
            frmConfig.Show()
            frmConfig.BringToFront()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: NewTestToolStripMenuItem_Click()
    ' Handles: User clicks the "New Test" button
    ' Description: Opens the frmTestInfo form allowing the user to enter test info and create a new test
    Private Sub NewTestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewTestToolStripMenuItem.Click
        frmTestInfo.Show()
        'Dim comm As New SerialCommunicator()
        'comm.Initialize()
        'comm.SendCommand("display.clear()")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        frmAbout.Show()
    End Sub
End Class