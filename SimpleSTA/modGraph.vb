Imports System.Windows.Forms.DataVisualization.Charting

Module modGraph
    ' --------------------------------------------------------
    ' Zoom and Chart updating functions and event handlers
    ' --------------------------------------------------------
    ' Name: TestChart_AxisViewChanged()
    Public Sub TestChart_AxisViewChanged(sender As Object, e As ViewEventArgs) Handles frmTestForm.TestChart.AxisViewChanged
        Dim dblMin As Double
        dblMin = frmTestForm.TestChart.ChartAreas(0).AxisX.ScaleView.Position
        If dblMin < 0 Then
            frmTestForm.TestChart.ChartAreas(0).AxisX.ScaleView.Position = 0
        Else
            frmTestForm.TestChart.ChartAreas(0).AxisX.ScaleView.Position = dblMin \ 1
        End If
        dblMin = frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.Position
        If dblMin < 0 Then
            frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.Position = 0
        End If
        frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.Position = Math.Round(frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.Position, 1)
        ' Adjust the interval so there are 10 lines shown, and round this interval to an integer
        frmTestForm.TestChart.ChartAreas(0).AxisX.Interval = frmTestForm.TestChart.ChartAreas(0).AxisX.ScaleView.Size \ 10
        ' Adjust the y interval so 10 lines are shown and round interval to the nearest 0.1
        frmTestForm.TestChart.ChartAreas(0).AxisY.Interval = Math.Round(frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.Size / 10, 1)
    End Sub
    ' This method requires the series names to be the same as their legend entries
    Public Sub TestChart_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles frmTestForm.TestChart.MouseDown
        ' Call hit test method
        Try
            Dim result As HitTestResult = frmTestForm.TestChart.HitTest(e.X, e.Y)
            If result.ChartElementType <> ChartElementType.DataPoint Then
                If (result.ChartElementType <> ChartElementType.LegendItem) Then
                    Return
                End If
            End If
            Dim strSeriesName As String = result.Series.Name
            For Each aSeries As Series In frmTestForm.TestChart.Series
                aSeries.BorderWidth = 2
            Next
            For Each aLegend As Legend In frmTestForm.TestChart.Legends
                aLegend.BorderWidth = 2
            Next
            frmTestForm.TestChart.Legends(strSeriesName).BorderWidth = 6
            frmTestForm.TestChart.Series(strSeriesName).BorderWidth = 6
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub btnZoomReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomReset.Click
        Try
            With frmTestForm
                .TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
                .TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
                .TestChart.ChartAreas(0).AxisX.Maximum = Double.NaN
                .TestChart.ChartAreas(0).AxisX.Minimum = 0
                .TestChart.ChartAreas(0).AxisY.Maximum = Double.NaN
                .TestChart.ChartAreas(0).AxisY.Minimum = 0
                .txtXMax.Text = ""
                .txtYMax.Text = ""
                .txtXMin.Text = ""
                .txtYMin.Text = ""
            End With

        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub chkZoomEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZoomEnabled.CheckedChanged
        Try
            If (chkZoomEnabled.Checked) Then
                With frmTestForm.TestChart.ChartAreas(0)
                    .AxisX.ScaleView.Zoomable = True
                    .AxisY.ScaleView.Zoomable = True
                    .AxisX.ScaleView.MinSizeType = DateTimeIntervalType.Number
                    .AxisX.ScaleView.MinSize = cfgGlobal.RecordInterval * 3
                    .AxisY.ScaleView.MinSizeType = DateTimeIntervalType.Number
                    .AxisY.ScaleView.MinSize = 0.5
                    .AxisY.RoundAxisValues()
                    .AxisX.RoundAxisValues()
                    .CursorY.IsUserSelectionEnabled = True
                    .CursorX.IsUserSelectionEnabled = True
                End With

            Else
                With frmTestForm.TestChart.ChartAreas(0)
                    .AxisX.ScaleView.Zoomable = False
                    .AxisY.ScaleView.Zoomable = False
                    .CursorY.IsUserSelectionEnabled = False
                    .CursorX.IsUserSelectionEnabled = False
                End With
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Public Sub chkScrollEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScrollEnabled.CheckedChanged
        Try
            If (frmTestForm.chkZoomEnabled.Checked) Then
                With frmTestForm.TestChart.ChartAreas(0)
                    .CursorX.AutoScroll = True
                    .CursorY.AutoScroll = True
                End With
            Else
                With frmTestForm.TestChart.ChartAreas(0)
                    .CursorX.AutoScroll = False
                    .CursorY.AutoScroll = False
                End With
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Public Sub btnZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZoomOut.Click
        Try
            frmTestForm.TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Try
            If Not (frmTestForm.txtXMax.Text = "") Then
                frmTestForm.TestChart.ChartAreas(0).AxisX.Maximum = frmTestForm.txtXMax.Text
            Else
                frmTestForm.TestChart.ChartAreas(0).AxisX.Maximum = Double.NaN
            End If
            If Not (frmTestForm.txtYMax.Text = "") Then
                frmTestForm.TestChart.ChartAreas(0).AxisY.Maximum = frmTestForm.txtYMax.Text
            End If
            If Not (frmTestForm.txtXMin.Text = "") Then
                frmTestForm.TestChart.ChartAreas(0).AxisX.Minimum = frmTestForm.txtXMin.Text
            End If
            If Not (frmTestForm.txtYMin.Text = "") Then
                frmTestForm.TestChart.ChartAreas(0).AxisY.Minimum = frmTestForm.txtYMin.Text
            End If
            frmTestForm.TestChart.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
            frmTestForm.TestChart.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try

    End Sub
    Public Sub showAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In frmTestForm.TestChart.Series
                aSeries.Enabled = True
                aSeries.IsVisibleInLegend = True
            Next
            For Each ctrl In HideShowSensors.Controls
                If ctrl.GetType.ToString = "System.Windows.Forms.CheckBox" Then
                    ctrl.Checked = True
                End If
            Next
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub hideAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In frmTestForm.TestChart.Series
                aSeries.Enabled = False
                aSeries.IsVisibleInLegend = False
            Next
            For Each ctrl In HideShowSensors.Controls
                If ctrl.GetType.ToString = "System.Windows.Forms.CheckBox" Then
                    ctrl.Checked = False
                End If
            Next
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub UpdateTraces(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If (sender.Checked) Then
                frmTestForm.TestChart.Series(sender.name).Enabled = True
                frmTestForm.TestChart.Series(sender.name).IsVisibleInLegend = True
            Else
                frmTestForm.TestChart.Series(sender.name).Enabled = False
                frmTestForm.TestChart.Series(sender.name).IsVisibleInLegend = False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
End Module
