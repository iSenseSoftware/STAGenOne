' -------------------------------------------------------------------------
' frmMain is, as the name indicates, the main form from which all functions
' are available.  Nothing too interesting happens here.
' ------------------------------------------------------------------------
Option Explicit On
Public Class frmMain

    ' ------------------------------------------
    ' Event Handlers
    ' -------------------------------------------

    ' Name: frmMain_Load
    ' Handles: Opening of program
    ' Description: Attempts to load the configuration file and updates status box

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadOrRefreshConfiguration()
        lblVersion.Text = "Version " & strApplicationVersion
    End Sub
    ' Name: btnNewTest_Click
    ' Handles: User clicks the "New Test" button
    ' Description: Opens the frmTestInfo form allowing the user to enter test info and create a new test
    Private Sub btnNewTest_Click(sender As Object, e As EventArgs) Handles btnNewTest.Click
        If EstablishKeithleyIO(cfgGlobal.Address) = False Then
            MsgBox("Communication not established")
            Exit Sub
        End If

        btnConfig.Enabled = False
        btnNewTest.Enabled = False
        frmTestName.Show()
    End Sub
    ' Name: btnConfig_Click
    ' Handles: User clicks "Configuration" button
    ' Description: Opens the config form! (frmConfig)
    Private Sub btnConfig_Click(sender As Object, e As EventArgs) Handles btnConfig.Click
        Try
            frmConfig.Show()
            frmConfig.BringToFront()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

End Class