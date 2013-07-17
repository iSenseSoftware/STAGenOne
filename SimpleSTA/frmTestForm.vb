﻿Option Explicit On
Imports System
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
'Imports Keithley.Ke37XX
'Imports Ivi.Driver.Interop
'Imports Keithley.Ke37XX.Interop
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization
' --------------------------------------------------------------------------------------------------
' frmTestForm is the interface presented to the user during the actual conduct of a sensor test.
' It contains the following major components:
' 1. Current vs. Time chart with traces for each sensor being tested
' 2. Show/Hide menu allowing user to toggle the display of each sensor individually
' 3. A set of elements allowing the user to customize the look of the current vs. time chart
' 4. Basic information about the test (test name, elapsed time, etc)
' 5. A summary of user-noted injections
' 6. Buttons to start and stop test
' 7. Button to note analyte injections
' --------------------------------------------------------------------------------------------------
Public Class frmTestForm
    Dim dblBias As Double
    Dim dblRecordInterval As Double
    Dim strCurrentRange As String
    Dim strFilterType As String
    Dim lngSamples As Long
    Dim lngNPLC As Long
    Dim strName As String
    Dim strSTAID As String
    Dim strDumpDir As String
    Dim strCardConfig As String




    
    ' Name: TestForm_FormClosing()
    ' Handles: User closes frmTestForm
    ' Description: Behavior is dependent on the state of the test.  If the MainLoop is still running, it warns the user they cannot close the 
    ' form while the test is running.  Otherwise it closes the test form and resets the switchdriver and its attendant UI element
    ' indicating IO has been broken.
    Private Sub TestForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            ' Close the instrument connection when exiting the test form
            If boolIsTestStopped Then
                If (switchDriver.Connected) Then
                    switchDriver.Close()
                    boolIOStatus = False
                End If
            Else
                e.Cancel = True
                MsgBox("Cannot close test form while test is running.  Stop test and close form.", vbOKOnly)
            End If
        Catch comEx As COMException
            ComExceptionHandler(comEx)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: TestForm_Load()
    ' Handles: The frmTestForm is opened
    ' Description: Enabled the start and note injection buttons, populates the form with test specific data
    ' and sets the display text on the SMU to indicate readiness to perform a test
    Private Sub TestForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            btnStartTest.Show()
            btnNoteInjection.Show()
            ' Set the background worker to report progress so that it can make cross-thread communications to the chart updater
            MainLoop.WorkerReportsProgress = True
            prepareForm()
            SwitchIOWrite("node[2].display.clear()")
            SwitchIOWrite("node[2].display.settext('Ready to test')")
        Catch ex As COMException
            ComExceptionHandler(ex)
            Me.Close()
        Catch ex As Exception
            GenericExceptionHandler(ex)
            Me.Close()
        End Try
    End Sub
    Private Sub MainLoop_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles MainLoop.DoWork
        MeasurementLoop(sender, e)
    End Sub



    ' --------------------------------------------
    ' Timer loop threads
    ' ---------------------------------------------
    ' Timer1_Tick and InjectionTime_Tick are both triggered every second by the Timer1 and InjectionTimer components in the user form.
    ' There are corresponding stopwatches that are managed within the test control loop
    ' and these are referenced within the Tick events to update the elapsed time shown to the user.

    ' Name: ElapsedTimer_Tick()
    ' Handles: Tick event for ElapsedTimer
    ' Description: The ElapsedTimer triggers the Tick event every second.  This sub runs when this event is triggered.  
    '           It grabs the the time elapsed in the stpTotalTime stopwatch, parses it into human-readable format and updates
    '           the user interface
    Private Sub ElapsedTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ElapsedTimer.Tick
        Try
            ' Update the total time timer in the user interface
            txtTime.Text = "Total Time: " & StrPad(stpTotalTime.Elapsed.Hours, 2) & ":" & StrPad(stpTotalTime.Elapsed.Minutes, 2) & ":" & StrPad(stpTotalTime.Elapsed.Seconds, 2)
            tfCurrentTestFile.TestLength = stpTotalTime.Elapsed.TotalSeconds
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Name: InjectionTimer_Tick()
    ' Handles: Tick event for InjectionTimer
    ' Description: The InjectionTimer triggers the Tick event every second.  This sub runs when this event is triggered.  
    '           It grabs the the time elapsed in the stpInjectionTime stopwatch, parses it into human-readable format and updates
    '           the user interface
    Private Sub InjectionTimer_Tick(sender As Object, e As EventArgs) Handles InjectionTimer.Tick
        Try
            ' Updat the time since injection in the user interface
            If Not boolIsTestStopped Then
                txtTimeSinceInjection.Text = "Time Since Injection: " & StrPad(stpInjectionTime.Elapsed.Hours, 2) & ":" & StrPad(stpInjectionTime.Elapsed.Minutes, 2) & ":" & StrPad(stpInjectionTime.Elapsed.Seconds, 2)
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    ' Name: btnNoteInjection_Click
    ' Handles: User clicks 'Note Injection' button
    ' Description: Adds a new timestamp to the TestFile Injections array and resets the stpInjectionTime stopwatch.
    Private Sub btnNoteInjection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoteInjection.Click
        ' Add the current time to the test file injections array
        Try
            Dim timestamp As DateTime = DateTime.Now()
            fCurrentTestFile.addInjection(timestamp)
            Dim txtNewInjection As New Label
            txtNewInjection.Text = StrPad(stpTotalTime.Elapsed.Hours, 2) & ":" & StrPad(stpTotalTime.Elapsed.Minutes, 2)
            flwInjections.Controls.Add(txtNewInjection)
            If (stpInjectionTime.IsRunning) Then
                stpInjectionTime.Restart()
            Else
                stpInjectionTime.Start()
            End If
            MsgBox("Injection noted", vbOKOnly)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    ' Sub: btnStartTest_Click()
    ' Handles: User clicks the Start/Stop button
    ' If a test is not already running, this sub starts the test thread (BackgroundWorker1)
    ' If the test is running it initiates the test stop sequence by setting the boolIsTestRunning
    ' flag to false and disabling further user interaction with the start/stop button
    Private Sub btnStartTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartTest.Click
        Try
            If (boolIsTestRunning) Then
                boolIsTestRunning = False
                btnStartTest.Text = "Ending Test"
                btnStartTest.Enabled = False
                btnNoteInjection.Enabled = False
            Else
                boolIsTestRunning = True
                btnStartTest.Text = "Stop"
                MainLoop.RunWorkerAsync()
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    ' ----------------------------------------------
    ' Utility functions
    ' ----------------------------------------------
    Private Sub prepareForm()
        Try
            ' Clear the default or previous series and legends from the test chart
            TestChart.Series.Clear()
            TestChart.Legends.Clear()
            txtTestName.Text = "Test Name: " & fCurrentTestFile.Name
            txtOperator.Text = "Operator: " & frmTestInfo.txt
            ' Configure the test chart
            With TestChart.ChartAreas(0)
                .CursorX.AutoScroll = False
                .CursorY.AutoScroll = False
                .CursorX.IsUserEnabled = True
                .CursorY.IsUserEnabled = True
                .CursorX.Interval = 0
                .CursorY.Interval = 0
                .AxisX.ScaleView.MinSize = 0
                .AxisY.ScaleView.MinSize = 0
                .AxisY.Minimum = 0
                .AxisX.Minimum = 0
                .AxisX.Title = "Elapsed Time (s)"
                .AxisY.Title = "Current (nA)"
                ' Setting IsMarginVisible to false increases the accuracy of deep zooming.  If this is true then zooms are padded
                ' and do not show the actual area selected
                .AxisX.IsMarginVisible = False
                .AxisY.IsMarginVisible = False
                .Name = "Main"
            End With
            ' Populate the chart series and legend
            For i = 1 To 32
                TestChart.Series.Add("Sensor" & i)
                With TestChart.Series("Sensor" & i)
                    .ChartType = SeriesChartType.Line
                    .BorderWidth = 2
                    ' other properties go here later
                End With
                TestChart.Legends.Add("Sensor" & i)
                With TestChart.Legends("Sensor" & i)
                    .Title = "Sensor" & i
                    .BorderColor = Color.Black
                    .BorderWidth = 2
                    .LegendStyle = LegendStyle.Column
                    .DockedToChartArea = "Main"
                    .IsDockedInsideChartArea = True
                End With
                Dim chkNewBox As New CheckBox
                With chkNewBox
                    .Width = 140
                    .Name = "Sensor" & i
                    .Text = "Sensor" & i
                    .Enabled = True
                    .Visible = True
                    .Checked = True
                    AddHandler chkNewBox.Click, AddressOf UpdateTraces
                End With
                HideShowSensors.Controls.Add(chkNewBox)
            Next
            Dim btnShowAllButton As New Button
            With btnShowAllButton
                .Name = "btnShowAllSensors"
                .Text = "Show All"
                .Font = New Font("Microsoft Sans Serif", 12)
                .AutoSize = True
            End With
            HideShowSensors.Controls.Add(btnShowAllButton)
            Dim btnHideAllButton As New Button
            With btnHideAllButton
                .Name = "btnHideAllSensors"
                .Text = "Hide All"
                .Font = New Font("Microsoft Sans Serif", 12)
                .AutoSize = True
            End With
            HideShowSensors.Controls.Add(btnHideAllButton)
            AddHandler btnShowAllButton.Click, AddressOf showAllButton_Click
            AddHandler btnHideAllButton.Click, AddressOf hideAllButton_Click
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub

    Public Sub TestChart_AxisViewChanged(sender As Object, e As ViewEventArgs) Handles TestChart.AxisViewChanged
        AxisViewChanged(TestChart)
    End Sub
    Public Sub TestChart_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TestChart.MouseDown
        ChartMouseDown(TestChart)
    End Sub

    Public Sub btnZoomReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomReset.Click
        ZoomReset(TestChart)
    End Sub
    Public Sub chkZoomEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZoomEnabled.CheckedChanged
        ZoomEnabled(TestChart)
    End Sub
    Public Sub chkScrollEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScrollEnabled.CheckedChanged
        ScrollEnabled(TestChart)
    End Sub
    Public Sub btnZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomOut.Click
        ZoomOut(TestChart)
    End Sub
    Public Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        ApplyButton(TestChart)
    End Sub
    Public Sub showAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ShowAllButton(TestChart)
    End Sub
    Public Sub hideAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        HideAllButton(TestChart)
    End Sub


End Class