<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Wizard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.GBWizard_top = New System.Windows.Forms.GroupBox()
        Me.PanelRight = New System.Windows.Forms.Panel()
        Me.PBMain = New System.Windows.Forms.PictureBox()
        Me.lblMain = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCommit = New System.Windows.Forms.Button()
        Me.Panel_Controls = New System.Windows.Forms.Panel()
        Me.FLPControls = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnYes = New System.Windows.Forms.Button()
        Me.btnNo = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rtxtCmdOut = New System.Windows.Forms.RichTextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.FLPOutput = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblCmdOut = New System.Windows.Forms.Label()
        Me.PanelBottom = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.GBWizard_options = New System.Windows.Forms.GroupBox()
        Me.btnLiveWeb = New System.Windows.Forms.Button()
        Me.GBWizard_top.SuspendLayout()
        Me.PanelRight.SuspendLayout()
        CType(Me.PBMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel_Controls.SuspendLayout()
        Me.FLPControls.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.FLPOutput.SuspendLayout()
        Me.PanelBottom.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.GBWizard_options.SuspendLayout()
        Me.SuspendLayout()
        '
        'GBWizard_top
        '
        Me.GBWizard_top.Controls.Add(Me.PanelRight)
        Me.GBWizard_top.Controls.Add(Me.lblMain)
        Me.GBWizard_top.Dock = System.Windows.Forms.DockStyle.Top
        Me.GBWizard_top.Location = New System.Drawing.Point(0, 0)
        Me.GBWizard_top.Name = "GBWizard_top"
        Me.GBWizard_top.Size = New System.Drawing.Size(556, 108)
        Me.GBWizard_top.TabIndex = 0
        Me.GBWizard_top.TabStop = False
        '
        'PanelRight
        '
        Me.PanelRight.Controls.Add(Me.PBMain)
        Me.PanelRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelRight.Location = New System.Drawing.Point(505, 16)
        Me.PanelRight.Name = "PanelRight"
        Me.PanelRight.Size = New System.Drawing.Size(48, 89)
        Me.PanelRight.TabIndex = 2
        '
        'PBMain
        '
        Me.PBMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.PBMain.Image = Global.SysNetAdmin_Project.My.Resources.Resources.Wizard48
        Me.PBMain.Location = New System.Drawing.Point(0, 0)
        Me.PBMain.Name = "PBMain"
        Me.PBMain.Size = New System.Drawing.Size(48, 49)
        Me.PBMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PBMain.TabIndex = 1
        Me.PBMain.TabStop = False
        '
        'lblMain
        '
        Me.lblMain.AutoSize = True
        Me.lblMain.Font = New System.Drawing.Font("Rockwell", 10.0!)
        Me.lblMain.ForeColor = System.Drawing.Color.LimeGreen
        Me.lblMain.Location = New System.Drawing.Point(3, 9)
        Me.lblMain.Name = "lblMain"
        Me.lblMain.Size = New System.Drawing.Size(485, 85)
        Me.lblMain.TabIndex = 0
        Me.lblMain.Text = "The wizard will guide you through the basic installation and configuration " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "for " &
    "your Apache webserver." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please click Next to begin the wizard."
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCommit)
        Me.Panel1.Controls.Add(Me.Panel_Controls)
        Me.Panel1.Controls.Add(Me.btnLiveWeb)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(550, 30)
        Me.Panel1.TabIndex = 0
        '
        'btnCommit
        '
        Me.btnCommit.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.btnCommit.ForeColor = System.Drawing.Color.LimeGreen
        Me.btnCommit.Location = New System.Drawing.Point(373, 3)
        Me.btnCommit.Name = "btnCommit"
        Me.btnCommit.Size = New System.Drawing.Size(75, 23)
        Me.btnCommit.TabIndex = 1
        Me.btnCommit.Text = "&Commit"
        Me.btnCommit.UseVisualStyleBackColor = True
        Me.btnCommit.Visible = False
        '
        'Panel_Controls
        '
        Me.Panel_Controls.Controls.Add(Me.FLPControls)
        Me.Panel_Controls.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel_Controls.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Controls.Name = "Panel_Controls"
        Me.Panel_Controls.Size = New System.Drawing.Size(318, 30)
        Me.Panel_Controls.TabIndex = 0
        '
        'FLPControls
        '
        Me.FLPControls.Controls.Add(Me.btnYes)
        Me.FLPControls.Controls.Add(Me.btnNo)
        Me.FLPControls.Controls.Add(Me.txtPassword)
        Me.FLPControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FLPControls.Location = New System.Drawing.Point(0, 0)
        Me.FLPControls.Name = "FLPControls"
        Me.FLPControls.Size = New System.Drawing.Size(318, 30)
        Me.FLPControls.TabIndex = 0
        '
        'btnYes
        '
        Me.btnYes.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.btnYes.ForeColor = System.Drawing.Color.LimeGreen
        Me.btnYes.Location = New System.Drawing.Point(3, 3)
        Me.btnYes.Name = "btnYes"
        Me.btnYes.Size = New System.Drawing.Size(75, 23)
        Me.btnYes.TabIndex = 0
        Me.btnYes.Text = "&Yes"
        Me.btnYes.UseVisualStyleBackColor = True
        '
        'btnNo
        '
        Me.btnNo.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.btnNo.ForeColor = System.Drawing.Color.LimeGreen
        Me.btnNo.Location = New System.Drawing.Point(84, 3)
        Me.btnNo.Name = "btnNo"
        Me.btnNo.Size = New System.Drawing.Size(75, 23)
        Me.btnNo.TabIndex = 1
        Me.btnNo.Text = "&No"
        Me.btnNo.UseVisualStyleBackColor = True
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Rockwell", 10.0!)
        Me.txtPassword.ForeColor = System.Drawing.Color.LimeGreen
        Me.txtPassword.Location = New System.Drawing.Point(165, 3)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(150, 23)
        Me.txtPassword.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rtxtCmdOut)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 46)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(550, 101)
        Me.Panel2.TabIndex = 1
        '
        'rtxtCmdOut
        '
        Me.rtxtCmdOut.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtxtCmdOut.Location = New System.Drawing.Point(0, 0)
        Me.rtxtCmdOut.Name = "rtxtCmdOut"
        Me.rtxtCmdOut.Size = New System.Drawing.Size(550, 101)
        Me.rtxtCmdOut.TabIndex = 0
        Me.rtxtCmdOut.Text = ""
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.FLPOutput)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(3, 147)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(550, 37)
        Me.Panel3.TabIndex = 2
        '
        'FLPOutput
        '
        Me.FLPOutput.Controls.Add(Me.lblCmdOut)
        Me.FLPOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FLPOutput.Location = New System.Drawing.Point(0, 0)
        Me.FLPOutput.Name = "FLPOutput"
        Me.FLPOutput.Size = New System.Drawing.Size(550, 37)
        Me.FLPOutput.TabIndex = 0
        '
        'lblCmdOut
        '
        Me.lblCmdOut.AutoSize = True
        Me.lblCmdOut.Font = New System.Drawing.Font("Rockwell", 10.0!)
        Me.lblCmdOut.ForeColor = System.Drawing.Color.LimeGreen
        Me.lblCmdOut.Location = New System.Drawing.Point(3, 0)
        Me.lblCmdOut.Name = "lblCmdOut"
        Me.lblCmdOut.Size = New System.Drawing.Size(80, 17)
        Me.lblCmdOut.TabIndex = 1
        Me.lblCmdOut.Text = "lblCmdOut"
        Me.lblCmdOut.Visible = False
        '
        'PanelBottom
        '
        Me.PanelBottom.Controls.Add(Me.FlowLayoutPanel1)
        Me.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelBottom.Location = New System.Drawing.Point(0, 295)
        Me.PanelBottom.Name = "PanelBottom"
        Me.PanelBottom.Size = New System.Drawing.Size(556, 30)
        Me.PanelBottom.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNext)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnBack)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(556, 30)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'btnNext
        '
        Me.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnNext.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.btnNext.ForeColor = System.Drawing.Color.LimeGreen
        Me.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNext.Location = New System.Drawing.Point(476, 3)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(77, 23)
        Me.btnNext.TabIndex = 0
        Me.btnNext.Text = "&Next"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.btnBack.ForeColor = System.Drawing.Color.LimeGreen
        Me.btnBack.Location = New System.Drawing.Point(395, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(75, 23)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'GBWizard_options
        '
        Me.GBWizard_options.Controls.Add(Me.Panel2)
        Me.GBWizard_options.Controls.Add(Me.Panel1)
        Me.GBWizard_options.Controls.Add(Me.Panel3)
        Me.GBWizard_options.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GBWizard_options.Location = New System.Drawing.Point(0, 108)
        Me.GBWizard_options.Name = "GBWizard_options"
        Me.GBWizard_options.Size = New System.Drawing.Size(556, 187)
        Me.GBWizard_options.TabIndex = 4
        Me.GBWizard_options.TabStop = False
        '
        'btnLiveWeb
        '
        Me.btnLiveWeb.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.btnLiveWeb.ForeColor = System.Drawing.Color.LimeGreen
        Me.btnLiveWeb.Location = New System.Drawing.Point(454, 4)
        Me.btnLiveWeb.Name = "btnLiveWeb"
        Me.btnLiveWeb.Size = New System.Drawing.Size(93, 23)
        Me.btnLiveWeb.TabIndex = 2
        Me.btnLiveWeb.Text = "&Live Website"
        Me.btnLiveWeb.UseVisualStyleBackColor = True
        Me.btnLiveWeb.Visible = False
        '
        'Wizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 325)
        Me.Controls.Add(Me.GBWizard_options)
        Me.Controls.Add(Me.GBWizard_top)
        Me.Controls.Add(Me.PanelBottom)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Wizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Wizard"
        Me.GBWizard_top.ResumeLayout(False)
        Me.GBWizard_top.PerformLayout()
        Me.PanelRight.ResumeLayout(False)
        CType(Me.PBMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel_Controls.ResumeLayout(False)
        Me.FLPControls.ResumeLayout(False)
        Me.FLPControls.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.FLPOutput.ResumeLayout(False)
        Me.FLPOutput.PerformLayout()
        Me.PanelBottom.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.GBWizard_options.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GBWizard_top As GroupBox
    Friend WithEvents PanelBottom As Panel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnNext As Button
    Friend WithEvents btnBack As Button
    Friend WithEvents PBMain As PictureBox
    Friend WithEvents lblMain As Label
    Friend WithEvents PanelRight As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel_Controls As Panel
    Friend WithEvents FLPControls As FlowLayoutPanel
    Friend WithEvents btnYes As Button
    Friend WithEvents btnNo As Button
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents FLPOutput As FlowLayoutPanel
    Friend WithEvents lblCmdOut As Label
    Friend WithEvents rtxtCmdOut As RichTextBox
    Friend WithEvents GBWizard_options As GroupBox
    Friend WithEvents btnCommit As Button
    Friend WithEvents btnLiveWeb As Button
End Class
