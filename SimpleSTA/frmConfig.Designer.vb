<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSTAID = New System.Windows.Forms.TextBox()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.cmbCardConfig = New System.Windows.Forms.ComboBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cmbRange = New System.Windows.Forms.ComboBox()
        Me.txtBias = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNPLC = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtInterval = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbFilterType = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtSamples = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnSelDataDir = New System.Windows.Forms.Button()
        Me.txtDataDir = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.txtSettlingTime = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtRow3Resistor = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtRow4Resistor = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtRow5Resistor = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtTolerance = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtRow6Resistor = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtAuditZero = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.LineShape3 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.LineShape2 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(10, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "# of Cards:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(276, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "STA ID:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "3706A Address:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(14, 175)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 16)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Range (nA):"
        '
        'txtSTAID
        '
        Me.txtSTAID.Enabled = False
        Me.txtSTAID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSTAID.Location = New System.Drawing.Point(333, 15)
        Me.txtSTAID.Name = "txtSTAID"
        Me.txtSTAID.Size = New System.Drawing.Size(130, 22)
        Me.txtSTAID.TabIndex = 1
        '
        'txtAddress
        '
        Me.txtAddress.Enabled = False
        Me.txtAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress.Location = New System.Drawing.Point(124, 12)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(130, 22)
        Me.txtAddress.TabIndex = 0
        '
        'cmbCardConfig
        '
        Me.cmbCardConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCardConfig.Enabled = False
        Me.cmbCardConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCardConfig.FormattingEnabled = True
        Me.cmbCardConfig.Items.AddRange(New Object() {"2"})
        Me.cmbCardConfig.Location = New System.Drawing.Point(124, 50)
        Me.cmbCardConfig.Name = "cmbCardConfig"
        Me.cmbCardConfig.Size = New System.Drawing.Size(130, 24)
        Me.cmbCardConfig.TabIndex = 2
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(142, 489)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(117, 35)
        Me.btnSave.TabIndex = 20
        Me.btnSave.Text = "Edit"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(278, 489)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(117, 35)
        Me.btnCancel.TabIndex = 21
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRange.Enabled = False
        Me.cmbRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Items.AddRange(New Object() {"100", "1000", "10000"})
        Me.cmbRange.Location = New System.Drawing.Point(124, 171)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(130, 24)
        Me.cmbRange.TabIndex = 3
        '
        'txtBias
        '
        Me.txtBias.Enabled = False
        Me.txtBias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBias.Location = New System.Drawing.Point(333, 171)
        Me.txtBias.Name = "txtBias"
        Me.txtBias.Size = New System.Drawing.Size(130, 22)
        Me.txtBias.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(276, 171)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 16)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Bias (V):"
        '
        'txtNPLC
        '
        Me.txtNPLC.Enabled = False
        Me.txtNPLC.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNPLC.Location = New System.Drawing.Point(333, 214)
        Me.txtNPLC.Name = "txtNPLC"
        Me.txtNPLC.Size = New System.Drawing.Size(130, 22)
        Me.txtNPLC.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(276, 215)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "NPLC:"
        '
        'txtInterval
        '
        Me.txtInterval.Enabled = False
        Me.txtInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterval.Location = New System.Drawing.Point(124, 214)
        Me.txtInterval.Name = "txtInterval"
        Me.txtInterval.Size = New System.Drawing.Size(130, 22)
        Me.txtInterval.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(14, 214)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 16)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Sample Rate (s):"
        '
        'cmbFilterType
        '
        Me.cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFilterType.Enabled = False
        Me.cmbFilterType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFilterType.FormattingEnabled = True
        Me.cmbFilterType.Items.AddRange(New Object() {"FILTER_MEDIAN", "FILTER_MOVING_AVG", "FILTER_REPEAT_AVG"})
        Me.cmbFilterType.Location = New System.Drawing.Point(333, 253)
        Me.cmbFilterType.Name = "cmbFilterType"
        Me.cmbFilterType.Size = New System.Drawing.Size(130, 24)
        Me.cmbFilterType.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(276, 258)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 16)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Filter:"
        '
        'txtSamples
        '
        Me.txtSamples.Enabled = False
        Me.txtSamples.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSamples.Location = New System.Drawing.Point(124, 255)
        Me.txtSamples.Name = "txtSamples"
        Me.txtSamples.Size = New System.Drawing.Size(130, 22)
        Me.txtSamples.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(14, 258)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 16)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Sample Count:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(84, 109)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(122, 16)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Data File Directory:"
        '
        'btnSelDataDir
        '
        Me.btnSelDataDir.Enabled = False
        Me.btnSelDataDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelDataDir.Location = New System.Drawing.Point(409, 103)
        Me.btnSelDataDir.Name = "btnSelDataDir"
        Me.btnSelDataDir.Size = New System.Drawing.Size(23, 22)
        Me.btnSelDataDir.TabIndex = 10
        Me.btnSelDataDir.Text = "..."
        Me.btnSelDataDir.UseVisualStyleBackColor = True
        '
        'txtDataDir
        '
        Me.txtDataDir.Enabled = False
        Me.txtDataDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataDir.Location = New System.Drawing.Point(207, 103)
        Me.txtDataDir.Name = "txtDataDir"
        Me.txtDataDir.Size = New System.Drawing.Size(192, 22)
        Me.txtDataDir.TabIndex = 9
        '
        'txtSettlingTime
        '
        Me.txtSettlingTime.Enabled = False
        Me.txtSettlingTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSettlingTime.Location = New System.Drawing.Point(322, 309)
        Me.txtSettlingTime.Name = "txtSettlingTime"
        Me.txtSettlingTime.Size = New System.Drawing.Size(62, 22)
        Me.txtSettlingTime.TabIndex = 13
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(160, 309)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(159, 16)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Switch Settling Time (ms):"
        '
        'txtRow3Resistor
        '
        Me.txtRow3Resistor.Enabled = False
        Me.txtRow3Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow3Resistor.Location = New System.Drawing.Point(175, 351)
        Me.txtRow3Resistor.Name = "txtRow3Resistor"
        Me.txtRow3Resistor.Size = New System.Drawing.Size(62, 22)
        Me.txtRow3Resistor.TabIndex = 14
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(92, 351)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(78, 16)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "R3 (MOhm):"
        '
        'txtRow4Resistor
        '
        Me.txtRow4Resistor.Enabled = False
        Me.txtRow4Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow4Resistor.Location = New System.Drawing.Point(401, 346)
        Me.txtRow4Resistor.Name = "txtRow4Resistor"
        Me.txtRow4Resistor.Size = New System.Drawing.Size(62, 22)
        Me.txtRow4Resistor.TabIndex = 15
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(319, 346)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(78, 16)
        Me.Label14.TabIndex = 30
        Me.Label14.Text = "R4 (MOhm):"
        '
        'txtRow5Resistor
        '
        Me.txtRow5Resistor.Enabled = False
        Me.txtRow5Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow5Resistor.Location = New System.Drawing.Point(175, 388)
        Me.txtRow5Resistor.Name = "txtRow5Resistor"
        Me.txtRow5Resistor.Size = New System.Drawing.Size(62, 22)
        Me.txtRow5Resistor.TabIndex = 16
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(92, 388)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(78, 16)
        Me.Label15.TabIndex = 32
        Me.Label15.Text = "R5 (MOhm):"
        '
        'txtTolerance
        '
        Me.txtTolerance.Enabled = False
        Me.txtTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTolerance.Location = New System.Drawing.Point(401, 427)
        Me.txtTolerance.Name = "txtTolerance"
        Me.txtTolerance.Size = New System.Drawing.Size(62, 22)
        Me.txtTolerance.TabIndex = 18
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(251, 429)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(152, 16)
        Me.Label16.TabIndex = 34
        Me.Label16.Text = "Self Test Tolerance (%):"
        '
        'txtRow6Resistor
        '
        Me.txtRow6Resistor.Enabled = False
        Me.txtRow6Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow6Resistor.Location = New System.Drawing.Point(401, 386)
        Me.txtRow6Resistor.Name = "txtRow6Resistor"
        Me.txtRow6Resistor.Size = New System.Drawing.Size(62, 22)
        Me.txtRow6Resistor.TabIndex = 17
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(319, 388)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(78, 16)
        Me.Label18.TabIndex = 39
        Me.Label18.Text = "R6 (MOhm):"
        '
        'txtAuditZero
        '
        Me.txtAuditZero.Enabled = False
        Me.txtAuditZero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAuditZero.Location = New System.Drawing.Point(175, 426)
        Me.txtAuditZero.Name = "txtAuditZero"
        Me.txtAuditZero.Size = New System.Drawing.Size(62, 22)
        Me.txtAuditZero.TabIndex = 19
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(28, 429)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(132, 16)
        Me.Label19.TabIndex = 41
        Me.Label19.Text = "Self Test ""Zero"" (nA):"
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape3, Me.LineShape2, Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(502, 524)
        Me.ShapeContainer1.TabIndex = 42
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape3
        '
        Me.LineShape3.Name = "LineShape3"
        Me.LineShape3.X1 = 36
        Me.LineShape3.X2 = 618
        Me.LineShape3.Y1 = 366
        Me.LineShape3.Y2 = 366
        '
        'LineShape2
        '
        Me.LineShape2.Name = "LineShape2"
        Me.LineShape2.X1 = 33
        Me.LineShape2.X2 = 615
        Me.LineShape2.Y1 = 182
        Me.LineShape2.Y2 = 182
        '
        'LineShape1
        '
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.X1 = 30
        Me.LineShape1.X2 = 612
        Me.LineShape1.Y1 = 109
        Me.LineShape1.Y2 = 109
        '
        'frmConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(519, 370)
        Me.Controls.Add(Me.txtAuditZero)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.txtRow6Resistor)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtTolerance)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtRow5Resistor)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.txtRow4Resistor)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.txtRow3Resistor)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtSettlingTime)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.btnSelDataDir)
        Me.Controls.Add(Me.txtDataDir)
        Me.Controls.Add(Me.txtSamples)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmbFilterType)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtInterval)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtNPLC)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtBias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbRange)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.cmbCardConfig)
        Me.Controls.Add(Me.txtAddress)
        Me.Controls.Add(Me.txtSTAID)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.Name = "frmConfig"
        Me.Text = "Configuration Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSTAID As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents cmbCardConfig As System.Windows.Forms.ComboBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents txtBias As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNPLC As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtInterval As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbFilterType As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtSamples As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnSelDataDir As System.Windows.Forms.Button
    Friend WithEvents txtDataDir As System.Windows.Forms.TextBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents txtSettlingTime As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtRow3Resistor As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtRow4Resistor As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtRow5Resistor As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtTolerance As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtRow6Resistor As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtAuditZero As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents LineShape3 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents LineShape2 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape

End Class
