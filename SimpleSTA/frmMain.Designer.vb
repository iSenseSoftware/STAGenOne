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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewTestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenTestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartTestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopTestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoteInjectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.TestToolStripMenuItem, Me.SettingsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(784, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewTestToolStripMenuItem, Me.OpenTestToolStripMenuItem, Me.ExitToolStripMenuItem, Me.TestToolStripMenuItem1})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewTestToolStripMenuItem
        '
        Me.NewTestToolStripMenuItem.Name = "NewTestToolStripMenuItem"
        Me.NewTestToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewTestToolStripMenuItem.Text = "New Test..."
        '
        'OpenTestToolStripMenuItem
        '
        Me.OpenTestToolStripMenuItem.Name = "OpenTestToolStripMenuItem"
        Me.OpenTestToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OpenTestToolStripMenuItem.Text = "Open Test..."
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartTestToolStripMenuItem, Me.StopTestToolStripMenuItem, Me.NoteInjectionToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.TestToolStripMenuItem.Text = "Test"
        '
        'StartTestToolStripMenuItem
        '
        Me.StartTestToolStripMenuItem.Name = "StartTestToolStripMenuItem"
        Me.StartTestToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.StartTestToolStripMenuItem.Text = "Start Test"
        '
        'StopTestToolStripMenuItem
        '
        Me.StopTestToolStripMenuItem.Name = "StopTestToolStripMenuItem"
        Me.StopTestToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.StopTestToolStripMenuItem.Text = "Stop Test"
        '
        'NoteInjectionToolStripMenuItem
        '
        Me.NoteInjectionToolStripMenuItem.Name = "NoteInjectionToolStripMenuItem"
        Me.NoteInjectionToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.NoteInjectionToolStripMenuItem.Text = "Note Injection"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditConfigurationToolStripMenuItem})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'EditConfigurationToolStripMenuItem
        '
        Me.EditConfigurationToolStripMenuItem.Name = "EditConfigurationToolStripMenuItem"
        Me.EditConfigurationToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.EditConfigurationToolStripMenuItem.Text = "Edit Configuration"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'TestToolStripMenuItem1
        '
        Me.TestToolStripMenuItem1.Name = "TestToolStripMenuItem1"
        Me.TestToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.TestToolStripMenuItem1.Text = "Test"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "CGM Sensor Release Tester"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewTestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenTestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartTestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StopTestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoteInjectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
End Class
