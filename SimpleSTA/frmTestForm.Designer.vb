<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Public Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTestForm))
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.MainLoop = New System.ComponentModel.BackgroundWorker()
        Me.TestChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.chkScrollEnabled = New System.Windows.Forms.CheckBox()
        Me.chkZoomEnabled = New System.Windows.Forms.CheckBox()
        Me.btnZoomOut = New System.Windows.Forms.Button()
        Me.btnZoomReset = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtYMax = New System.Windows.Forms.TextBox()
        Me.txtYMin = New System.Windows.Forms.TextBox()
        Me.txtXMax = New System.Windows.Forms.TextBox()
        Me.txtXMin = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.flwInjections = New System.Windows.Forms.FlowLayoutPanel()
        Me.txtTestName = New System.Windows.Forms.Label()
        Me.txtOperator = New System.Windows.Forms.Label()
        Me.txtTime = New System.Windows.Forms.Label()
        Me.txtTimeSinceInjection = New System.Windows.Forms.Label()
        Me.txtInjections = New System.Windows.Forms.Label()
        Me.btnStartTest = New System.Windows.Forms.Button()
        Me.btnNoteInjection = New System.Windows.Forms.Button()
        Me.HideShowSensors = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ElapsedTimer = New System.Windows.Forms.Timer(Me.components)
        Me.InjectionTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.TestChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.flwInjections.SuspendLayout()
        Me.HideShowSensors.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainLoop
        '
        '
        'TestChart
        '
        Me.TestChart.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        ChartArea1.Name = "ChartArea1"
        Me.TestChart.ChartAreas.Add(ChartArea1)
        resources.ApplyResources(Me.TestChart, "TestChart")
        Legend1.Name = "Legend1"
        Me.TestChart.Legends.Add(Legend1)
        Me.TestChart.Name = "TestChart"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.TestChart.Series.Add(Series1)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TestChart)
        Me.Panel1.Controls.Add(Me.Panel4)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.btnApply)
        Me.Panel4.Controls.Add(Me.chkScrollEnabled)
        Me.Panel4.Controls.Add(Me.chkZoomEnabled)
        Me.Panel4.Controls.Add(Me.btnZoomOut)
        Me.Panel4.Controls.Add(Me.btnZoomReset)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.txtYMax)
        Me.Panel4.Controls.Add(Me.txtYMin)
        Me.Panel4.Controls.Add(Me.txtXMax)
        Me.Panel4.Controls.Add(Me.txtXMin)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.Label1)
        resources.ApplyResources(Me.Panel4, "Panel4")
        Me.Panel4.Name = "Panel4"
        '
        'btnApply
        '
        resources.ApplyResources(Me.btnApply, "btnApply")
        Me.btnApply.Name = "btnApply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'chkScrollEnabled
        '
        resources.ApplyResources(Me.chkScrollEnabled, "chkScrollEnabled")
        Me.chkScrollEnabled.Name = "chkScrollEnabled"
        Me.chkScrollEnabled.UseVisualStyleBackColor = True
        '
        'chkZoomEnabled
        '
        resources.ApplyResources(Me.chkZoomEnabled, "chkZoomEnabled")
        Me.chkZoomEnabled.Name = "chkZoomEnabled"
        Me.chkZoomEnabled.UseVisualStyleBackColor = True
        '
        'btnZoomOut
        '
        resources.ApplyResources(Me.btnZoomOut, "btnZoomOut")
        Me.btnZoomOut.Name = "btnZoomOut"
        Me.btnZoomOut.UseVisualStyleBackColor = True
        '
        'btnZoomReset
        '
        resources.ApplyResources(Me.btnZoomReset, "btnZoomReset")
        Me.btnZoomReset.Name = "btnZoomReset"
        Me.btnZoomReset.UseVisualStyleBackColor = True
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'txtYMax
        '
        resources.ApplyResources(Me.txtYMax, "txtYMax")
        Me.txtYMax.Name = "txtYMax"
        '
        'txtYMin
        '
        resources.ApplyResources(Me.txtYMin, "txtYMin")
        Me.txtYMin.Name = "txtYMin"
        '
        'txtXMax
        '
        resources.ApplyResources(Me.txtXMax, "txtXMax")
        Me.txtXMax.Name = "txtXMax"
        '
        'txtXMin
        '
        resources.ApplyResources(Me.txtXMin, "txtXMin")
        Me.txtXMin.Name = "txtXMin"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.flwInjections)
        Me.Panel2.Controls.Add(Me.btnStartTest)
        Me.Panel2.Controls.Add(Me.btnNoteInjection)
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.Name = "Panel2"
        '
        'flwInjections
        '
        resources.ApplyResources(Me.flwInjections, "flwInjections")
        Me.flwInjections.Controls.Add(Me.txtTestName)
        Me.flwInjections.Controls.Add(Me.txtOperator)
        Me.flwInjections.Controls.Add(Me.txtTime)
        Me.flwInjections.Controls.Add(Me.txtTimeSinceInjection)
        Me.flwInjections.Controls.Add(Me.txtInjections)
        Me.flwInjections.Name = "flwInjections"
        '
        'txtTestName
        '
        resources.ApplyResources(Me.txtTestName, "txtTestName")
        Me.flwInjections.SetFlowBreak(Me.txtTestName, True)
        Me.txtTestName.Name = "txtTestName"
        '
        'txtOperator
        '
        resources.ApplyResources(Me.txtOperator, "txtOperator")
        Me.flwInjections.SetFlowBreak(Me.txtOperator, True)
        Me.txtOperator.Name = "txtOperator"
        '
        'txtTime
        '
        resources.ApplyResources(Me.txtTime, "txtTime")
        Me.flwInjections.SetFlowBreak(Me.txtTime, True)
        Me.txtTime.Name = "txtTime"
        '
        'txtTimeSinceInjection
        '
        resources.ApplyResources(Me.txtTimeSinceInjection, "txtTimeSinceInjection")
        Me.flwInjections.SetFlowBreak(Me.txtTimeSinceInjection, True)
        Me.txtTimeSinceInjection.Name = "txtTimeSinceInjection"
        '
        'txtInjections
        '
        resources.ApplyResources(Me.txtInjections, "txtInjections")
        Me.flwInjections.SetFlowBreak(Me.txtInjections, True)
        Me.txtInjections.Name = "txtInjections"
        '
        'btnStartTest
        '
        resources.ApplyResources(Me.btnStartTest, "btnStartTest")
        Me.btnStartTest.Name = "btnStartTest"
        Me.btnStartTest.UseVisualStyleBackColor = True
        '
        'btnNoteInjection
        '
        resources.ApplyResources(Me.btnNoteInjection, "btnNoteInjection")
        Me.btnNoteInjection.Name = "btnNoteInjection"
        Me.btnNoteInjection.UseVisualStyleBackColor = True
        '
        'HideShowSensors
        '
        resources.ApplyResources(Me.HideShowSensors, "HideShowSensors")
        Me.HideShowSensors.Controls.Add(Me.Label7)
        Me.HideShowSensors.Name = "HideShowSensors"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'ElapsedTimer
        '
        Me.ElapsedTimer.Enabled = True
        Me.ElapsedTimer.Interval = 1000
        '
        'InjectionTimer
        '
        Me.InjectionTimer.Enabled = True
        Me.InjectionTimer.Interval = 1000
        '
        'frmTestForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.HideShowSensors)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "frmTestForm"
        CType(Me.TestChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.flwInjections.ResumeLayout(False)
        Me.flwInjections.PerformLayout()
        Me.HideShowSensors.ResumeLayout(False)
        Me.HideShowSensors.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainLoop As System.ComponentModel.BackgroundWorker
    Friend WithEvents TestChart As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents chkScrollEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents chkZoomEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents btnZoomOut As System.Windows.Forms.Button
    Friend WithEvents btnZoomReset As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtYMax As System.Windows.Forms.TextBox
    Friend WithEvents txtYMin As System.Windows.Forms.TextBox
    Friend WithEvents txtXMax As System.Windows.Forms.TextBox
    Friend WithEvents txtXMin As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnStartTest As System.Windows.Forms.Button
    Friend WithEvents btnNoteInjection As System.Windows.Forms.Button
    Friend WithEvents HideShowSensors As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtTestName As System.Windows.Forms.Label
    Friend WithEvents txtInjections As System.Windows.Forms.Label
    Friend WithEvents flwInjections As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents txtTimeSinceInjection As System.Windows.Forms.Label
    Friend WithEvents txtOperator As System.Windows.Forms.Label
    Friend WithEvents ElapsedTimer As System.Windows.Forms.Timer
    Friend WithEvents txtTime As System.Windows.Forms.Label
    Friend WithEvents InjectionTimer As System.Windows.Forms.Timer
End Class
