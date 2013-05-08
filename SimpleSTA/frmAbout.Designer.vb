<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Me.txtTitle = New System.Windows.Forms.Label()
        Me.txtVersion = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtTitle
        '
        Me.txtTitle.AutoSize = True
        Me.txtTitle.Location = New System.Drawing.Point(69, 21)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(39, 13)
        Me.txtTitle.TabIndex = 0
        Me.txtTitle.Text = "Label1"
        Me.txtTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtVersion
        '
        Me.txtVersion.AutoSize = True
        Me.txtVersion.Location = New System.Drawing.Point(69, 77)
        Me.txtVersion.Name = "txtVersion"
        Me.txtVersion.Size = New System.Drawing.Size(39, 13)
        Me.txtVersion.TabIndex = 1
        Me.txtVersion.Text = "Label2"
        Me.txtVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDescription
        '
        Me.txtDescription.AutoSize = True
        Me.txtDescription.Location = New System.Drawing.Point(69, 49)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(39, 13)
        Me.txtDescription.TabIndex = 2
        Me.txtDescription.Text = "Label1"
        Me.txtDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 100)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtVersion)
        Me.Controls.Add(Me.txtTitle)
        Me.Name = "frmAbout"
        Me.Text = "About"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTitle As System.Windows.Forms.Label
    Friend WithEvents txtVersion As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.Label
End Class
