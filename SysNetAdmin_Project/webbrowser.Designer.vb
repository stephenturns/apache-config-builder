<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class webbrowser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(webbrowser))
        Me.GBTop = New System.Windows.Forms.GroupBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.txturl = New System.Windows.Forms.TextBox()
        Me.GBFill = New System.Windows.Forms.GroupBox()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.GBTop.SuspendLayout()
        Me.GBFill.SuspendLayout()
        Me.SuspendLayout()
        '
        'GBTop
        '
        Me.GBTop.Controls.Add(Me.btnGo)
        Me.GBTop.Controls.Add(Me.txturl)
        Me.GBTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.GBTop.Location = New System.Drawing.Point(0, 0)
        Me.GBTop.Name = "GBTop"
        Me.GBTop.Size = New System.Drawing.Size(399, 44)
        Me.GBTop.TabIndex = 0
        Me.GBTop.TabStop = False
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(349, 14)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(42, 23)
        Me.btnGo.TabIndex = 1
        Me.btnGo.Text = "&Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'txturl
        '
        Me.txturl.Dock = System.Windows.Forms.DockStyle.Left
        Me.txturl.Font = New System.Drawing.Font("Rockwell", 9.0!)
        Me.txturl.ForeColor = System.Drawing.Color.LimeGreen
        Me.txturl.Location = New System.Drawing.Point(3, 16)
        Me.txturl.Name = "txturl"
        Me.txturl.Size = New System.Drawing.Size(340, 22)
        Me.txturl.TabIndex = 0
        '
        'GBFill
        '
        Me.GBFill.Controls.Add(Me.WebBrowser1)
        Me.GBFill.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GBFill.Location = New System.Drawing.Point(0, 44)
        Me.GBFill.Name = "GBFill"
        Me.GBFill.Size = New System.Drawing.Size(399, 311)
        Me.GBFill.TabIndex = 1
        Me.GBFill.TabStop = False
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(3, 16)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(393, 292)
        Me.WebBrowser1.TabIndex = 0
        '
        'webbrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(399, 355)
        Me.Controls.Add(Me.GBFill)
        Me.Controls.Add(Me.GBTop)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "webbrowser"
        Me.Text = "Apache: Web Browser"
        Me.GBTop.ResumeLayout(False)
        Me.GBTop.PerformLayout()
        Me.GBFill.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GBTop As GroupBox
    Friend WithEvents btnGo As Button
    Friend WithEvents txturl As TextBox
    Friend WithEvents GBFill As GroupBox
    Friend WithEvents WebBrowser1 As Windows.Forms.WebBrowser
End Class
