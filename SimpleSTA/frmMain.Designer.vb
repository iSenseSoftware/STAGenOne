<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.btnNewTest = New System.Windows.Forms.Button()
        Me.btnConfig = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnNewTest
        '
        Me.btnNewTest.Location = New System.Drawing.Point(167, 50)
        Me.btnNewTest.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnNewTest.Name = "btnNewTest"
        Me.btnNewTest.Size = New System.Drawing.Size(223, 147)
        Me.btnNewTest.TabIndex = 7
        Me.btnNewTest.Text = "New Test"
        Me.btnNewTest.UseVisualStyleBackColor = True
        '
        'btnConfig
        '
        Me.btnConfig.Location = New System.Drawing.Point(167, 223)
        Me.btnConfig.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnConfig.Name = "btnConfig"
        Me.btnConfig.Size = New System.Drawing.Size(223, 147)
        Me.btnConfig.TabIndex = 8
        Me.btnConfig.Text = "Configuration"
        Me.btnConfig.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(21, 7)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(69, 13)
        Me.lblVersion.TabIndex = 9
        Me.lblVersion.Text = "Version 0.0.0"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(586, 451)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnConfig)
        Me.Controls.Add(Me.btnNewTest)
        Me.Name = "frmMain"
        Me.Text = "GlucoMatrix"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnNewTest As System.Windows.Forms.Button
    Friend WithEvents btnConfig As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label
End Class
