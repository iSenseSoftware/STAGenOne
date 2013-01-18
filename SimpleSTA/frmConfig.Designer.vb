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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtRecordInterval = New System.Windows.Forms.TextBox()
        Me.txtSTAID = New System.Windows.Forms.TextBox()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.txtNPLC = New System.Windows.Forms.TextBox()
        Me.txtSamples = New System.Windows.Forms.TextBox()
        Me.txtBias = New System.Windows.Forms.TextBox()
        Me.cmbFilterType = New System.Windows.Forms.ComboBox()
        Me.cmbCardConfig = New System.Windows.Forms.ComboBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cmbRange = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(317, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Define Test and Instrument Settings Below:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(29, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Record Interval (s):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(326, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Card Configuration"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(390, 95)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "STA ID:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(248, 54)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(196, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "3706A Logical Name / Address:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(103, 250)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 16)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "NPLC:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(84, 213)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 16)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Samples:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(74, 176)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 16)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Filter Type:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(52, 134)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 16)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Current Range:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(91, 95)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(58, 16)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Bias (V):"
        '
        'txtRecordInterval
        '
        Me.txtRecordInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRecordInterval.Location = New System.Drawing.Point(155, 51)
        Me.txtRecordInterval.Name = "txtRecordInterval"
        Me.txtRecordInterval.Size = New System.Drawing.Size(59, 22)
        Me.txtRecordInterval.TabIndex = 0
        '
        'txtSTAID
        '
        Me.txtSTAID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSTAID.Location = New System.Drawing.Point(450, 92)
        Me.txtSTAID.Name = "txtSTAID"
        Me.txtSTAID.Size = New System.Drawing.Size(151, 22)
        Me.txtSTAID.TabIndex = 7
        '
        'txtAddress
        '
        Me.txtAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress.Location = New System.Drawing.Point(450, 51)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(151, 22)
        Me.txtAddress.TabIndex = 6
        '
        'txtNPLC
        '
        Me.txtNPLC.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNPLC.Location = New System.Drawing.Point(155, 247)
        Me.txtNPLC.Name = "txtNPLC"
        Me.txtNPLC.Size = New System.Drawing.Size(59, 22)
        Me.txtNPLC.TabIndex = 5
        '
        'txtSamples
        '
        Me.txtSamples.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSamples.Location = New System.Drawing.Point(155, 210)
        Me.txtSamples.Name = "txtSamples"
        Me.txtSamples.Size = New System.Drawing.Size(59, 22)
        Me.txtSamples.TabIndex = 4
        '
        'txtBias
        '
        Me.txtBias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBias.Location = New System.Drawing.Point(155, 92)
        Me.txtBias.Name = "txtBias"
        Me.txtBias.Size = New System.Drawing.Size(59, 22)
        Me.txtBias.TabIndex = 1
        '
        'cmbFilterType
        '
        Me.cmbFilterType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFilterType.FormattingEnabled = True
        Me.cmbFilterType.Items.AddRange(New Object() {"Average", "Median", "Repeat"})
        Me.cmbFilterType.Location = New System.Drawing.Point(155, 173)
        Me.cmbFilterType.Name = "cmbFilterType"
        Me.cmbFilterType.Size = New System.Drawing.Size(125, 24)
        Me.cmbFilterType.TabIndex = 3
        '
        'cmbCardConfig
        '
        Me.cmbCardConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCardConfig.FormattingEnabled = True
        Me.cmbCardConfig.Items.AddRange(New Object() {"1 Card  (16 Sensors)", "2 Cards (32 Sensors)", "3 Cards (48 Sensors)", "4 Cards (64 Sensors)", "5 Cards (80 Sensors)", "6 Cards (96 Sensors)"})
        Me.cmbCardConfig.Location = New System.Drawing.Point(450, 134)
        Me.cmbCardConfig.Name = "cmbCardConfig"
        Me.cmbCardConfig.Size = New System.Drawing.Size(151, 24)
        Me.cmbCardConfig.TabIndex = 8
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(174, 303)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(117, 35)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(311, 303)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(117, 35)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Items.AddRange(New Object() {"Average", "Median", "Repeat"})
        Me.cmbRange.Location = New System.Drawing.Point(155, 131)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(125, 24)
        Me.cmbRange.TabIndex = 2
        '
        'frmConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(645, 360)
        Me.Controls.Add(Me.cmbRange)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.cmbCardConfig)
        Me.Controls.Add(Me.cmbFilterType)
        Me.Controls.Add(Me.txtBias)
        Me.Controls.Add(Me.txtSamples)
        Me.Controls.Add(Me.txtNPLC)
        Me.Controls.Add(Me.txtAddress)
        Me.Controls.Add(Me.txtSTAID)
        Me.Controls.Add(Me.txtRecordInterval)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmConfig"
        Me.Text = "Configuration Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtRecordInterval As System.Windows.Forms.TextBox
    Friend WithEvents txtSTAID As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtNPLC As System.Windows.Forms.TextBox
    Friend WithEvents txtSamples As System.Windows.Forms.TextBox
    Friend WithEvents txtBias As System.Windows.Forms.TextBox
    Friend WithEvents cmbFilterType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCardConfig As System.Windows.Forms.ComboBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox

End Class
