Option Explicit On
Imports System.Windows.Forms.DataVisualization.Charting

Public Module modGraph

    ' Name: AddGraphData()
    ' Parameters:
    '           strSensorID: String of sensor ID in the form of "SensorX", where X is a positive integer
    '           intTimePoint: Time index in seconds, a multiple of "interval" seconds
    '           dblCurrentReading: Current reading corresponding to the reading at intTimePoint
    ' Description: 
    Public Sub AddGraphData(strSensorID As String, intTimePoint As Integer, dblCurrentReading As Double)
        'Add data point using the variables passed to the subroutines
        frmTestForm.TestChart.Series(strSensorID).Points.AddXY(intTimePoint, Math.Round(dblCurrentReading, 2))
    End Sub
    ' Name: RefreshGraph()
    ' Parameters:
    '           strSensorID: String of sensor ID in the form of "SensorX", where X is a positive integer
    '           intTimePoint: Time index in seconds, a multiple of "interval" seconds
    '           dblCurrentReading: Current reading corresponding to the reading at intTimePoint
    ' Description: 
    Public Sub RefreshGraph(sender As Object)
        If Not (sender.ChartAreas(0).AxisY.ScaleView.IsZoomed Or sender.ChartAreas(0).AxisX.ScaleView.IsZoomed) Then
            sender.ChartAreas(0).AxisX.Interval = sender.ChartAreas(0).AxisX.Maximum \ 10
            sender.ChartAreas(0).AxisY.Interval = Math.Round(sender.ChartAreas(0).AxisY.Maximum / 10, 1)
        Else
            ' do nothing
        End If
        sender.Update()
    End Sub


    ' --------------------------------------------------------
    ' Zoom and Chart updating functions and event handlers
    ' --------------------------------------------------------
    ' Name: TestChart_AxisViewChanged()
    Public Sub AxisViewChanged(sender As Object, e As ViewEventArgs)
        Dim dblMin As Double
        dblMin = sender.ChartAreas(0).AxisX.ScaleView.Position
        If dblMin < 0 Then
            sender.ChartAreas(0).AxisX.ScaleView.Position = 0
        Else
            sender.ChartAreas(0).AxisX.ScaleView.Position = dblMin \ 1
        End If
        dblMin = sender.ChartAreas(0).AxisY.ScaleView.Position
        If dblMin < 0 Then
            sender.ChartAreas(0).AxisY.ScaleView.Position = 0
        End If
        sender.ChartAreas(0).AxisY.ScaleView.Position = Math.Round(sender.ChartAreas(0).AxisY.ScaleView.Position, 1)
        ' Adjust the interval so there are 10 lines shown, and round this interval to an integer
        sender.TestChart.ChartAreas(0).AxisX.Interval = sender.ChartAreas(0).AxisX.ScaleView.Size \ 10
        ' Adjust the y interval so 10 lines are shown and round interval to the nearest 0.1
        sender.ChartAreas(0).AxisY.Interval = Math.Round(sender.ChartAreas(0).AxisY.ScaleView.Size / 10, 1)
    End Sub

    ' This method requires the series names to be the same as their legend entries
    Public Sub ChartMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        ' Call hit test method
        Try
            Dim result As HitTestResult = sender.HitTest(e.X, e.Y)
            If result.ChartElementType <> ChartElementType.DataPoint Then
                If (result.ChartElementType <> ChartElementType.LegendItem) Then
                    Return
                End If
            End If
            Dim strSeriesName As String = result.Series.Name
            For Each aSeries As Series In sender.Series
                aSeries.BorderWidth = 2
            Next
            For Each aLegend As Legend In sender.Legends
                aLegend.BorderWidth = 2
            Next
            sender.Legends(strSeriesName).BorderWidth = 6
            sender.Series(strSeriesName).BorderWidth = 6
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ZoomReset(ChartName As Chart, ByVal sender As System.Object)
        Try
            With ChartName
                .ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
                .ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
                .ChartAreas(0).AxisX.Maximum = Double.NaN
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = Double.NaN
                .ChartAreas(0).AxisY.Minimum = 0
            End With
            With sender
                .txtXMax.Text = ""
                .txtYMax.Text = ""
                .txtXMin.Text = ""
                .txtYMin.Text = ""
            End With


        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ZoomEnabled(ChartName As Chart, ByVal sender As System.Object)
        Try
            If (sender.chkZoomEnabled.Checked) Then
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
    Public Sub ScrollEnabled(ChartName As Chart, ByVal sender As System.Object)
        Try
            If (sender.chkZoomEnabled.Checked) Then
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
    Public Sub ZoomOut(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            sender.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            sender.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ApplyButton(ChartName As Chart, sender As System.Object)
        Try
            If Not (sender.txtXMax.Text = "") Then
                ChartName.ChartAreas(0).AxisX.Maximum = sender.txtXMax.Text
            Else
                ChartName.ChartAreas(0).AxisX.Maximum = Double.NaN
            End If
            If Not (sender.txtYMax.Text = "") Then
                ChartName.ChartAreas(0).AxisY.Maximum = sender.txtYMax.Text
            End If
            If Not (sender.txtXMin.Text = "") Then
                ChartName.ChartAreas(0).AxisX.Minimum = sender.txtXMin.Text
            End If
            If Not (sender.txtYMin.Text = "") Then
                ChartName.ChartAreas(0).AxisY.Minimum = sender.txtYMin.Text
            End If
            ChartName.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
            ChartName.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub ShowAllButton(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In sender.Series
                aSeries.Enabled = True
                aSeries.IsVisibleInLegend = True
            Next
            For Each ctrl In sender.HideShowSensors.Controls
                If ctrl.GetType.ToString = "System.Windows.Forms.CheckBox" Then
                    ctrl.Checked = True
                End If
            Next
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub
    Public Sub HideAllButton(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each aSeries In sender.Series
                aSeries.Enabled = False
                aSeries.IsVisibleInLegend = False
            Next
            For Each ctrl In sender.HideShowSensors.Controls
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
                sender.Series(sender.name).Enabled = True
                sender.Series(sender.name).IsVisibleInLegend = True
            Else
                sender.Series(sender.name).Enabled = False
                sender.Series(sender.name).IsVisibleInLegend = False
            End If
        Catch ex As Exception
            GenericExceptionHandler(ex)
        End Try
    End Sub




End Module
