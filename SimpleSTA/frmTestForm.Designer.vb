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
    Private Sub InitializeComponent()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.MainStrip = New System.Windows.Forms.MenuStrip()
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
        Me.btnNoteInjection = New System.Windows.Forms.Button()
        Me.btnStartTest = New System.Windows.Forms.Button()
        Me.TestChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.HideShowSensors = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel4.SuspendLayout()
        CType(Me.TestChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HideShowSensors.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1618, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'MainStrip
        '
        Me.MainStrip.Location = New System.Drawing.Point(0, 0)
        Me.MainStrip.Name = "MainStrip"
        Me.MainStrip.Size = New System.Drawing.Size(1618, 24)
        Me.MainStrip.TabIndex = 1
        Me.MainStrip.Text = "MenuStrip1"
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.Panel4.Location = New System.Drawing.Point(10, 493)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1141, 55)
        Me.Panel4.TabIndex = 4
        '
        'btnApply
        '
        Me.btnApply.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnApply.Location = New System.Drawing.Point(125, 27)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(81, 29)
        Me.btnApply.TabIndex = 14
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'chkScrollEnabled
        '
        Me.chkScrollEnabled.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkScrollEnabled.AutoSize = True
        Me.chkScrollEnabled.Location = New System.Drawing.Point(475, 39)
        Me.chkScrollEnabled.Name = "chkScrollEnabled"
        Me.chkScrollEnabled.Size = New System.Drawing.Size(102, 17)
        Me.chkScrollEnabled.TabIndex = 13
        Me.chkScrollEnabled.Text = "Enable Scrolling"
        Me.chkScrollEnabled.UseVisualStyleBackColor = True
        '
        'chkZoomEnabled
        '
        Me.chkZoomEnabled.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkZoomEnabled.AutoSize = True
        Me.chkZoomEnabled.Location = New System.Drawing.Point(475, 16)
        Me.chkZoomEnabled.Name = "chkZoomEnabled"
        Me.chkZoomEnabled.Size = New System.Drawing.Size(89, 17)
        Me.chkZoomEnabled.TabIndex = 12
        Me.chkZoomEnabled.Text = "Enable Zoom"
        Me.chkZoomEnabled.UseVisualStyleBackColor = True
        '
        'btnZoomOut
        '
        Me.btnZoomOut.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnZoomOut.Location = New System.Drawing.Point(583, 10)
        Me.btnZoomOut.Name = "btnZoomOut"
        Me.btnZoomOut.Size = New System.Drawing.Size(0, 20)
        Me.btnZoomOut.TabIndex = 11
        Me.btnZoomOut.Text = "Zoom Out"
        Me.btnZoomOut.UseVisualStyleBackColor = True
        '
        'btnZoomReset
        '
        Me.btnZoomReset.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnZoomReset.Location = New System.Drawing.Point(583, 41)
        Me.btnZoomReset.Name = "btnZoomReset"
        Me.btnZoomReset.Size = New System.Drawing.Size(0, 20)
        Me.btnZoomReset.TabIndex = 10
        Me.btnZoomReset.Text = "Reset"
        Me.btnZoomReset.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(165, 76)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(27, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Max"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(164, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(27, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Max"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(96, 76)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(24, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Min"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(95, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(24, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Min"
        '
        'txtYMax
        '
        Me.txtYMax.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtYMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYMax.Location = New System.Drawing.Point(151, 51)
        Me.txtYMax.Name = "txtYMax"
        Me.txtYMax.Size = New System.Drawing.Size(0, 22)
        Me.txtYMax.TabIndex = 5
        '
        'txtYMin
        '
        Me.txtYMin.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtYMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYMin.Location = New System.Drawing.Point(80, 51)
        Me.txtYMin.Name = "txtYMin"
        Me.txtYMin.Size = New System.Drawing.Size(0, 22)
        Me.txtYMin.TabIndex = 4
        '
        'txtXMax
        '
        Me.txtXMax.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtXMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXMax.Location = New System.Drawing.Point(151, 10)
        Me.txtXMax.Name = "txtXMax"
        Me.txtXMax.Size = New System.Drawing.Size(0, 22)
        Me.txtXMax.TabIndex = 3
        '
        'txtXMin
        '
        Me.txtXMin.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtXMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXMin.Location = New System.Drawing.Point(80, 10)
        Me.txtXMin.Name = "txtXMin"
        Me.txtXMin.Size = New System.Drawing.Size(0, 22)
        Me.txtXMin.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Y-Range"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "X-Range"
        '
        'btnNoteInjection
        '
        Me.btnNoteInjection.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNoteInjection.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoteInjection.Location = New System.Drawing.Point(3, 48)
        Me.btnNoteInjection.Name = "btnNoteInjection"
        Me.btnNoteInjection.Size = New System.Drawing.Size(232, 0)
        Me.btnNoteInjection.TabIndex = 2
        Me.btnNoteInjection.Text = "Note Injection"
        Me.btnNoteInjection.UseVisualStyleBackColor = True
        '
        'btnStartTest
        '
        Me.btnStartTest.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStartTest.Location = New System.Drawing.Point(60, 42)
        Me.btnStartTest.Margin = New System.Windows.Forms.Padding(60, 3, 3, 3)
        Me.btnStartTest.Name = "btnStartTest"
        Me.btnStartTest.Size = New System.Drawing.Size(105, 0)
        Me.btnStartTest.TabIndex = 0
        Me.btnStartTest.Text = "Start"
        Me.btnStartTest.UseVisualStyleBackColor = True
        '
        'TestChart
        '
        Me.TestChart.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea1.Name = "ChartArea1"
        Me.TestChart.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.TestChart.Legends.Add(Legend1)
        Me.TestChart.Location = New System.Drawing.Point(256, 49)
        Me.TestChart.Name = "TestChart"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.TestChart.Series.Add(Series1)
        Me.TestChart.Size = New System.Drawing.Size(689, 429)
        Me.TestChart.TabIndex = 6
        Me.TestChart.Text = "TestChart"
        '
        'BackgroundWorker1
        '
        '
        'HideShowSensors
        '
        Me.HideShowSensors.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HideShowSensors.AutoScroll = True
        Me.HideShowSensors.Controls.Add(Me.Label7)
        Me.HideShowSensors.Controls.Add(Me.Button1)
        Me.HideShowSensors.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.HideShowSensors.Location = New System.Drawing.Point(956, 52)
        Me.HideShowSensors.Name = "HideShowSensors"
        Me.HideShowSensors.Size = New System.Drawing.Size(195, 429)
        Me.HideShowSensors.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(161, 20)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Show / Hide Sensors:"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(3, 23)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(8, 8)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label8)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label9)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label10)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnStartTest)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNoteInjection)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(10, 52)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(235, 426)
        Me.FlowLayoutPanel1.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.FlowLayoutPanel1.SetFlowBreak(Me.Label8, True)
        Me.Label8.Location = New System.Drawing.Point(3, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(39, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Label8"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.FlowLayoutPanel1.SetFlowBreak(Me.Label9, True)
        Me.Label9.Location = New System.Drawing.Point(3, 13)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(39, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Label9"
        '
        'Label10
        '
        Me.Label10.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.FlowLayoutPanel1.SetFlowBreak(Me.Label10, True)
        Me.Label10.Location = New System.Drawing.Point(3, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Label10"
        '
        'frmTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1618, 836)
        Me.Controls.Add(Me.HideShowSensors)
        Me.Controls.Add(Me.TestChart)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MainStrip)
        Me.MainMenuStrip = Me.MainStrip
        Me.Name = "frmTestForm"
        Me.Text = "Run Test"
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.TestChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HideShowSensors.ResumeLayout(False)
        Me.HideShowSensors.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents MainStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
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
    Friend WithEvents TestChart As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents btnZoomReset As System.Windows.Forms.Button
    Friend WithEvents chkScrollEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents chkZoomEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents btnZoomOut As System.Windows.Forms.Button
    Friend WithEvents HideShowSensors As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnNoteInjection As System.Windows.Forms.Button
    Friend WithEvents btnStartTest As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
