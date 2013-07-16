Option Explicit On
Imports System.Windows.Forms.DataVisualization.Charting

Public Module modGraph
    ' --------------------------------------------------------
    ' Zoom and Chart updating functions and event handlers
    ' --------------------------------------------------------
    ' Name: TestChart_AxisViewChanged()
    Public Sub AxisViewChanged(ChartName As Chart)
        Dim dblMin As Double
        dblMin = ChartName.ChartAreas(0).AxisX.ScaleView.Position
        If dblMin < 0 Then
            ChartName.ChartAreas(0).AxisX.ScaleView.Position = 0
        Else
            ChartName.ChartAreas(0).AxisX.ScaleView.Position = dblMin \ 1
        End If
        dblMin = ChartName.ChartAreas(0).AxisY.ScaleView.Position
        If dblMin < 0 Then
            ChartName.ChartAreas(0).AxisY.ScaleView.Position = 0
        End If
        ChartName.ChartAreas(0).AxisY.ScaleView.Position = Math.Round(ChartName.ChartAreas(0).AxisY.ScaleView.Position, 1)
        ' Adjust the interval so there are 10 lines shown, and round this interval to an integer
        ChartName.TestChart.ChartAreas(0).AxisX.Interval = ChartName.ChartAreas(0).AxisX.ScaleView.Size \ 10
        ' Adjust the y interval so 10 lines are shown and round interval to the nearest 0.1
        ChartName.ChartAreas(0).AxisY.Interval = Math.Round(ChartName.ChartAreas(0).AxisY.ScaleView.Size / 10, 1)
    End Sub

    ' This method requires the series names to be the same as their legend entries
    Public Sub ChartMouseDown(ChartName As Chart)
        ' Call hit test method
        Try
            Dim result As HitTestResult = ChartName.HitTest(e.X, e.Y)
            If result.ChartElementType <> ChartElementType.DataPoint Then
                If (result.ChartElementType <> ChartElementType.LegendItem) Then
                    Return
                End If
            End If
            Dim strSeriesName As String = result.Series.Name
            For Each aSeries As Series In ChartName.Series
                aSeries.BorderWidth = 2
            Next
            For Each aLegend As Legend In ChartName.Legends
                aLegend.BorderWidth = 2
            Next
            ChartName.Legends(strSeriesName).BorderWidth = 6
            ChartName.Series(strSeriesName).BorderWidth = 6
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ZoomReset(ChartName As Chart)
        Try
            With 
                .ChartName.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
                .ChartName.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
                .ChartName.ChartAreas(0).AxisX.Maximum = Double.NaN
                .ChartName.ChartAreas(0).AxisX.Minimum = 0
                .ChartName.ChartAreas(0).AxisY.Maximum = Double.NaN
                .ChartName.ChartAreas(0).AxisY.Minimum = 0
                .txtXMax.Text = ""
                .txtYMax.Text = ""
                .txtXMin.Text = ""
                .txtYMin.Text = ""
            End With

        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ZoomEnabled(ChartName As Chart)
        Try
            If (chkZoomEnabled.Checked) Then
                With ChartName.ChartAreas(0)
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
                With ChartName.ChartAreas(0)
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
    Public Sub ScrollEnabled(ChartName As Chart)
        Try
            If (ChartName.chkZoomEnabled.Checked) Then
                With ChartName.ChartAreas(0)
                    .CursorX.AutoScroll = True
                    .CursorY.AutoScroll = True
                End With
            Else
                With ChartName.ChartAreas(0)
                    .CursorX.AutoScroll = False
                    .CursorY.AutoScroll = False
                End With
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ZoomOut(ChartName As Chart)
        Try
            ChartName.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            ChartName.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ApplyButton(ChartName As Chart)
        Try
            If Not (ChartName.txtXMax.Text = "") Then
                ChartName.ChartAreas(0).AxisX.Maximum = ChartName.txtXMax.Text
            Else
                ChartName.ChartAreas(0).AxisX.Maximum = Double.NaN
            End If
            If Not (ChartName.txtYMax.Text = "") Then
                ChartName.ChartAreas(0).AxisY.Maximum = ChartName.txtYMax.Text
            End If
            If Not (ChartName.txtXMin.Text = "") Then
                ChartName.ChartAreas(0).AxisX.Minimum = ChartName.txtXMin.Text
            End If
            If Not (ChartName.txtYMin.Text = "") Then
                ChartName.ChartAreas(0).AxisY.Minimum = ChartName.txtYMin.Text
            End If
            ChartName.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
            ChartName.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ShowAllButton(ChartName As Chart)
        Try
            For Each aSeries In ChartName.Series
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
    Public Sub HideAllButton(ChartName As Chart)
        Try
            For Each aSeries In ChartName.Series
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
    Public Sub UpdateTraces()
        Try
            If (sender.Checked) Then
                ChartName.Series(sender.name).Enabled = True
                ChartName.Series(sender.name).IsVisibleInLegend = True
            Else
                ChartName.Series(sender.name).Enabled = False
                ChartName.Series(sender.name).IsVisibleInLegend = False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub



        
End Module
