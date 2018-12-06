<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class httpdconf
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(httpdconf))
        Me.GBMain = New System.Windows.Forms.GroupBox()
        Me.rtxtHttpdconf = New System.Windows.Forms.RichTextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ReloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PullFromSSHServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadFromProgramConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportFromLocalFilehttpdconfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PushToSSHServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToLocalFilehttpdconfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GBMain.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GBMain
        '
        Me.GBMain.Controls.Add(Me.rtxtHttpdconf)
        Me.GBMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GBMain.ForeColor = System.Drawing.Color.LimeGreen
        Me.GBMain.Location = New System.Drawing.Point(0, 24)
        Me.GBMain.Name = "GBMain"
        Me.GBMain.Size = New System.Drawing.Size(498, 439)
        Me.GBMain.TabIndex = 1
        Me.GBMain.TabStop = False
        Me.GBMain.Text = "httpd.conf"
        '
        'rtxtHttpdconf
        '
        Me.rtxtHttpdconf.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtxtHttpdconf.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.rtxtHttpdconf.ForeColor = System.Drawing.Color.LimeGreen
        Me.rtxtHttpdconf.Location = New System.Drawing.Point(3, 16)
        Me.rtxtHttpdconf.Name = "rtxtHttpdconf"
        Me.rtxtHttpdconf.Size = New System.Drawing.Size(492, 420)
        Me.rtxtHttpdconf.TabIndex = 0
        Me.rtxtHttpdconf.Text = ""
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReloadToolStripMenuItem, Me.SaveToolStripMenuItem, Me.CommitToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(498, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ReloadToolStripMenuItem
        '
        Me.ReloadToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PullFromSSHServerToolStripMenuItem, Me.LoadFromProgramConfigToolStripMenuItem, Me.ImportFromLocalFilehttpdconfToolStripMenuItem})
        Me.ReloadToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.logs
        Me.ReloadToolStripMenuItem.Name = "ReloadToolStripMenuItem"
        Me.ReloadToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.ReloadToolStripMenuItem.Text = "&Load"
        '
        'PullFromSSHServerToolStripMenuItem
        '
        Me.PullFromSSHServerToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.IncomingData48
        Me.PullFromSSHServerToolStripMenuItem.Name = "PullFromSSHServerToolStripMenuItem"
        Me.PullFromSSHServerToolStripMenuItem.Size = New System.Drawing.Size(253, 22)
        Me.PullFromSSHServerToolStripMenuItem.Text = "Pull from SSH server"
        '
        'LoadFromProgramConfigToolStripMenuItem
        '
        Me.LoadFromProgramConfigToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.logs
        Me.LoadFromProgramConfigToolStripMenuItem.Name = "LoadFromProgramConfigToolStripMenuItem"
        Me.LoadFromProgramConfigToolStripMenuItem.Size = New System.Drawing.Size(253, 22)
        Me.LoadFromProgramConfigToolStripMenuItem.Text = "Load from program config"
        '
        'ImportFromLocalFilehttpdconfToolStripMenuItem
        '
        Me.ImportFromLocalFilehttpdconfToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.Copy48
        Me.ImportFromLocalFilehttpdconfToolStripMenuItem.Name = "ImportFromLocalFilehttpdconfToolStripMenuItem"
        Me.ImportFromLocalFilehttpdconfToolStripMenuItem.Size = New System.Drawing.Size(253, 22)
        Me.ImportFromLocalFilehttpdconfToolStripMenuItem.Text = "Import from local file (httpd.conf)"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PushToSSHServerToolStripMenuItem, Me.ExportToLocalFilehttpdconfToolStripMenuItem})
        Me.SaveToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.EditFile48
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.SaveToolStripMenuItem.Text = "&Save"
        '
        'PushToSSHServerToolStripMenuItem
        '
        Me.PushToSSHServerToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.OutgoingData48
        Me.PushToSSHServerToolStripMenuItem.Name = "PushToSSHServerToolStripMenuItem"
        Me.PushToSSHServerToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.PushToSSHServerToolStripMenuItem.Text = "Push to SSH server and Commit"
        '
        'ExportToLocalFilehttpdconfToolStripMenuItem
        '
        Me.ExportToLocalFilehttpdconfToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.AddList48
        Me.ExportToLocalFilehttpdconfToolStripMenuItem.Name = "ExportToLocalFilehttpdconfToolStripMenuItem"
        Me.ExportToLocalFilehttpdconfToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.ExportToLocalFilehttpdconfToolStripMenuItem.Text = "Export to local file (httpd.conf)"
        '
        'CommitToolStripMenuItem
        '
        Me.CommitToolStripMenuItem.Image = Global.SysNetAdmin_Project.My.Resources.Resources.OutgoingData48
        Me.CommitToolStripMenuItem.Name = "CommitToolStripMenuItem"
        Me.CommitToolStripMenuItem.Size = New System.Drawing.Size(79, 20)
        Me.CommitToolStripMenuItem.Text = "&Commit"
        '
        'httpdconf
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(498, 463)
        Me.Controls.Add(Me.GBMain)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "httpdconf"
        Me.Text = "httpd.conf"
        Me.GBMain.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GBMain As GroupBox
    Friend WithEvents rtxtHttpdconf As RichTextBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ReloadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PullFromSSHServerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadFromProgramConfigToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportFromLocalFilehttpdconfToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PushToSSHServerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportToLocalFilehttpdconfToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CommitToolStripMenuItem As ToolStripMenuItem
End Class
