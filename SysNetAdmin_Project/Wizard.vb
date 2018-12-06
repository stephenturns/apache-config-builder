
Public Class Wizard
#Region "Variable Declaration"
    Dim LR As Long 'Pass SetTopMostWindow() - This window is ontop
    Dim intPage As Integer 'Counter for wizard page number
#End Region

#Region "Form Handlers"
    Private Sub Wizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LR = SetTopMostWindow(Me.Handle, True) 'Set this window ontop NOW

        boolWizardActive = True 'The wizard is active now

        'Graphical page layout
        txtPassword.Visible = False
        btnYes.Visible = False
        btnNo.Visible = False
        lblCmdOut.Visible = False
        Panel2.Visible = False

        intPage = 1 'Page one of the wizard
        WizardPages(intPage) 'Do page 1 - graphical layout, options, etc.
    End Sub
    Private Sub Wizard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        boolWizardActive = False 'The wizard is not active
    End Sub
#End Region

#Region "User Event Handlers"
    'Step to the next page
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If btnNext.Text = "Close" Then
            Me.Close()
        Else
            'Step to the next page
            intPage += 1
            WizardPages(intPage)
        End If
    End Sub
    'Step back a page
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        'Step back a page
        intPage -= 1
        WizardPages(intPage)
    End Sub
    'User clicks YES/OK
    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click

        'Depending on what page - Do YES/OK

        Select Case intPage
            Case 2 ' Connect to server
                If txtPassword.Visible = True Then
                    main.txtPassword.Text = txtPassword.Text
                    txtPassword.Clear()
                    txtPassword.Visible = False
                    PrepareConnection()
                    btnYes.Text = "Yes"
                    btnYes.Visible = False
                    btnNo.Visible = False
                    LR = SetTopMostWindow(Me.Handle, True)
                Else
                    main.TabControl_Main.SelectedTab = main.TabServer
                    LR = SetTopMostWindow(Me.Handle, False)

                    Dim strResult As String
                    strResult = InputBox("Please enter the SSH IP address or Name: ", "SSH Server Connection: IP/Name", "")
                    If strResult = "" Then
                        MsgBox("We cannot continue without a SSH server address...", MsgBoxStyle.Exclamation, "SSH Server Connection: NO NAME")
                        Exit Sub
                    End If
                    main.txtHostAddress.Text = strResult
                    strResult = InputBox("Please enter a user to connect with (require root access): ", "SSH Server Connection: User", "")
                    If strResult = "" Then
                        MsgBox("We cannot continue without a username...", MsgBoxStyle.Exclamation, "SSH Server Connection: NO USER")
                        Exit Sub
                    End If
                    main.txtUsername.Text = strResult
                    strUsername = strResult
                    txtPassword.Visible = True
                    MsgBox("Please enter the password in the password box", MsgBoxStyle.Information, "SSH Server Connection: Password")
                    btnNo.Visible = False
                    btnYes.Text = "OK"
                    txtPassword.Select()
                End If
            Case 3 ' Service Diag
                main.TabControl_Main.SelectedTab = main.TabServer
                LR = SetTopMostWindow(Me.Handle, False)
                lblCmdOut.Text = ""
                lblCmdOut.Visible = True
                main.httpdVersion(True)
            Case 4 ' Local DNS (/etc/hosts)
                If btnYes.Text = "OK" Then
                    btnYes.Text = "Yes"
                    btnNo.Visible = True

                    SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "cp /etc/hosts /etc/hosts.back" & vbCrLf)
                    doCommandSSH("cp /etc/hosts /etc/hosts.back")
                    SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "rm -f /etc/hosts" & vbCrLf)
                    doCommandSSH("rm -f /etc/hosts")
                    For Each line In rtxtCmdOut.Lines
                        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "echo " & line & ">>/etc/hosts" & vbCrLf)
                        doCommandSSH("echo " & line & ">>/etc/hosts")
                    Next
                    main.doSend("cat /etc/hosts")
                    strSentCmd = "cathosts"
                    btnNext.PerformClick()
                Else
                    Panel2.Visible = True
                    btnNo.Visible = False
                    main.doSend("cat /etc/hosts")
                    strSentCmd = "cathosts"
                    btnYes.Text = "OK"

                    MsgBox("Please edit the /etc/hosts file and press OK to commit", MsgBoxStyle.Information, "SSH Server Configuration: DNS")
                End If
            Case 5 ' Server directives - Base
                main.TabControl_Main.SelectedTab = main.TabDirectives
                LR = SetTopMostWindow(Me.Handle, False)
                main.btnGetDirectives_Local.PerformClick()

                Dim strResult As String
                strResult = InputBox("Please enter value for ServerName [www.example.com]: ", "Directive: ServerName", main.txtServerName.Text)
                main.txtServerName.Text = strResult
                strServerName = strResult
                strResult = InputBox("Please enter value for Listen [80]: ", "Directive: Listen", main.txtListen.Text)
                main.txtListen.Text = strResult
                strListen = strResult
                strResult = InputBox("Please enter value for ServerAdmin [root@example.com]: ", "Directive: ServerAdmin", main.txtServerAdmin.Text)
                main.txtServerAdmin.Text = strResult
                strServerAdmin = strResult
                strResult = InputBox("Please enter value for DocumentRoot [""/var/www/html""]: ", "Directive: DocumentRoot", main.txtDocumentRoot.Text)
                main.txtDocumentRoot.Text = strResult
                strDocumentRoot = strResult
            Case 6 ' VirtualHosts
                main.TabControl_Main.SelectedTab = main.TabContainers
                LR = SetTopMostWindow(Me.Handle, False)
                strContainer = "virtualhost"
                main.displayContainer("virtualhost")
                main.ConfigureVirtualHost()
            Case 7 ' Firewall
                main.TabControl_Main.SelectedTab = main.TabFirewall
                LR = SetTopMostWindow(Me.Handle, False)
                If boolSSHConnection = True Then
                    main.doSend("systemctl status firewalld")
                    strSentCmd = "firewalldstatus"
                Else
                    SetText_rtxtTerminal(" - No current SSH connection" & vbCrLf)
                End If
                MsgBox("Please edit or commit the basic firewall rule from the FIREWALL tab", MsgBoxStyle.Information, "Firewall - Commit Rule")
                main.btnGetDirectives_Local.PerformClick()
            Case 8 ' Review Commit
                main.TabControl_Main.SelectedTab = main.TabCommit
                httpdconf.Show()
        End Select
    End Sub
    'User clicks NO
    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click

        'Step forward

        Select Case intPage
            Case 2
                btnNext.PerformClick()
            Case 3
                btnNext.PerformClick()
            Case 4
                btnNext.PerformClick()
            Case 5
                btnNext.PerformClick()
            Case 6
                btnNext.PerformClick()
            Case 7
                btnNext.PerformClick()
            Case 8
                main.TabControl_Main.SelectedTab = main.TabCommit
                exporttoFile()
        End Select
    End Sub
    'Key press=ENTER on the password text box
    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown

        'Enter on password box - thats a YES/Enter

        If e.KeyCode = Keys.Return Then
            btnYes.PerformClick()
        End If
    End Sub
    'User clicks Commit
    Private Sub btnCommit_Click(sender As Object, e As EventArgs) Handles btnCommit.Click

        'Commit starts the save and send httpd.conf push to SSH server

        Dim msgres As MsgBoxResult = MsgBox("Pushing to the SSH Server will overwright /etc/httpd/conf/httpd.conf.  The config must be saved locally first." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
        If msgres = MsgBoxResult.Yes Then
            If boolSSHConnection = True Then
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf)
                'Back up httpd.conf.back
                doCommandSSH("cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "apachectl stop" & vbCrLf)
                'Stop the service
                doCommandSSH("apachectl stop")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "rm -f /etc/httpd/conf/httpd.conf" & vbCrLf)
                'Remove the httpd.conf file
                doCommandSSH("rm -f /etc/httpd/conf/httpd.conf")
                'Initialize the httpd.conf file (generate the httpdconf text)
                loadHttpdFromProgramConfig()
                'Export/Save locally to a file (required to build a filestream) then send to server (TRUE)
                exporttoFile(True)
            Else
                MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
            End If
        End If
    End Sub
    'User clicks Live - Webbrowser
    Private Sub btnLiveWeb_Click(sender As Object, e As EventArgs) Handles btnLiveWeb.Click
        webbrowser.Show()
    End Sub
#End Region

#Region "Subs and Functions"
    'The display and options available on a NEXT PAGE
    Private Sub WizardPages(ByVal intPage As Integer)

        Select Case intPage
            Case 1 ' Intro
                lblMain.Text = "The wizard will guide you through the basic installation and configuration " & vbCrLf &
                     "for your Apache webserver." & vbCrLf & vbCrLf & vbCrLf & "Please click Next to begin the wizard."
                btnBack.Visible = False
            Case 2 ' Connect to server
                main.TabControl_Main.SelectedTab = main.TabServer
                If boolSSHConnection = True Then
                    btnYes.Visible = False
                    btnNo.Visible = False
                    lblMain.Text = "Server Connection" & vbCrLf &
                        "You have a connection to your server at " & strServerConnection & vbCrLf & vbCrLf &
                        "Click Next to continue."
                Else
                    btnYes.Visible = True
                    btnNo.Visible = True
                    lblMain.Text = "Server Connection" & vbCrLf &
                        "Would you like to connect to your server via SSH now? " & vbCrLf &
                         "Connecting allows to forward commands and run diagonsitcs." & vbCrLf & vbCrLf &
                         "Connect now then press Next or connect later and skip."

                    btnYes.Visible = True
                    btnNo.Visible = True

                End If
                btnBack.Visible = True
            Case 3 ' Service Diag
                LR = SetTopMostWindow(Me.Handle, True)
                main.TabControl_Main.SelectedTab = main.TabServer
                Panel2.Visible = False
                If boolSSHConnection = True Then
                    btnYes.Visible = True
                    btnNo.Visible = True

                    lblMain.Text = "Service Diagnostics" & vbCrLf &
                        "Would you like to run HTTPD service diagnostics?" & vbCrLf &
                         "The diagnostics will check is the service is installed and it's current state." & vbCrLf & vbCrLf &
                         "Click Yes now or try later and skip."
                Else
                    lblMain.Text = "Service Diagnostics" & vbCrLf &
                        "Would you like to run HTTPD service diagnostics?" & vbCrLf & vbCrLf &
                         "If you would like to start service diagnostics you need to connect to your server." & vbCrLf &
                         "Go back and connect or skip and move forward."
                    btnYes.Visible = False
                    btnNo.Visible = False
                End If
            Case 4 ' Local DNS
                lblCmdOut.Visible = False
                LR = SetTopMostWindow(Me.Handle, True)
                If boolSSHConnection = True Then
                    btnYes.Visible = True
                    btnNo.Visible = True

                    lblMain.Text = "Service Configuration - Local DNS: Server Name" & vbCrLf &
                        "Would you like to add the name of the webserver into /etc/hosts?" & vbCrLf &
                         "A local address such as www.example.com for testing your configuration." & vbCrLf & vbCrLf &
                         "Click Yes now to bring set the text or skip."
                Else
                    lblMain.Text = "Service Configuration - Local DNS: Server Name" & vbCrLf &
                         "A local address such as www.example.com for testing your configuration." & vbCrLf & vbCrLf &
                         "You must connect to the server.  Go back and connect or skip and move forward."
                    btnYes.Visible = False
                    btnNo.Visible = False
                End If
            Case 5 ' Directives - Base values
                main.TabControl_Main.SelectedTab = main.TabDirectives
                Panel2.Visible = False
                btnYes.Visible = True
                btnYes.Text = "OK"
                btnNo.Visible = False

                lblMain.Text = "Service Configuration - Directives: Base values" & vbCrLf &
                        "Please configure the base directive values." & vbCrLf &
                         "Click OK now to begin the process or stop and goto the DIRECTIVES tab." & vbCrLf & vbCrLf &
                         "Click Next when ready."
            Case 6 ' VirtualHosts
                LR = SetTopMostWindow(Me.Handle, True)
                main.TabControl_Main.SelectedTab = main.TabContainers
                Panel1.Visible = True
                Panel3.Visible = True
                Panel2.Visible = False
                btnYes.Visible = True
                btnYes.Text = "YES"
                lblMain.Text = "Service Configuration - VirtualHosts" & vbCrLf &
                         "Are you adding VirtualHosts to your httpd.conf configuration?." & vbCrLf &
                         "Click YES now to begin configuration or stop and goto the CONTAINERS tab." & vbCrLf & vbCrLf &
                         "Click Next when ready."
            Case 7 ' Firewall
                LR = SetTopMostWindow(Me.Handle, True)
                main.TabControl_Main.SelectedTab = main.TabFirewall
                Panel1.Visible = True
                Panel3.Visible = True
                btnCommit.Visible = False
                btnLiveWeb.Visible = False
                Panel2.Visible = False
                btnYes.Visible = True
                btnNext.Text = "Next"
                btnYes.Text = "YES"
                lblMain.Text = "Service Configuration - Firewall" & vbCrLf &
                       "Would you like to configure the firewall to allow web traffic?." & vbCrLf &
                       "Click YES now to begin configuration or stop and goto the FIREWALL tab." & vbCrLf & vbCrLf &
                       "Click Next when ready."

            Case 8 ' Review Commit
                LR = SetTopMostWindow(Me.Handle, False)
                main.TabControl_Main.SelectedTab = main.TabCommit
                Panel1.Visible = True
                Panel3.Visible = True
                Panel2.Visible = False
                btnYes.Visible = True
                btnCommit.Visible = True
                btnLiveWeb.Visible = True
                btnNext.Text = "Close"
                btnNo.Visible = True
                btnNo.Text = "Export"
                btnYes.Text = "Review"

                lblMain.Text = "Service Configuration - Review, Export, Commit, View Live" & vbCrLf &
                       "Please review your httpd.conf file, export, commit, then view live!."
        End Select
    End Sub
#End Region

End Class