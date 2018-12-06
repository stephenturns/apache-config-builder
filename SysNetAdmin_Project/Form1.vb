Imports System.ComponentModel
Imports System.Threading
Imports Renci.SshNet 'Renci.sshnet - SSH Class for .NET v4 (Renci.SshNet.dll)

Public Class main
#Region "Variable Decleration"
    Dim strLogs_LogSelected As String ' Tab Logs - The file selected (passed to btnLog_Save)
    Dim intI As Integer 'Timer count for SSH copy to server operation
#End Region

#Region "Form Handlers"
    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Build and construct the string container arrays
        loadDefaults()
    End Sub
    Private Sub main_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            ' If a connection is live - Disconnect then close
            If sshClient.IsConnected Then
                doDisconnectSSH()
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Menu Systems"

    'Click handlers for the main menu system.

#Region "Main TOP"
    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Close()
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub
    Private Sub ShowConfigOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowConfigOptionsToolStripMenuItem.Click
        If ShowConfigOptionsToolStripMenuItem.CheckState = CheckState.Checked Then
            SplitContainerMain.Panel1Collapsed = False
        Else
            SplitContainerMain.Panel1Collapsed = True
        End If
    End Sub
    Private Sub SplitTerminalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SplitTerminalToolStripMenuItem.Click
        If SplitTerminalToolStripMenuItem.CheckState = CheckState.Checked Then
            SplitContainerMain.Panel2Collapsed = False
        Else
            SplitContainerMain.Panel2Collapsed = True
        End If
    End Sub
    Private Sub DisconnectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisconnectToolStripMenuItem.Click
        doDisconnectSSH()
    End Sub
    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click
        doStartStopService(True)
    End Sub
    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click
        doStartStopService(False)
    End Sub
    Private Sub RestartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartToolStripMenuItem.Click
        doStartStopService(False, True)
    End Sub
    Private Sub ReloadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReloadToolStripMenuItem.Click
        doStartStopService(False,, True)
    End Sub
    Private Sub GracefulToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GracefulToolStripMenuItem.Click
        doStartStopService(False,,, True)
    End Sub
    Private Sub ConnectgToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectgToolStripMenuItem.Click
        doConnectSSH()
    End Sub
    Private Sub WizardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WizardToolStripMenuItem.Click
        Wizard.Show()
    End Sub
    Private Sub HttpdconfToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HttpdconfToolStripMenuItem.Click
        httpdconf.Show()
    End Sub
    Private Sub WebBrowserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WebBrowserToolStripMenuItem.Click
        webbrowser.Show()
    End Sub
#End Region
#End Region

#Region "User Events"
    'Tab Control - Selected Index Change
    Private Sub TabControl_Main_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl_Main.SelectedIndexChanged

        'When the index changes via user tab selection - Graphical output

        If TabControl_Main.SelectedTab.Name.ToString = TabServer.Name.ToString Then
            If boolSSHConnection = True Then
                PBConnected.Image = My.Resources.Ok48

                PBConnectedToggle.Image = My.Resources.ToggleOn48
            Else
                SetText_ObjectTEXT("No Server Connection", lblServerConnection)
                PBConnected.Image = My.Resources.Fail48
                PBConnectedToggle.Image = My.Resources.ToggleOff48
            End If
        ElseIf TabControl_Main.SelectedTab.Name.ToString = TabContainers.Name.ToString Then
            lblDirectory_dirRoot.Text = "<Directory " & strDocumentRoot & ">"
            If rtxtContainers.Text = "" Then
                strContainer = "directory/"
                displayContainer("directory/")
            End If
        End If
    End Sub
    'Terminal - Click, Command, Key Events
    Private Sub btnConnectSSH_Click(sender As Object, e As EventArgs) Handles btnConnectSSH.Click

        'Connect SSH

        If Not boolSSHConnection = True Then
            PrepareConnection() 'Build the connection
        Else
            Try
                If sshClient.IsConnected = True Then
                    SetText_rtxtTerminal(" - Connected to " & strUsername & "@" & strHostname & vbCrLf)
                Else
                    boolSSHConnection = False
                    PrepareConnection()
                End If
            Catch ex As Exception
                SetText_rtxtTerminal(" - Connected to " & strUsername & "@" & strHostname & vbCrLf)
            End Try
        End If
    End Sub
    Private Sub btnCmdSend_Terminal_Click(sender As Object, e As EventArgs) Handles btnCmdSend_Terminal.Click

        'Send SSH command

        doSend()
        cmbCommands.Text = ""
        txtCmd_Terminal.Select()
    End Sub
    Private Sub btnDisconnectSSH_Click(sender As Object, e As EventArgs) Handles btnDisconnectSSH.Click
        doDisconnectSSH()
    End Sub
    Private Sub cmbCommands_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCommands.SelectedIndexChanged
        txtCmd_Terminal.Text = cmbCommands.SelectedItem
    End Sub
    Private Sub txtCmd_Terminal_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCmd_Terminal.KeyDown

        'Enter key press on txtCmd terminal - Sends the command via ENTER

        If e.KeyCode = Keys.Return Then
            doSend()
            cmbCommands.Text = ""
        ElseIf e.KeyCode = Keys.Up Then
            SetText_ObjectTEXT(strCommand, txtCmd_Terminal)
        End If
    End Sub
    'Wizard Tab - Click
    Private Sub PBWiz_Click(sender As Object, e As EventArgs) Handles PBWiz.Click
        Wizard.Show()
    End Sub
    Private Sub PBHttpdFile_Click(sender As Object, e As EventArgs) Handles PBHttpdFile.Click
        httpdconf.Show()
    End Sub
    'Server Status Tab - Click
    Private Sub PBConnected_Click(sender As Object, e As EventArgs) Handles PBConnected.Click
        If boolSSHConnection = False Then
            doConnectSSH()
        End If
    End Sub
    Private Sub PBConnectedToggle_Click(sender As Object, e As EventArgs) Handles PBConnectedToggle.Click

        If boolSSHConnection = True Then
            doDisconnectSSH()
        Else
            PrepareConnection()
        End If

    End Sub
    Private Sub PBServiceVerToggle_Click(sender As Object, e As EventArgs) Handles PBServiceVerToggle.Click
        If boolSSHConnection = True Then
            If sshClient.IsConnected Then
                httpdVersion()
            End If
        Else
            MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
        End If
    End Sub
    Private Sub PBServiceStatToggle_Click(sender As Object, e As EventArgs) Handles PBServiceStatToggle.Click
        If boolSSHConnection = True Then
            If lblServiceStarted.Text = "HTTPD Service - Active" Then
                doStartStopService(False)
            Else
                doStartStopService(True)
            End If
        Else
            MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
        End If

    End Sub
    'Directives Tab - Click
    Private Sub btnGetDirectives_Click(sender As Object, e As EventArgs) Handles btnGetDirectives.Click
        If Not boolSSHConnection = False Then
            doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^ServerName"" | awk '{print $2}'")
            strSentCmd = "directives"
        Else
            MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
        End If

    End Sub
    Public Sub btnGetDirectives_Local_Click(sender As Object, e As EventArgs) Handles btnGetDirectives_Local.Click
        txtServerName.Text = strServerName
        txtListen.Text = strListen
        txtServerAdmin.Text = strServerAdmin
        txtDocumentRoot.Text = strDocumentRoot
        txtServerRoot.Text = strServerRoot
        txtInclude.Text = strInclude
        txtIncludeOptional.Text = strIncludeOptional
        txtUser.Text = strUser
        txtGroup.Text = strGroup
        txtErrorLog.Text = strErrorLog
        txtLogLevel.Text = strLoglevel
    End Sub
    Private Sub btnSaveDirectives_Values_Click(sender As Object, e As EventArgs) Handles btnSaveDirectives_Values.Click
        strServerName = txtServerName.Text
        strListen = txtListen.Text
        strServerAdmin = txtServerAdmin.Text
        strDocumentRoot = txtDocumentRoot.Text
        strInclude = txtInclude.Text
        strIncludeOptional = txtIncludeOptional.Text
        strUser = txtUser.Text
        strGroup = txtGroup.Text
        strErrorLog = txtErrorLog.Text
        strLoglevel = txtLogLevel.Text
    End Sub
    'Containers Tab - Click
    Private Sub lblDirectory_FSroot_Click(sender As Object, e As EventArgs) Handles lblDirectory_FSroot.Click
        strContainer = "directory/"
        displayContainer("directory/")
    End Sub
    Private Sub lblDirectory_dirRoot_Click(sender As Object, e As EventArgs) Handles lblDirectory_dirRoot.Click
        strContainer = "directory"
        displayContainer("directory")
    End Sub
    Private Sub lblVirtualHost_Click(sender As Object, e As EventArgs) Handles lblVirtualHost.Click
        strContainer = "virtualhost"
        displayContainer("virtualhost")
    End Sub
    Private Sub lblIfModuleDirMod_Click(sender As Object, e As EventArgs) Handles lblIfModuleDirMod.Click
        strContainer = "ifmoduledirmodule"
        displayContainer("ifmoduledirmodule")
    End Sub
    Private Sub lblFilesht_Click(sender As Object, e As EventArgs) Handles lblFilesht.Click
        strContainer = "filesht"
        displayContainer("filesht")
    End Sub
    Private Sub lblMimeModule_Click(sender As Object, e As EventArgs) Handles lblMimeModule.Click
        strContainer = "mimemodule"
        displayContainer("mimemodule")
    End Sub
    Private Sub btnSaveSelContainer_Click(sender As Object, e As EventArgs) Handles btnSaveSelContainer.Click
        SetText_rtxtTerminal(" - building container: " & strContainer & vbCrLf)
        buildContainer(strContainer)
    End Sub
    Private Sub PBVirtualHostConf_Click(sender As Object, e As EventArgs) Handles PBVirtualHostConf.Click
        strContainer = "virtualhost"
        displayContainer("virtualhost")

        ConfigureVirtualHost()
    End Sub
    Private Sub PBVirtualHostReset_Click(sender As Object, e As EventArgs) Handles PBVirtualHostReset.Click
        ReDim strCont_Virtualhost(0 To 1)
        strCont_Virtualhost(0) = "<Virtualhost>"
        strCont_Virtualhost(1) = "</Virtualhost>"
        ReDim strListenMulti(0 To 0)
        ReDim strNamedVirtualHostMulti(0 To 0)
        boolVirtualHostIPBased = False
        boolVirtualHostNameBased_Single = False
        boolVirtualHostsAdded = False
        SetText_rtxtTerminal(" - All VirtualHost related configuration has been reset [No VirtualHost]" & vbCrLf)
        strContainer = "virtualhost"
        displayContainer("virtualhost")
    End Sub
    'Firewall - Click
    Private Sub btnFirewall_CheckStatus_Click(sender As Object, e As EventArgs) Handles btnFirewall_CheckStatus.Click
        If boolSSHConnection = True Then
            doSend("systemctl status firewalld")
            strSentCmd = "firewalldstatus"
        Else
            SetText_rtxtTerminal(" - No current SSH connection" & vbCrLf)
        End If
    End Sub
    Private Sub lblFirewall_ruleD_Click(sender As Object, e As EventArgs) Handles lblFirewall_ruleD.Click
        Dim strRule As String = InputBox("Edit basic firewall rule: " & vbCrLf & lblFirewall_ruleD.Text, "Basic Rule FirewallD: Edit", lblFirewall_ruleD.Text)
        lblFirewall_ruleD.Text = strRule
    End Sub
    Private Sub lblFirewall_RuleIP_Click(sender As Object, e As EventArgs) Handles lblFirewall_RuleIP.Click
        Dim strRule As String = InputBox("Edit basic firewall rule: " & vbCrLf & lblFirewall_RuleIP.Text, "Basic Rule IPTables: Edit", lblFirewall_RuleIP.Text)
        lblFirewall_RuleIP.Text = strRule
    End Sub
    Private Sub btnFirewall_addD_Click(sender As Object, e As EventArgs) Handles btnFirewall_addD.Click
        If boolSSHConnection = True Then
            doSend(lblFirewall_ruleD.Text)
        Else
            SetText_rtxtTerminal(" - No current SSH connection" & vbCrLf)
        End If
    End Sub
    Private Sub btnFirewall_AddIP_Click(sender As Object, e As EventArgs) Handles btnFirewall_AddIP.Click
        If boolSSHConnection = True Then
            doSend(lblFirewall_RuleIP.Text)
        Else
            SetText_rtxtTerminal(" - No current SSH connection" & vbCrLf)
        End If
    End Sub
    'Logs Tab - Click
    Private Sub lblLogs_VarLogMessages_Click(sender As Object, e As EventArgs) Handles lblLogs_VarLogMessages.Click
        If PanelLogsRightBottom.Visible = True Then
            PanelLogsRightBottom.Visible = False
        End If

        buildLogOutput(lblLogs_VarLogMessages.Text)
        PanelLogsRightBottom.Visible = False
    End Sub
    Private Sub lblLogs_VarLogHttpdAccessLog_Click(sender As Object, e As EventArgs) Handles lblLogs_VarLogHttpdAccessLog.Click
        If PanelLogsRightBottom.Visible = True Then
            PanelLogsRightBottom.Visible = False
        End If

        buildLogOutput(lblLogs_VarLogHttpdAccessLog.Text)
        PanelLogsRightBottom.Visible = False
    End Sub
    Private Sub lblLogs_VarLogHttpdErrorLog_Click(sender As Object, e As EventArgs) Handles lblLogs_VarLogHttpdErrorLog.Click
        If PanelLogsRightBottom.Visible = True Then
            PanelLogsRightBottom.Visible = False
        End If

        buildLogOutput(lblLogs_VarLogHttpdErrorLog.Text)
        PanelLogsRightBottom.Visible = False
    End Sub
    Private Sub lblLogs_Custom_Click(sender As Object, e As EventArgs) Handles lblLogs_Custom.Click
        PanelLogsRightBottom.Visible = True
        Dim strLogPath As String = InputBox("Enter a log path:", "Custom Path", "")
        If Not strLogPath = "" Then
            lblLogs_Custom.Text = "[" & strLogPath & "]"
            buildLogOutput(strLogPath)
            strLogs_LogSelected = "custom"
        End If
    End Sub
    Private Sub lblLogs_EtcHosts_Click(sender As Object, e As EventArgs) Handles lblLogs_EtcHosts.Click
        strLogs_LogSelected = "hosts"
        buildLogOutput(lblLogs_EtcHosts.Text)
        PanelLogsRightBottom.Visible = True
    End Sub
    Private Sub lblLogs_Etchttpdconfhttpdconf_Click(sender As Object, e As EventArgs) Handles lblLogs_Etchttpdconfhttpdconf.Click
        PanelLogsRightBottom.Visible = True
        strLogs_LogSelected = "httpdconf"
        buildLogOutput(lblLogs_Etchttpdconfhttpdconf.Text)
    End Sub
    Private Sub btnLogSave_Click(sender As Object, e As EventArgs) Handles btnLogSave.Click
        Try
            Dim msgres As MsgBoxResult
            Dim strCmd1 As String
            Dim strCmd2 As String
            Dim strFile As String
            Dim strFileName As String
            Dim strCustom As String

            If strLogs_LogSelected = "hosts" Then
                msgres = MsgBox("Pushing to the SSH Server will overwright /etc/hosts." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp /etc/hosts /etc/hosts.back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
                strCmd1 = "cp /etc/hosts /etc/hosts.back"
                strCmd2 = "rm -f /etc/hosts"
                strFile = "/etc"
                strFileName = "hosts"
            ElseIf strLogs_LogSelected = "httpdconf" Then
                msgres = MsgBox("Pushing to the SSH Server will overwright /etc/httpd/conf/httpd.conf." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
                strCmd1 = "cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf"
                strCmd2 = "rm -f /etc/httpd/conf/httpd.conf"
            ElseIf strLogs_LogSelected = "custom" Then
                strCustom = lblLogs_Custom.Text
                strCustom = Replace(strCustom, "[", "")
                strCustom = Replace(strCustom, "]", "")

                msgres = MsgBox("Pushing to the SSH Server will overwright " & strCustom & " if present." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp " & strCustom & " " & strCustom & ".back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
                strCmd1 = "cp " & strCustom & " " & strCustom & ".back"
                strCmd2 = "rm -f " & strCustom
                Dim strSplit() As String = Split(strCustom, "/")
                strFile = "/"
                For intI = LBound(strSplit) To UBound(strSplit) - 1
                    If Not intI = 0 Then
                        strFile = strFile & strSplit(intI) & "/"
                    End If
                Next intI
                strFile = LSet(strFile, strFile.Length - 1)
                strFileName = strSplit(strSplit.Count - 1)
            End If

            If msgres = MsgBoxResult.Yes Then
                Me.Cursor = Cursors.AppStarting
                If boolSSHConnection = True Then
                    SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & strCmd1 & vbCrLf)
                    doCommandSSH(strCmd1)

                    If strLogs_LogSelected = "httpdconf" Then
                        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "apachectl stop" & vbCrLf)
                        doCommandSSH("apachectl stop")
                        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & strCmd2 & vbCrLf)
                        doCommandSSH(strCmd2)
                        exporttoFile(True)
                    Else
                        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & strCmd2 & vbCrLf)
                        doCommandSSH(strCmd2)
                        exporttoFile(True, strFile, True, strFileName)
                    End If

                Else
                    MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Copying File")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    'Commit Tab - Click
    Private Sub PBCommit_Click(sender As Object, e As EventArgs) Handles PBCommit.Click
        Dim msgres As MsgBoxResult = MsgBox("Pushing to the SSH Server will overwright /etc/httpd/conf/httpd.conf.  The config must be saved locally first." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
        If msgres = MsgBoxResult.Yes Then
            If boolSSHConnection = True Then
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf)
                doCommandSSH("cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "apachectl stop" & vbCrLf)
                doCommandSSH("apachectl stop")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "rm -f /etc/httpd/conf/httpd.conf" & vbCrLf)
                doCommandSSH("rm -f /etc/httpd/conf/httpd.conf")
                loadHttpdFromProgramConfig()
                exporttoFile(True)
            Else
                MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
            End If

        End If
    End Sub
    Private Sub PBExportConfig_Click(sender As Object, e As EventArgs) Handles PBExportConfig.Click
        exporttoFile()
    End Sub
    Private Sub PBWebaccess_Click(sender As Object, e As EventArgs) Handles PBWebaccess.Click
        webbrowser.Show()
    End Sub
#End Region

#Region "Subs & Functions"
    'Disconnect the SSH session
    Public Sub doDisconnectSSH()

        'Disconnect the SSH Session if active

        Try
            If sshClient.IsConnected Then
                intSSHStatus = 99
                sshClient.Disconnect()
                sshClient.Dispose()
                SetText_rtxtTerminal(" - Disconnected from " & strHostname & vbCrLf)
                SetText_ObjectTEXT("No Server Connection", lblServerConnection)
                PBConnected.Image = My.Resources.Fail48
                PBHTTPDVersion.Image = My.Resources.Help48
                PBServiceStatus.Image = My.Resources.Help48
                PBServiceVerToggle.Image = My.Resources.ToggleOff48
                PBServiceStatToggle.Image = My.Resources.ToggleOff48
                PBConnectedToggle.Image = My.Resources.ToggleOff48
                boolSSHConnection = False
            Else
                SetText_rtxtTerminal(" - No current SSH connection" & vbCrLf)
            End If
        Catch ex As NullReferenceException
            SetText_rtxtTerminal(" - Not associated with SSH Client" & vbCrLf)
        Catch ex As ObjectDisposedException
            SetText_rtxtTerminal(" - Not associated with SSH Client" & vbCrLf)
        Catch ex As Exception
            SetText_rtxtTerminal(" - ERROR: " & ex.Message & vbCrLf)
        End Try
    End Sub

    'Prepare commands to send to SSH session
    Public Sub doSend(Optional ByVal strSelCommand As String = "")

        'Prepare and send commands to SSH server or terminal window

        If strSelCommand = "" Then
            strCommand = txtCmd_Terminal.Text
        Else
            strCommand = strSelCommand
        End If

        If strCommand.ToLower = "clear" Or strCommand = "cls" Then
            rtxtTerminal.Clear()
            SetText_rtxtTerminal(strUsername & "@" & strHostname & " # ")
            SetText_ObjectTEXT("", txtCmd_Terminal)
        ElseIf strCommand.ToLower = "exit" Or strCommand = "close" Or strCommand = "disconnect" Then
            doDisconnectSSH()
            SetText_ObjectTEXT("", txtCmd_Terminal)
        ElseIf strCommand.ToLower = "?" Or strCommand = "-h" Or strCommand = "help" Then
            commandHelp()
        ElseIf strCommand.ToLower = "status" Then
            doStartStopService(False, False, False, False, True)
        ElseIf strCommand.ToLower = "start" Then
            doStartStopService(True)
        ElseIf strCommand.ToLower = "stop" Then
            doStartStopService(False)
        ElseIf strCommand.ToLower = "restart" Then
            doStartStopService(False, True)
        ElseIf strCommand.ToLower = "check config" Then
            doCheckConfig()
        ElseIf strCommand.ToLower = "check virtualhost" Then
            doSend("apachectl -S")
        ElseIf strCommand.ToLower = "ver" Or strCommand.ToLower = "version" Then
            If boolSSHConnection = True Then
                httpdVersion()
            Else
                SetText_rtxtTerminal(" - No SSH Connection" & vbCrLf)
            End If
        ElseIf strCommand.ToLower = "connect" Then
            If Not boolSSHConnection = True Then
                PrepareConnection()
            Else
                SetText_rtxtTerminal(" - Connected to " & strUsername & "@" & strHostname & vbCrLf)
            End If
        ElseIf strCommand = "" Then
            commandHelp()
        Else
            If Not boolSSHConnection = True Then
                SetText_rtxtTerminal(strCommand & vbCrLf)
                SetText_rtxtTerminal(" - No SSH Connection" & vbCrLf)
                commandHelp()
            End If

            If Not BWdoCommandSSH.IsBusy Then
                PBCommandStatus.Image = My.Resources.loadcirc
                btnCmdSend_Terminal.Enabled = False
                SetText_rtxtTerminal(strCommand & vbCrLf)
                BWdoCommandSSH.RunWorkerAsync()
            Else
                MsgBox("Please wait until current command returns", MsgBoxStyle.Information, "Processing Command")
            End If

        End If
    End Sub

    'Terminal help/? - Show list of commands
    Private Sub commandHelp()

        '? or help in the terminal window - Show a list of commands.

        SetText_rtxtTerminal(strCommand & vbCrLf)
        SetText_rtxtTerminal(" connect - connect to the ssh server" & vbCrLf)
        SetText_rtxtTerminal(" ver - check the version of the httpd service" & vbCrLf)
        SetText_rtxtTerminal(" version - check the version of the httpd service" & vbCrLf)
        SetText_rtxtTerminal(" status - check the status of the httpd service" & vbCrLf)
        SetText_rtxtTerminal(" start - start the httpd service" & vbCrLf)
        SetText_rtxtTerminal(" stop - stop the httpd service" & vbCrLf)
        SetText_rtxtTerminal(" restart - restart the httpd service" & vbCrLf)
        SetText_rtxtTerminal(" check config - check the config file httpd.conf for error" & vbCrLf)
        SetText_rtxtTerminal(" check virtualhost - check VirtualHost config from httpd.conf" & vbCrLf)
        SetText_rtxtTerminal(" clear - clear the terminal text" & vbCrLf)
        SetText_rtxtTerminal(" cls - clear the terminal text" & vbCrLf)
        SetText_rtxtTerminal(" exit - disconnect from an ssh session" & vbCrLf)
        SetText_rtxtTerminal(" close - disconnect from an ssh session" & vbCrLf)
        SetText_rtxtTerminal(" disconnect - disconnect from an ssh session" & vbCrLf)
        SetText_ObjectTEXT("", txtCmd_Terminal)
        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # ")
        SetText_rtxtTerminal("")
    End Sub

    'Check httpd version; install httpd service via yum
    Public Sub doCheckConfig()
        doSend("apachectl configtest 2> /tmp/apache_configtest")
        strSentCmd = "apachectlconfigtest"
    End Sub

    'Prepare and send httpdVersion syntax to SSH server
    Public Sub httpdVersion(ByVal Optional boolStatusCheck As Boolean = False)
        If boolStatusCheck = True Then
            doSend("httpd -v | head -1 && apachectl status")
            strSentCmd = "version"
        Else
            doSend("httpd -v | head -1")
            strSentCmd = "version"
        End If
    End Sub

    'Start and stop the httpd service - Return true if service active
    Public Sub doStartStopService(ByVal boolStart As Boolean, Optional ByVal boolRestart As Boolean = False, Optional ByVal boolReload As Boolean = False, Optional ByVal boolGraceful As Boolean = False, Optional ByVal boolStatusOnly As Boolean = False)
        If boolSSHConnection = True Then
            If sshClient.IsConnected Then
                strSentCmd = "service"
                If boolRestart = True Then
                    PBServiceStatToggle.Image = My.Resources.ToggleOff48
                    PBServiceStatus.Image = My.Resources.Help48
                    doSend("apachectl restart && apachectl status")
                ElseIf boolReload = True Then
                    PBServiceStatus.Image = My.Resources.Help48
                    doSend("systemctl reload httpd && apachectl status")
                ElseIf boolGraceful = True Then
                    PBServiceStatus.Image = My.Resources.Help48
                    doSend("apachectl graceful && apachectl status")
                ElseIf boolStart = True Then
                    doSend("apachectl start && apachectl status")
                ElseIf boolStatusOnly = True Then
                    doSend("apachectl status")
                ElseIf boolStart = False Then
                    doSend("apachectl stop && apachectl status")
                End If
            End If
        End If
    End Sub

    'Prepare log string and bind Thread (doSend(strCmd))
    Private Sub buildLogOutput(ByVal strLog As String)
        If boolSSHConnection = True Then
            Dim strTail As String = ""

            If Not txtLogTail.Text = "" Then
                strTail = " | " & txtLogTail.Text
            End If

            Dim strCmd As String = "cat " & strLog & strTail
            doSend(strCmd)
            strSentCmd = "logs"
        Else
            MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
        End If
    End Sub

    'Initialize and build the container (httpd.conf) string array varaibles
    Public Function buildContainer(ByVal strContainer As String)

        'Build the array values for the container strings.
        'The values are taken from the text box with tabContainers.

        Try
            Dim boolDimensioned As Boolean

            If strContainer = "directory/" Then
                For Each line In rtxtContainers.Lines
                    If Not line = "" Then
                        If boolDimensioned = True Then
                            ReDim Preserve strCont_DirectoryRoot(0 To UBound(strCont_DirectoryRoot) + 1)
                        Else
                            boolDimensioned = True
                            ReDim strCont_DirectoryRoot(0 To 0)
                        End If
                        strCont_DirectoryRoot(UBound(strCont_DirectoryRoot)) = line
                    End If
                Next
            ElseIf strContainer = "directory" Then
                For Each line In rtxtContainers.Lines
                    If Not line = "" Then
                        If boolDimensioned = True Then
                            ReDim Preserve strCont_Directory(0 To UBound(strCont_Directory) + 1)
                        Else
                            boolDimensioned = True
                            ReDim strCont_Directory(0 To 0)
                        End If
                        strCont_Directory(UBound(strCont_Directory)) = line
                    End If
                Next
            ElseIf strContainer = "virtualhost" Then
                For Each line In rtxtContainers.Lines
                    If Not line = "" Then
                        If boolDimensioned = True Then
                            ReDim Preserve strCont_Virtualhost(0 To UBound(strCont_Virtualhost) + 1)
                        Else
                            boolDimensioned = True
                            ReDim strCont_Virtualhost(0 To 0)
                        End If
                        strCont_Virtualhost(UBound(strCont_Virtualhost)) = line
                    End If
                Next
            ElseIf strContainer = "ifmoduledirmodule" Then
                For Each line In rtxtContainers.Lines
                    If Not line = "" Then
                        If boolDimensioned = True Then
                            ReDim Preserve strCont_IfMod_dirmod(0 To UBound(strCont_IfMod_dirmod) + 1)
                        Else
                            boolDimensioned = True
                            ReDim strCont_IfMod_dirmod(0 To 0)
                        End If
                        strCont_IfMod_dirmod(UBound(strCont_IfMod_dirmod)) = line
                    End If
                Next
            ElseIf strContainer = "filesht" Then
                For Each line In rtxtContainers.Lines
                    If Not line = "" Then
                        If boolDimensioned = True Then
                            ReDim Preserve strCont_Files_ht(0 To UBound(strCont_Files_ht) + 1)
                        Else
                            boolDimensioned = True
                            ReDim strCont_Files_ht(0 To 0)
                        End If
                        strCont_Files_ht(UBound(strCont_Files_ht)) = line
                    End If
                Next
            ElseIf strContainer = "mimemodule" Then
                For Each line In rtxtContainers.Lines
                    If Not line = "" Then
                        If boolDimensioned = True Then
                            ReDim Preserve strCont_IfMod_mimemod(0 To UBound(strCont_IfMod_mimemod) + 1)
                        Else
                            boolDimensioned = True
                            ReDim strCont_IfMod_mimemod(0 To 0)
                        End If
                        strCont_IfMod_mimemod(UBound(strCont_IfMod_mimemod)) = line
                    End If
                Next
            End If
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    'Display the contents of the container (httpd.conf) string array values
    Public Function displayContainer(ByVal strContainer As String)
        Try
            rtxtContainers.Clear()
            Dim intI As Integer = 0
            If strContainer = "directory/" Then
                For intI = LBound(strCont_DirectoryRoot) To UBound(strCont_DirectoryRoot)
                    SetText_ObjectTEXT(strCont_DirectoryRoot(intI) & vbCrLf, rtxtContainers, True)
                Next intI
            ElseIf strContainer = "directory" Then
                For intI = LBound(strCont_Directory) To UBound(strCont_Directory)
                    SetText_ObjectTEXT(strCont_Directory(intI) & vbCrLf, rtxtContainers, True)
                Next intI
            ElseIf strContainer = "virtualhost" Then
                For intI = LBound(strCont_Virtualhost) To UBound(strCont_Virtualhost)
                    SetText_ObjectTEXT(strCont_Virtualhost(intI) & vbCrLf, rtxtContainers, True)
                Next intI
            ElseIf strContainer = "ifmoduledirmodule" Then
                For intI = LBound(strCont_IfMod_dirmod) To UBound(strCont_IfMod_dirmod)
                    SetText_ObjectTEXT(strCont_IfMod_dirmod(intI) & vbCrLf, rtxtContainers, True)
                Next intI
            ElseIf strContainer = "filesht" Then
                For intI = LBound(strCont_Files_ht) To UBound(strCont_Files_ht)
                    SetText_ObjectTEXT(strCont_Files_ht(intI) & vbCrLf, rtxtContainers, True)
                Next intI
            ElseIf strContainer = "mimemodule" Then
                For intI = LBound(strCont_IfMod_mimemod) To UBound(strCont_IfMod_mimemod)
                    SetText_ObjectTEXT(strCont_IfMod_mimemod(intI) & vbCrLf, rtxtContainers, True)
                Next intI
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    'VirtualHost configuration - Build and add virtualhost configuration to httpd.conf
    Public Sub ConfigureVirtualHost()

        'Prompts to build the virtualhost containers.
        'Loads them into a string variable, copies to rtxtContainers, the builds/saves the virtualhost array.

        Dim strVirtualHost As String
        Dim strResult As String
        Dim boolNamebased As Boolean
        Dim boolNamebased_SingleIP As Boolean
        Dim msgres As MsgBoxResult

        msgres = MsgBox("Is this a named-based virtualhost?", vbYesNoCancel, "VirtualHost Setup - 1: name-based?")
        If msgres = vbCancel Then
            Exit Sub

        ElseIf msgres = vbYes Then
            boolNamebased = True
        ElseIf msgres = vbNo Then
            boolNamebased = False
        End If

        If boolNamebased = True Then
            msgres = MsgBox("Are you running name-based websites on a single IP address?", vbYesNoCancel, "VirtualHost Setup - 2: single-IP?")
            If msgres = vbCancel Then
                Exit Sub
            ElseIf msgres = vbYes Then
                boolNamebased_SingleIP = True
            ElseIf msgres = vbNo Then
                boolNamebased_SingleIP = False
            End If

            If boolNamebased_SingleIP = True Then
                If boolVirtualHostNameBased_Single = True Then
                    msgres = MsgBox("A VirtualHost IP has already been added.  You selected multiple IP's prior." & vbCrLf & "Adding a VirtualHost Single IP now may cause config error." & vbCrLf & vbCrLf & "Continue?", MsgBoxStyle.OkCancel, "VirtualHost Setup - 2 Single-IP")
                    If msgres = vbCancel Then
                        Exit Sub
                    End If
                End If

                strVirtualHost = "<VirtualHost *:" & strListen.Trim & ">" & vbCrLf
                strResult = InputBox("Please enter value for ServerAdmin [webmaster@example.com]: ", "VirtualHost Setup - 3: ServerAdmin", strServerAdmin)
                strVirtualHost = strVirtualHost & "  ServerAdmin " & strResult.Trim & vbCrLf
                strResult = InputBox("Please enter value for DocumentRoot [""/var/www/html""]: ", "VirtualHost Setup - 4: DocumentRoot", strDocumentRoot)
                strVirtualHost = strVirtualHost & "  DocumentRoot " & strResult.Trim & vbCrLf

                strVirtualHost = strVirtualHost & "  <Directory " & strResult.Trim & ">" & vbCrLf
                strVirtualHost = strVirtualHost & "     Options Indexes FollowSymLinks" & vbCrLf
                strVirtualHost = strVirtualHost & "     AllowOverride None" & vbCrLf
                strVirtualHost = strVirtualHost & "     Require all granted" & vbCrLf
                strVirtualHost = strVirtualHost & "  </Directory>" & vbCrLf

                strResult = InputBox("Please enter value for ServerName [www.example.com]: ", "VirtualHost Setup - 5: ServerName", strServerName)
                strVirtualHost = strVirtualHost & "  ServerName " & strResult.Trim & vbCrLf
                strResult = InputBox("Please enter value for ErrorLog [""/logs/error_log""]: ", "VirtualHost Setup - 6: ErrorLog", strErrorLog)
                strVirtualHost = strVirtualHost & "  ErrorLog " & strResult.Trim & vbCrLf
                strResult = InputBox("Please enter value for LogLevel [warn]: ", "VirtualHost Setup - 7: LogLevel", strLoglevel)
                strVirtualHost = strVirtualHost & "  LogLevel " & strResult.Trim & vbCrLf
                strVirtualHost = strVirtualHost & "</VirtualHost>" & vbCrLf
                boolNamebased_SingleIP = True
            Else
                Dim strPort As String = InputBox("Please enter value for the Port number to bind", "VirtualHost Setup - 3: Bind Port", strListen)
                strResult = InputBox("Please enter value for this IP Address addition: ", "VirtualHost Setup - 4: IP Address Addition", "")
                strVirtualHost = "<VirtualHost " & strResult.Trim & ":" & strPort.Trim & ">" & vbCrLf

                boolVirtualHostNameBased_Single = True
                ReDim Preserve strNamedVirtualHostMulti(0 To UBound(strNamedVirtualHostMulti) + 1)
                strNamedVirtualHostMulti(UBound(strNamedVirtualHostMulti)) = "NameVirtualHost " & strResult.Trim

                strResult = InputBox("Please enter value for ServerAdmin [""webmaster@www.example.com""]: ", "VirtualHost Setup - 5: ServerAdmin", strServerAdmin)
                strVirtualHost = strVirtualHost & "  ServerAdmin " & strResult.Trim & vbCrLf
                strResult = InputBox("Please enter value for DocumentRoot [""/var/www/html""]: ", "VirtualHost Setup - 6: DocumentRoot", strDocumentRoot)
                strVirtualHost = strVirtualHost & "  DocumentRoot " & strResult.Trim & vbCrLf

                strVirtualHost = strVirtualHost & "  <Directory " & strResult.Trim & ">" & vbCrLf
                strVirtualHost = strVirtualHost & "     Options Indexes FollowSymLinks" & vbCrLf
                strVirtualHost = strVirtualHost & "     AllowOverride None" & vbCrLf
                strVirtualHost = strVirtualHost & "     Require all granted" & vbCrLf
                strVirtualHost = strVirtualHost & "  </Directory>" & vbCrLf

                strResult = InputBox("Please enter value for ServerName [www.example.com]: ", "VirtualHost Setup - 7: ServerName", strServerName)
                strVirtualHost = strVirtualHost & "  ServerName " & strResult.Trim & vbCrLf
                strResult = InputBox("Please enter value for ErrorLog [""/logs/error_log""]: ", "VirtualHost Setup - 8: ErrorLog", strErrorLog)
                strVirtualHost = strVirtualHost & "  ErrorLog " & strResult.Trim & vbCrLf
                strResult = InputBox("Please enter value for LogLevel [warn]: ", "VirtualHost Setup - 9: LogLevel", strLoglevel)
                strVirtualHost = strVirtualHost & "  LogLevel " & strResult.Trim & vbCrLf
                strVirtualHost = strVirtualHost & "</VirtualHost>" & vbCrLf
                boolNamebased_SingleIP = False
            End If
        End If

        If boolNamebased = False Then
            Dim strPort As String = InputBox("Please enter value for the Port number to bind", "VirtualHost Setup - 3: Bind Port", strListen)
            strResult = InputBox("Please enter value for this IP Address addition", "VirtualHost Setup - 3: IP Address Addition", "")
            strVirtualHost = "<VirtualHost " & strResult.Trim & ":" & strPort.Trim & ">" & vbCrLf

            boolVirtualHostIPBased = True
            ReDim Preserve strListenMulti(0 To UBound(strListenMulti) + 1)
            strListenMulti(UBound(strListenMulti)) = "Listen " & strResult.Trim & ":" & strPort

            strResult = InputBox("Please enter value for ServerAdmin [""webmaster@www.example.com""]: ", "VirtualHost Setup - 4: ServerAdmin", strServerAdmin)
            strVirtualHost = strVirtualHost & "  ServerAdmin " & strResult.Trim & vbCrLf
            strResult = InputBox("Please enter value for DocumentRoot [""/var/www/html""]: ", "VirtualHost Setup - 5: DocumentRoot", strDocumentRoot)
            strVirtualHost = strVirtualHost & "  DocumentRoot " & strResult.Trim & vbCrLf

            strVirtualHost = strVirtualHost & "  <Directory " & strResult.Trim & ">" & vbCrLf
            strVirtualHost = strVirtualHost & "     Options Indexes FollowSymLinks" & vbCrLf
            strVirtualHost = strVirtualHost & "     AllowOverride None" & vbCrLf
            strVirtualHost = strVirtualHost & "     Require all granted" & vbCrLf
            strVirtualHost = strVirtualHost & "  </Directory>" & vbCrLf

            strResult = InputBox("Please enter value for ServerName [www.example.com]: ", "VirtualHost Setup - 6: ServerName", strServerName)
            strVirtualHost = strVirtualHost & "  ServerName " & strResult.Trim & vbCrLf
            strResult = InputBox("Please enter value for ErrorLog [""/logs/error_log""]: ", "VirtualHost Setup - 7: ErrorLog", strErrorLog)
            strVirtualHost = strVirtualHost & "  ErrorLog " & strResult.Trim & vbCrLf
            strResult = InputBox("Please enter value for LogLevel [warn]: ", "VirtualHost Setup - 8: LogLevel", strLoglevel)
            strVirtualHost = strVirtualHost & "  LogLevel " & strResult.Trim & vbCrLf
            strVirtualHost = strVirtualHost & "</VirtualHost>" & vbCrLf
        End If

        If boolVirtualHostsAdded = True Then
            rtxtContainers.AppendText(strVirtualHost)
        Else
            rtxtContainers.Text = strVirtualHost
        End If

        SetText_rtxtTerminal(" - building container: " & strContainer & vbCrLf)
        buildContainer(strContainer)
        boolVirtualHostsAdded = True

        If boolWizardActive = True Then
            Wizard.btnYes.Text = "Add"
        End If

    End Sub
#End Region

#Region "Threads - Timers/Background Workers"
    'Check the status of the SSH connection - Output graphically
    Private Sub TimerSSHStatus_Tick(sender As Object, e As EventArgs) Handles TimerSSHStatus.Tick

        'Main timer thread for graphical output (terminal)
        'Displays connection status
        'Displays file copy operation progress

        If Not boolFileCopy = True Then
            intSSHStatus += 1
        End If

        If boolSSHConnection = True Then
            If Not boolFileCopy = True Then
                TimerSSHStatus.Interval = 1000
            Else
                TimerSSHStatus.Interval = 50
                SetText_rtxtTerminal(".")

                If intSSHStatus = 5000 Then
                    If boolExists = True Then
                        SetText_rtxtTerminal("Completed Successfully!" & vbCrLf)
                    Else
                        SetText_rtxtTerminal("Failed!" & vbCrLf)
                        If Not strErrorReturn = "" Then
                            SetText_rtxtTerminal(strErrorReturn)
                            strErrorReturn = ""
                        End If
                    End If

                    If Not boolCopyFromLog = True Then
                        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "apachectl start" & vbCrLf)
                        doCommandSSH("apachectl start")
                        doCheckConfig()
                    End If

                    intSSHStatus = 0
                    intI = 0
                    boolFileCopy = False
                    Cursor = Cursors.Default
                Else
                    intI += 1
                    If intI = 1200 Then
                        SetText_rtxtTerminal("- SSH server copy operation terminated!" & vbCrLf)
                        SetText_rtxtTerminal("- Time expired to catch a successful operation (120 seconds)" & vbCrLf)
                        SetText_rtxtTerminal("- Please try the push operation again." & vbCrLf)
                        boolFileCopy = False
                    End If
                End If
            End If

            If sshClient Is Nothing Then
                'if sshClient disposed then start disconnect graphics
                If intSSHStatus < 100 Then
                    intSSHStatus = 99
                    SetText_rtxtTerminal(" - connection disconnected" & vbCrLf)
                End If
            ElseIf Not sshClient.IsConnected = True Then
                If intSSHStatus < 100 Then
                    intSSHStatus = 99
                    SetText_rtxtTerminal(" - connection timeout " & My.Computer.Clock.LocalTime & vbCrLf)
                End If
            End If
        Else
            TimerSSHStatus.Interval = 500
            If Not intSSHStatus = 99 And Not intSSHStatus = 100 Then
                SetText_rtxtTerminal(".")
            End If
        End If

        'Graphical network status
        If intSSHStatus = 1 Then
            PBConnectionStatus.BackgroundImage = My.Resources.network1.ToBitmap
            intSSHStatus += 1
        ElseIf intSSHStatus = 2 Then
            PBConnectionStatus.BackgroundImage = My.Resources.networkreceive
            intSSHStatus += 1
        ElseIf intSSHStatus = 3 Then
            PBConnectionStatus.BackgroundImage = My.Resources.networktransmit.ToBitmap
            intSSHStatus += 1
        ElseIf intSSHStatus = 99 Then
            PBConnectionStatus.BackgroundImage = My.Resources.networkoffline.ToBitmap
            TimerSSHStatus.Interval = 2000
        ElseIf intSSHStatus = 100 Then
            PBConnectionStatus.BackgroundImage = My.Resources.network1.ToBitmap
            TimerSSHStatus.Stop()
            intSSHStatus = 0
        Else
            PBConnectionStatus.BackgroundImage = My.Resources.networktransmitreceive.ToBitmap
            intSSHStatus = 0
        End If

    End Sub

    'Main SSH send command thread - Send strCommand to doCommandSSH(strCommand)
    Public Sub BWdoCommandSSH_DoWork(sender As Object, e As DoWorkEventArgs) Handles BWdoCommandSSH.DoWork
        doCommandSSH(strCommand)
    End Sub
    Private Sub BWdoCommandSSH_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BWdoCommandSSH.RunWorkerCompleted

        'Output display - the result of the SSH Command

        SetText_rtxtTerminal(strCommandResult)
        SetText_ObjectTEXT("", txtCmd_Terminal)
        SetText_rtxtTerminal(strUsername & "@" & strHostname & " # ")
        SetText_rtxtTerminal("")

        'Once the command thread has finished - proceed to process the results of the output
        CommandResultProc()

        btnCmdSend_Terminal.Enabled = True
    End Sub

    'Main SSH connect thread - Connect to the ssh server
    Public Sub BWdoConnectSSH_DoWork(sender As Object, e As DoWorkEventArgs) Handles BWdoConnectSSH.DoWork
        doConnectSSH()
    End Sub
    Public Sub BWdoConnectSSH_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BWdoConnectSSH.RunWorkerCompleted

        'SSH connected?  - Display the terminal output.

        If sshClient.IsConnected Then
            SetText_rtxtTerminal("Connected" & vbCrLf)
            SetText_rtxtTerminal(strUsername & "@" & strHostname & " # ")
            SetText_ObjectTEXT("Connected: " & strUsername & "@" & strHostname, lblServerConnection)
            PBConnected.Image = My.Resources.Ok48
            PBConnectedToggle.Image = My.Resources.ToggleOn48
            boolSSHConnection = True
            strServerConnection = strUsername & "@" & strHostname

            If boolWizardActive = True Then
                Wizard.lblCmdOut.Text = "Connected: " & strUsername & "@" & strHostname
            End If
        End If

        If Not strErrorReturn = "" Then
            SetText_rtxtTerminal(strErrorReturn)
            SetText_rtxtTerminal(" - Failed to connect" & vbCrLf)
            strErrorReturn = ""
            If boolWizardActive = True Then
                Wizard.lblCmdOut.Text = "Failed to connect"
            End If
        End If
        txtCmd_Terminal.Select()
    End Sub


#End Region

End Class
