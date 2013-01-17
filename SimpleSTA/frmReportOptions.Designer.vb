<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportOptions
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
        Me.btnSelectGlucoseFile = New System.Windows.Forms.Button()
        Me.txtGlucoseTestFile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSelectAPAPFile = New System.Windows.Forms.Button()
        Me.txtAPAPTestFile = New System.Windows.Forms.TextBox()
        Me.btnCreateReport = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'btnSelectGlucoseFile
        '
        Me.btnSelectGlucoseFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectGlucoseFile.Location = New System.Drawing.Point(429, 44)
        Me.btnSelectGlucoseFile.Name = "btnSelectGlucoseFile"
        Me.btnSelectGlucoseFile.Size = New System.Drawing.Size(26, 22)
        Me.btnSelectGlucoseFile.TabIndex = 3
        Me.btnSelectGlucoseFile.Text = "..."
        Me.btnSelectGlucoseFile.UseVisualStyleBackColor = True
        '
        'txtGlucoseTestFile
        '
        Me.txtGlucoseTestFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGlucoseTestFile.Location = New System.Drawing.Point(179, 44)
        Me.txtGlucoseTestFile.Name = "txtGlucoseTestFile"
        Me.txtGlucoseTestFile.Size = New System.Drawing.Size(244, 22)
        Me.txtGlucoseTestFile.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(26, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(147, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Select Glucose File:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(43, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Select APAP File:"
        '
        'btnSelectAPAPFile
        '
        Me.btnSelectAPAPFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectAPAPFile.Location = New System.Drawing.Point(429, 93)
        Me.btnSelectAPAPFile.Name = "btnSelectAPAPFile"
        Me.btnSelectAPAPFile.Size = New System.Drawing.Size(26, 22)
        Me.btnSelectAPAPFile.TabIndex = 6
        Me.btnSelectAPAPFile.Text = "..."
        Me.btnSelectAPAPFile.UseVisualStyleBackColor = True
        '
        'txtAPAPTestFile
        '
        Me.txtAPAPTestFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAPAPTestFile.Location = New System.Drawing.Point(179, 93)
        Me.txtAPAPTestFile.Name = "txtAPAPTestFile"
        Me.txtAPAPTestFile.Size = New System.Drawing.Size(244, 22)
        Me.txtAPAPTestFile.TabIndex = 5
        '
        'btnCreateReport
        '
        Me.btnCreateReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateReport.Location = New System.Drawing.Point(179, 160)
        Me.btnCreateReport.Name = "btnCreateReport"
        Me.btnCreateReport.Size = New System.Drawing.Size(121, 30)
        Me.btnCreateReport.TabIndex = 8
        Me.btnCreateReport.Text = "Create Report"
        Me.btnCreateReport.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.FileName = "OpenFileDialog2"
        '
        'frmReportOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 228)
        Me.Controls.Add(Me.btnCreateReport)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnSelectAPAPFile)
        Me.Controls.Add(Me.txtAPAPTestFile)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSelectGlucoseFile)
        Me.Controls.Add(Me.txtGlucoseTestFile)
        Me.Name = "frmReportOptions"
        Me.Text = "Create Report"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSelectGlucoseFile As System.Windows.Forms.Button
    Friend WithEvents txtGlucoseTestFile As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSelectAPAPFile As System.Windows.Forms.Button
    Friend WithEvents txtAPAPTestFile As System.Windows.Forms.TextBox
    Friend WithEvents btnCreateReport As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
End Class
