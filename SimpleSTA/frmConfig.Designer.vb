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
        Me.Label1 = New System.Windows.Forms.Label()
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
        Me.btnSelectFile = New System.Windows.Forms.Button()
        Me.txtDumpDir = New System.Windows.Forms.TextBox()
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
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnSelectInfoFile = New System.Windows.Forms.Button()
        Me.txtSystemInfoFile = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(57, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(317, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Define Test and Instrument Settings Below:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(136, 143)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Card Configuration"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(200, 101)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "STA ID:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(58, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(196, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "3706A Logical Name / Address:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(157, 186)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 16)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Current Range:"
        '
        'txtSTAID
        '
        Me.txtSTAID.Enabled = False
        Me.txtSTAID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSTAID.Location = New System.Drawing.Point(260, 98)
        Me.txtSTAID.Name = "txtSTAID"
        Me.txtSTAID.Size = New System.Drawing.Size(151, 22)
        Me.txtSTAID.TabIndex = 7
        '
        'txtAddress
        '
        Me.txtAddress.Enabled = False
        Me.txtAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress.Location = New System.Drawing.Point(260, 57)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(151, 22)
        Me.txtAddress.TabIndex = 6
        '
        'cmbCardConfig
        '
        Me.cmbCardConfig.Enabled = False
        Me.cmbCardConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCardConfig.FormattingEnabled = True
        Me.cmbCardConfig.Items.AddRange(New Object() {"1 Card  (16 Sensors)", "2 Cards (32 Sensors)", "3 Cards (48 Sensors)", "4 Cards (64 Sensors)", "5 Cards (80 Sensors)", "6 Cards (96 Sensors)"})
        Me.cmbCardConfig.Location = New System.Drawing.Point(260, 140)
        Me.cmbCardConfig.Name = "cmbCardConfig"
        Me.cmbCardConfig.Size = New System.Drawing.Size(151, 24)
        Me.cmbCardConfig.TabIndex = 8
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(89, 715)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(117, 35)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Edit"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(226, 715)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(117, 35)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.Enabled = False
        Me.cmbRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Items.AddRange(New Object() {"Average", "Median", "Repeat"})
        Me.cmbRange.Location = New System.Drawing.Point(260, 183)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(151, 24)
        Me.cmbRange.TabIndex = 2
        '
        'txtBias
        '
        Me.txtBias.Enabled = False
        Me.txtBias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBias.Location = New System.Drawing.Point(260, 224)
        Me.txtBias.Name = "txtBias"
        Me.txtBias.Size = New System.Drawing.Size(151, 22)
        Me.txtBias.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(200, 227)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 16)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Bias (V):"
        '
        'txtNPLC
        '
        Me.txtNPLC.Enabled = False
        Me.txtNPLC.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNPLC.Location = New System.Drawing.Point(260, 262)
        Me.txtNPLC.Name = "txtNPLC"
        Me.txtNPLC.Size = New System.Drawing.Size(151, 22)
        Me.txtNPLC.TabIndex = 14
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(200, 265)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "NPLC:"
        '
        'txtInterval
        '
        Me.txtInterval.Enabled = False
        Me.txtInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterval.Location = New System.Drawing.Point(260, 300)
        Me.txtInterval.Name = "txtInterval"
        Me.txtInterval.Size = New System.Drawing.Size(151, 22)
        Me.txtInterval.TabIndex = 16
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(132, 303)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(122, 16)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Sample Interval (s):"
        '
        'cmbFilterType
        '
        Me.cmbFilterType.Enabled = False
        Me.cmbFilterType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFilterType.FormattingEnabled = True
        Me.cmbFilterType.Items.AddRange(New Object() {"Average", "Median", "Repeat"})
        Me.cmbFilterType.Location = New System.Drawing.Point(260, 338)
        Me.cmbFilterType.Name = "cmbFilterType"
        Me.cmbFilterType.Size = New System.Drawing.Size(151, 24)
        Me.cmbFilterType.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(171, 341)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 16)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Filter Type:"
        '
        'txtSamples
        '
        Me.txtSamples.Enabled = False
        Me.txtSamples.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSamples.Location = New System.Drawing.Point(260, 379)
        Me.txtSamples.Name = "txtSamples"
        Me.txtSamples.Size = New System.Drawing.Size(151, 22)
        Me.txtSamples.TabIndex = 22
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(136, 385)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 16)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Sample Count:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(67, 421)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(122, 16)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Data File Directory:"
        '
        'btnSelectFile
        '
        Me.btnSelectFile.Enabled = False
        Me.btnSelectFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectFile.Location = New System.Drawing.Point(393, 418)
        Me.btnSelectFile.Name = "btnSelectFile"
        Me.btnSelectFile.Size = New System.Drawing.Size(23, 22)
        Me.btnSelectFile.TabIndex = 25
        Me.btnSelectFile.Text = "..."
        Me.btnSelectFile.UseVisualStyleBackColor = True
        '
        'txtDumpDir
        '
        Me.txtDumpDir.Enabled = False
        Me.txtDumpDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDumpDir.Location = New System.Drawing.Point(195, 418)
        Me.txtDumpDir.Name = "txtDumpDir"
        Me.txtDumpDir.Size = New System.Drawing.Size(192, 22)
        Me.txtDumpDir.TabIndex = 23
        '
        'txtSettlingTime
        '
        Me.txtSettlingTime.Enabled = False
        Me.txtSettlingTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSettlingTime.Location = New System.Drawing.Point(260, 509)
        Me.txtSettlingTime.Name = "txtSettlingTime"
        Me.txtSettlingTime.Size = New System.Drawing.Size(151, 22)
        Me.txtSettlingTime.TabIndex = 27
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(95, 512)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(159, 16)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Switch Settling Time (ms):"
        '
        'txtRow3Resistor
        '
        Me.txtRow3Resistor.Enabled = False
        Me.txtRow3Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow3Resistor.Location = New System.Drawing.Point(260, 551)
        Me.txtRow3Resistor.Name = "txtRow3Resistor"
        Me.txtRow3Resistor.Size = New System.Drawing.Size(151, 22)
        Me.txtRow3Resistor.TabIndex = 29
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(12, 554)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(242, 16)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Row 3 Resistor Nominal Value (MOhm):"
        '
        'txtRow4Resistor
        '
        Me.txtRow4Resistor.Enabled = False
        Me.txtRow4Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow4Resistor.Location = New System.Drawing.Point(260, 589)
        Me.txtRow4Resistor.Name = "txtRow4Resistor"
        Me.txtRow4Resistor.Size = New System.Drawing.Size(151, 22)
        Me.txtRow4Resistor.TabIndex = 31
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(12, 592)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(242, 16)
        Me.Label14.TabIndex = 30
        Me.Label14.Text = "Row 4 Nominal Resistor Value (MOhm):"
        '
        'txtRow5Resistor
        '
        Me.txtRow5Resistor.Enabled = False
        Me.txtRow5Resistor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRow5Resistor.Location = New System.Drawing.Point(260, 627)
        Me.txtRow5Resistor.Name = "txtRow5Resistor"
        Me.txtRow5Resistor.Size = New System.Drawing.Size(151, 22)
        Me.txtRow5Resistor.TabIndex = 33
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(12, 630)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(242, 16)
        Me.Label15.TabIndex = 32
        Me.Label15.Text = "Row 5 Nominal Resistor Value (MOhm):"
        '
        'txtTolerance
        '
        Me.txtTolerance.Enabled = False
        Me.txtTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTolerance.Location = New System.Drawing.Point(260, 666)
        Me.txtTolerance.Name = "txtTolerance"
        Me.txtTolerance.Size = New System.Drawing.Size(151, 22)
        Me.txtTolerance.TabIndex = 35
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(102, 669)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(152, 16)
        Me.Label16.TabIndex = 34
        Me.Label16.Text = "Self Test Tolerance (%):"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(22, 462)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(167, 16)
        Me.Label17.TabIndex = 37
        Me.Label17.Text = "Test System Info Directory:"
        '
        'btnSelectInfoFile
        '
        Me.btnSelectInfoFile.Enabled = False
        Me.btnSelectInfoFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectInfoFile.Location = New System.Drawing.Point(393, 456)
        Me.btnSelectInfoFile.Name = "btnSelectInfoFile"
        Me.btnSelectInfoFile.Size = New System.Drawing.Size(23, 22)
        Me.btnSelectInfoFile.TabIndex = 38
        Me.btnSelectInfoFile.Text = "..."
        Me.btnSelectInfoFile.UseVisualStyleBackColor = True
        '
        'txtSystemInfoFile
        '
        Me.txtSystemInfoFile.Enabled = False
        Me.txtSystemInfoFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSystemInfoFile.Location = New System.Drawing.Point(195, 456)
        Me.txtSystemInfoFile.Name = "txtSystemInfoFile"
        Me.txtSystemInfoFile.Size = New System.Drawing.Size(192, 22)
        Me.txtSystemInfoFile.TabIndex = 36
        '
        'frmConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(438, 573)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.btnSelectInfoFile)
        Me.Controls.Add(Me.txtSystemInfoFile)
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
        Me.Controls.Add(Me.btnSelectFile)
        Me.Controls.Add(Me.txtDumpDir)
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
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmConfig"
        Me.Text = "Configuration Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
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
    Friend WithEvents btnSelectFile As System.Windows.Forms.Button
    Friend WithEvents txtDumpDir As System.Windows.Forms.TextBox
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
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents btnSelectInfoFile As System.Windows.Forms.Button
    Friend WithEvents txtSystemInfoFile As System.Windows.Forms.TextBox

End Class
