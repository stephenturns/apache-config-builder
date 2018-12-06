Imports Renci.SshNet 'Renci.sshnet - SSH Class for .NET v4 (Renci.SshNet.dll)
Imports System.Threading 'Threading Class
Module Module1

    'Module1 - Global variable and routine parser

#Region "Variable Decleration"
    'Thread handler
    Delegate Sub settextcallbackObject([text] As String, objOfInvoke As Object) ' Global thread for objects
    Delegate Sub settextcallback([text] As String) ' Global thread for rtxtTerminal

    'Set the a window on top of all windows
    Public Const SWP_NOMOVE = 2
    Public Const SWP_NOSIZE = 1
    Public Const FLAGS = SWP_NOMOVE Or SWP_NOSIZE
    Public Const HWND_TOPMOST = -1
    Public Const HWND_NOTOPMOST = -2
    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Long, ByVal hWndInsertAfter As Long, ByVal x As Long, ByVal y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long

    'SSH Client and Connection variables
    Public sshConnectionDetails As PasswordConnectionInfo
    Public sshClient As SshClient
    Public sshCommand As SshCommand

    'General
    Public boolSSHConnection As Boolean 'True when SSH server is active
    Public strServerConnection As String ' strUsername & "@" & strHostname - for terminal display
    Public strCommand As String 'The command sent to the doSend() - for global processing 
    Public strCommandResult As String 'The output from doCommandSSH() - The reply from the SSH server command 
    Public strHostname As String 'Connection hostname
    Public strUsername As String 'Connection username
    Public strPassword As String 'Connection password
    Public strErrorReturn As String 'Global exception.message catcher.  To show errors to user.
    Public boolWizardActive As Boolean 'True when commands are pushed to threads via the wizard.  So the reply can be directed back.
    Public strSentCmd As String 'The command id sent to doSend().  Used to direct commandResultProc() for user/terminal output/direction.
    Public boolFileCopy As Boolean 'True when an SFTP file copy operation is current.  Used to direct output for timerSSHStatus graphical terminal.
    Public boolExists As Boolean 'True if file copy operation exists.  (sftp.Exists(strDestDir & "/" & strRemoteFileName))
    Public boolCopyFromLog As Boolean 'True if the copy operation is user directed from the tabLog.  For focused return.
    Public intSSHStatus As Integer 'The counter for the timerSSHStatus.  Values represent graphical output options.

    'Directive Values (default values for basic directives)
    Public strServerName As String
    Public strListen As String = "80"
    Public strServerAdmin As String = "root@localhost"
    Public strDocumentRoot As String = """/var/www/html"""
    Public strServerRoot As String = """/etc/httpd"""
    Public strInclude As String = "conf.modules.d/*.conf"
    Public strIncludeOptional As String = "conf.d/*.conf"
    Public strUser As String = "apache"
    Public strGroup As String = "apache"
    Public strErrorLog As String = """logs/error_log"""
    Public strLoglevel As String = "warn"

    'Container Values (decleration of container arrays)
    Public strContainer As String
    Public strCont_DirectoryRoot() As String
    Public strCont_Directory() As String
    Public strCont_Virtualhost() As String
    Public strCont_IfMod_dirmod() As String
    Public strCont_Files_ht() As String
    Public strCont_IfMod_mimemod() As String
    Public strListenMulti() As String
    Public strNamedVirtualHostMulti() As String

    Public boolVirtualHostsAdded As Boolean 'True when a virtualhost is added
    Public boolVirtualHostIPBased As Boolean 'True when the virtualhost is IP based
    Public boolVirtualHostNameBased_Single As Boolean 'True when the NameBased virtualhost is single IP
#End Region

#Region "Threading Global"
    'Global thread for rtxtTerminal text output.
    Public Sub SetText_rtxtTerminal(ByVal [text] As String)

        'Isolate thread call to main.rtxtTerminal

        Try
            If main.rtxtTerminal.InvokeRequired Then
                Dim d As New settextcallback(AddressOf SetText_rtxtTerminal)
                main.Invoke(d, New Object() {[text]})
            Else
                'Append the text - scroll to the end
                main.rtxtTerminal.SelectionStart = main.rtxtTerminal.TextLength
                main.rtxtTerminal.ScrollToCaret()
                main.rtxtTerminal.AppendText([text])
                main.rtxtTerminal.SelectionStart = main.rtxtTerminal.TextLength
                main.rtxtTerminal.ScrollToCaret()
            End If
        Catch ex As Exception
        End Try
    End Sub

    'Global thread for all other object text output.
    Public Sub SetText_ObjectTEXT(ByVal [text] As String, Optional ByVal objOfInvoke As Object = Nothing, Optional ByVal boolAppend As Boolean = False)

        'Isolate thread call to all other objects (links, labels, text)

        Try
            If objOfInvoke.InvokeRequired Then
                Dim d As New settextcallbackObject(AddressOf SetText_ObjectTEXT)
                main.Invoke(d, New Object() {[text], objOfInvoke})
            Else
                If boolAppend = True Then
                    objOfInvoke.appendtext([text])
                Else
                    objOfInvoke.Text = ([text])
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Subs and Functions Global"
    'Build and define the string array container values
    Public Sub loadDefaults()

        'Build and define the defaults for all the container string arrays

        Try
            ReDim strCont_DirectoryRoot(0 To 3)
            strCont_DirectoryRoot(0) = "<Directory />"
            strCont_DirectoryRoot(1) = "  AllowOverride none"
            strCont_DirectoryRoot(2) = "  Require all denied"
            strCont_DirectoryRoot(3) = "</Directory>"
            ReDim strCont_Directory(0 To 4)
            strCont_Directory(0) = "<Directory " & strDocumentRoot & ">"
            strCont_Directory(1) = "  Options Indexes FollowSymLinks"
            strCont_Directory(2) = "  AllowOverride None"
            strCont_Directory(3) = "  Require all granted"
            strCont_Directory(4) = "</Directory>"
            ReDim strCont_Virtualhost(0 To 1)
            strCont_Virtualhost(0) = "<Virtualhost>"
            strCont_Virtualhost(1) = "</Virtualhost>"
            ReDim strCont_IfMod_dirmod(0 To 2)
            strCont_IfMod_dirmod(0) = "<IfModule dir_module>"
            strCont_IfMod_dirmod(1) = "  DirectoryIndex index.html"
            strCont_IfMod_dirmod(2) = "</IfModule>"
            ReDim strCont_Files_ht(0 To 2)
            strCont_Files_ht(0) = "<Files """ & ".ht*" & """>"
            strCont_Files_ht(1) = "  Require all denied"
            strCont_Files_ht(2) = "</Files>"
            ReDim strCont_IfMod_mimemod(0 To 6)
            strCont_IfMod_mimemod(0) = "<IfModule mime_module>"
            strCont_IfMod_mimemod(1) = "  TypesConfig /etc/mime.types"
            strCont_IfMod_mimemod(2) = "  AddType Application/x - compress.Z"
            strCont_IfMod_mimemod(3) = "  AddType Application/x - gzip.gz.tgz"
            strCont_IfMod_mimemod(4) = "  AddType Text/html .shtml"
            strCont_IfMod_mimemod(5) = "  AddOutputFilter INCLUDES .shtml"
            strCont_IfMod_mimemod(6) = "</IfModule>"
            ReDim strListenMulti(0 To 0)
            ReDim strNamedVirtualHostMulti(0 To 0)
        Catch ex As Exception
        End Try
    End Sub

    'Make the a window on top of the main window
    Public Function SetTopMostWindow(hwnd As Long, Topmost As Boolean) As Long

        'Set the passed window to be onto of ALL other windows (system wide)

        If Topmost = True Then
            SetTopMostWindow = SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
        Else
            SetTopMostWindow = SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, FLAGS)
            SetTopMostWindow = False
        End If
    End Function

    'Build and load the httpd.conf file for output from the system and user defined values.
    Public Function loadHttpdFromProgramConfig()

        'Function builds the httpd.conf text from the defined values.  Output goes to rtxtHttpdConf (httpdconf page)

        Try
            SetText_ObjectTEXT("ServerRoot " & strServerRoot & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf)

            If boolVirtualHostIPBased = False Then
                If boolVirtualHostNameBased_Single = True Then
                    For intI = LBound(strNamedVirtualHostMulti) To UBound(strNamedVirtualHostMulti)
                        If Not strNamedVirtualHostMulti(intI) = "" Then
                            SetText_ObjectTEXT(strNamedVirtualHostMulti(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                        End If
                    Next intI
                End If

                SetText_ObjectTEXT("Listen " & strListen & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)
            Else
                For intI = LBound(strListenMulti) To UBound(strListenMulti)
                    If Not strListenMulti(intI) = "" Then
                        SetText_ObjectTEXT(strListenMulti(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                    End If
                Next intI
            End If

            SetText_ObjectTEXT("Include " & strInclude & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)
            SetText_ObjectTEXT("User " & strUser & vbCrLf, httpdconf.rtxtHttpdconf, True)
            SetText_ObjectTEXT("Group " & strGroup & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)


            If boolVirtualHostsAdded = True Then
                For intI = LBound(strCont_Virtualhost) To UBound(strCont_Virtualhost)
                    SetText_ObjectTEXT(strCont_Virtualhost(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI
                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)

                For intI = LBound(strCont_DirectoryRoot) To UBound(strCont_DirectoryRoot)
                    SetText_ObjectTEXT(strCont_DirectoryRoot(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI

                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
                For intI = LBound(strCont_IfMod_dirmod) To UBound(strCont_IfMod_dirmod)
                    SetText_ObjectTEXT(strCont_IfMod_dirmod(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI

                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
                For intI = LBound(strCont_Files_ht) To UBound(strCont_Files_ht)
                    SetText_ObjectTEXT(strCont_Files_ht(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI

            Else
                SetText_ObjectTEXT("ServerAdmin " & strServerAdmin & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)

                If strServerName = "" Then
                    SetText_ObjectTEXT("#ServerName " & strServerName & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Else
                    SetText_ObjectTEXT("ServerName " & strServerName & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)
                End If

                For intI = LBound(strCont_DirectoryRoot) To UBound(strCont_DirectoryRoot)
                    SetText_ObjectTEXT(strCont_DirectoryRoot(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI
                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
                SetText_ObjectTEXT("DocumentRoot " & strDocumentRoot & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)

                For intI = LBound(strCont_Directory) To UBound(strCont_Directory)
                    SetText_ObjectTEXT(strCont_Directory(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI

                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
                For intI = LBound(strCont_IfMod_dirmod) To UBound(strCont_IfMod_dirmod)
                    SetText_ObjectTEXT(strCont_IfMod_dirmod(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI

                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
                For intI = LBound(strCont_Files_ht) To UBound(strCont_Files_ht)
                    SetText_ObjectTEXT(strCont_Files_ht(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
                Next intI

                SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
                SetText_ObjectTEXT("ErrorLog " & strErrorLog & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)
                SetText_ObjectTEXT("LogLevel " & strLoglevel & vbCrLf & vbCrLf, httpdconf.rtxtHttpdconf, True)
            End If

            SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
            For intI = LBound(strCont_IfMod_mimemod) To UBound(strCont_IfMod_mimemod)
                SetText_ObjectTEXT(strCont_IfMod_mimemod(intI) & vbCrLf, httpdconf.rtxtHttpdconf, True)
            Next intI

            SetText_ObjectTEXT("" & vbCrLf, httpdconf.rtxtHttpdconf, True)
            SetText_ObjectTEXT("IncludeOptional " & strIncludeOptional, httpdconf.rtxtHttpdconf, True)
        Catch ex As Exception
            strErrorReturn = ex.Message
            Return False
        End Try

        Return True
    End Function

    'Prepare the SSH session
    Public Sub PrepareConnection()

        'Set the params for the SSH session.

        'No params - Abort the connection attempt
        If main.txtHostAddress.Text = "" Or main.txtUsername.Text = "" Or main.txtPassword.Text = "" Then
            SetText_rtxtTerminal(vbCrLf & " - Fail: Enter all connection details" & vbCrLf)
            Exit Sub
        End If

        'Start the timer for graphical output 
        main.TimerSSHStatus.Start()

        strHostname = main.txtHostAddress.Text
        strUsername = main.txtUsername.Text
        strPassword = main.txtPassword.Text

        'Start a background thread (doConnectSSH())
        If Not main.BWdoConnectSSH.IsBusy Then
            SetText_rtxtTerminal(" - Connecting to " & strUsername & "@" & strHostname & " ")
            main.BWdoConnectSSH.RunWorkerAsync()
        End If

    End Sub

    'Connect the SSH session
    Public Sub doConnectSSH()

        'Construct the sshClient - Connect return results (via intSSHStatus to terminal output)

        Try
            intSSHStatus = 0
            sshConnectionDetails = New PasswordConnectionInfo(strHostname, strUsername, strPassword)
            sshClient = New SshClient(sshConnectionDetails)
            sshClient.KeepAliveInterval = New TimeSpan(0, 0, 5)
            sshClient.Connect()
        Catch ex As Net.Sockets.SocketException
            intSSHStatus = 98
            strErrorReturn = " - " & ex.Message & vbCrLf
        Catch ex As Renci.SshNet.Common.SshAuthenticationException
            intSSHStatus = 98
            strErrorReturn = " - " & ex.Message & vbCrLf
        Catch ex As ArgumentException
            intSSHStatus = 98
            strErrorReturn = " - " & ex.Message & vbCrLf
        Catch ex As Exception
            intSSHStatus = 98
            strErrorReturn = " - " & ex.Message & vbCrLf
        End Try
    End Sub

    'Send and receive the SSH command and response - Return output
    Public Function doCommandSSH(ByVal strCommand_term As String) As String

        'Run the command via sshClient.RunCommand - return & retrieve(variable store) the command output

        Try
            sshCommand = sshClient.RunCommand(strCommand_term)

            'Retrieve the command output
            strCommandResult = sshCommand.Result.ToString
        Catch ex As Renci.SshNet.Common.SshOperationTimeoutException
            intSSHStatus = 98
            strErrorReturn = " - SSH client timeout" & vbCrLf
        Catch ex As Common.SshConnectionException
            intSSHStatus = 98
            strErrorReturn = " - ERROR SSH " & ex.Message
        Catch ex As Exception
            strErrorReturn = " - ERROR " & ex.Message
        End Try

        'Retrun the command output
        Return strCommandResult
    End Function

    'The response output from doCommandSSH().  With the reply, configure terminal reply and user options.
    Public Sub CommandResultProc()

        'From the results of the command, direct flow.

        If strSentCmd = "version" Then
            If strCommandResult.Contains("version:") Then
                SetText_ObjectTEXT(strCommandResult, main.lblHTTPDVersion)
                main.PBHTTPDVersion.Image = My.Resources.Ok48
                main.PBServiceVerToggle.Image = My.Resources.ToggleOn48
                main.PBCommandStatus.Image = Nothing
                strSentCmd = ""

                If boolWizardActive = True Then
                    Wizard.lblCmdOut.Text = "Version - OK"
                End If

                If strCommandResult.Contains("running") Then
                    SetText_ObjectTEXT("HTTPD Service - Active", main.lblServiceStarted)
                    main.PBServiceStatus.Image = My.Resources.Ok48
                    main.PBServiceStatToggle.Image = My.Resources.ToggleOn48
                    If boolWizardActive = True Then
                        Wizard.lblCmdOut.Text = Wizard.lblCmdOut.Text & vbCrLf & "Service - Running"
                    End If
                End If
            Else
                SetText_ObjectTEXT("Service Version - Unable to find HTTPD", main.lblHTTPDVersion)
                main.PBHTTPDVersion.Image = My.Resources.Fail48
                main.PBServiceVerToggle.Image = My.Resources.ToggleOff48
                If boolWizardActive = True Then
                    Wizard.lblCmdOut.Text = "Version - Could Not find the service"
                End If
                Dim msgres As MsgBoxResult = MsgBox("No HTTPD service detected.  Download And install from repo now?" & vbCrLf & vbCrLf & " # yum install httpd -y", MsgBoxStyle.YesNo, "Install Service")
                If msgres = MsgBoxResult.Yes Then
                    MsgBox("Ok.  About to send command - click ok And wait for output", MsgBoxStyle.Information, "Continue")

                    main.doSend("yum install httpd -y && httpd -v | head -1")
                    strSentCmd = "yuminstall"
                Else
                    main.PBCommandStatus.Image = Nothing
                    strSentCmd = ""
                End If
            End If

        ElseIf strSentCmd = "yuminstall" Then
            If strCommandResult.Contains("Installed") Then
                main.PBHTTPDVersion.Image = My.Resources.Ok48
                main.PBServiceVerToggle.Image = My.Resources.ToggleOn48
                If strCommand.Contains("version:") Then
                    SetText_ObjectTEXT(strCommandResult, main.lblHTTPDVersion)
                    main.PBHTTPDVersion.Image = My.Resources.Ok48
                    main.PBServiceVerToggle.Image = My.Resources.ToggleOn48
                    main.PBCommandStatus.Image = Nothing
                    strSentCmd = ""
                End If
            Else
                main.PBHTTPDVersion.Image = My.Resources.Fail48
                main.PBServiceVerToggle.Image = My.Resources.ToggleOff48
            End If
            main.PBCommandStatus.Image = Nothing
            strSentCmd = ""
        ElseIf strSentCmd = "cathosts" Then
            If Not strCommandResult = "" Then
                Wizard.rtxtCmdOut.Text = strCommandResult
            End If
            main.PBCommandStatus.Image = Nothing
            strSentCmd = ""
        ElseIf strSentCmd = "logs" Then
            If Not strCommandResult = "" Then
                SetText_ObjectTEXT(strCommandResult, main.rtxtLogs)
            End If
            main.PBCommandStatus.Image = Nothing
            strSentCmd = ""
        ElseIf strSentCmd = "service" Then
            If strCommandResult.Contains("running") Then
                SetText_ObjectTEXT("HTTPD Service - Active", main.lblServiceStarted)
                main.PBServiceStatus.Image = My.Resources.Ok48
                main.PBServiceStatToggle.Image = My.Resources.ToggleOn48
            Else
                SetText_ObjectTEXT("HTTPD Service - In-Active", main.lblServiceStarted)
                main.PBServiceStatus.Image = My.Resources.Fail48
                main.PBServiceStatToggle.Image = My.Resources.ToggleOff48
            End If
            main.PBCommandStatus.Image = Nothing
            strSentCmd = ""
        ElseIf strSentCmd = "firewalldstatus" Then
            If strCommandResult.Contains("running") Then
                main.lblFirewall_Dstatus.ForeColor = Color.LimeGreen
                SetText_ObjectTEXT("ACTIVE", main.lblFirewall_Dstatus)
            Else
                main.lblFirewall_Dstatus.ForeColor = Color.Crimson
                SetText_ObjectTEXT("IN-ACTIVE", main.lblFirewall_Dstatus)
            End If
            main.doSend("systemctl status iptables")
            strSentCmd = "firewalldstatus2"
        ElseIf strSentCmd = "firewalldstatus2" Then
            If strCommandResult.Contains("running") Then
                main.lblFirewall_Iptables.ForeColor = Color.LimeGreen
                SetText_ObjectTEXT("ACTIVE", main.lblFirewall_Iptables)
            Else
                main.lblFirewall_Iptables.ForeColor = Color.Crimson
                SetText_ObjectTEXT("IN-ACTIVE", main.lblFirewall_Iptables)
            End If
            main.PBCommandStatus.Image = Nothing
            strSentCmd = ""
        ElseIf strSentCmd = "cathttpdconf" Then
            If Not strCommandResult = "" Then
                SetText_ObjectTEXT(strCommandResult, httpdconf.rtxtHttpdconf)
            End If
            main.PBCommandStatus.Image = Nothing
            strSentCmd = ""
        ElseIf strSentCmd = "apachectlconfigtest" Then
            main.doSend("cat /tmp/apache_configtest")
            strSentCmd = "apachectlconfigtest2"
        ElseIf strSentCmd = "apachectlconfigtest2" Then
            main.PBCommandStatus.Image = Nothing
            If Not strCommandResult = "" Then
                If strCommandResult.Contains("error") Or strCommandResult.Contains("No such file") Then
                    MsgBox("Apachectl configtest reported an error:" & vbCrLf & strCommandResult, MsgBoxStyle.Critical, "Apachectl Configtest: Error")
                Else
                    MsgBox("Apachectl configtest - OK", MsgBoxStyle.Information, "Apachectl Configtest: Good")
                End If
            End If
            strSentCmd = ""
        ElseIf strSentCmd = "directives" Then
            If Not strCommandResult = "" Then
                main.txtServerName.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^ServerAdmin"" | awk '{print $2}'")
            strSentCmd = "directives_1"
        ElseIf strSentCmd = "directives_1" Then
            If Not strCommandResult = "" Then
                main.txtServerAdmin.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^Listen"" | awk '{print $2}'")
            strSentCmd = "directives_2"
        ElseIf strSentCmd = "directives_2" Then
            If Not strCommandResult = "" Then
                main.txtListen.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^DocumentRoot"" | awk '{print $2}'")
            strSentCmd = "directives_3"
        ElseIf strSentCmd = "directives_3" Then
            If Not strCommandResult = "" Then
                main.txtDocumentRoot.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^ServerRoot"" | awk '{print $2}'")
            strSentCmd = "directives_31"
        ElseIf strSentCmd = "directives_31" Then
            If Not strCommandResult = "" Then
                main.txtServerRoot.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^Include"" | awk '{print $2}'")
            strSentCmd = "directives_4"
        ElseIf strSentCmd = "directives_4" Then
            If Not strCommandResult = "" Then
                main.txtInclude.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^IncludeOptional"" | awk '{print $2}'")
            strSentCmd = "directives_41"
        ElseIf strSentCmd = "directives_41" Then
            If Not strCommandResult = "" Then
                main.txtIncludeOptional.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^User"" | awk '{print $2}'")
            strSentCmd = "directives_5"
        ElseIf strSentCmd = "directives_5" Then
            If Not strCommandResult = "" Then
                main.txtUser.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^Group"" | awk '{print $2}'")
            strSentCmd = "directives_6"
        ElseIf strSentCmd = "directives_6" Then
            If Not strCommandResult = "" Then
                main.txtGroup.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^ErrorLog"" | awk '{print $2}'")
            strSentCmd = "directives_7"
        ElseIf strSentCmd = "directives_7" Then
            If Not strCommandResult = "" Then
                main.txtErrorLog.Text = strCommandResult
            End If
            main.doSend("cat /etc/httpd/conf/httpd.conf | grep -w ""^LogLevel"" | awk '{print $2}'")
            strSentCmd = "directives_8"
        ElseIf strSentCmd = "directives_8" Then
            If Not strCommandResult = "" Then
                main.txtLogLevel.Text = strCommandResult
            End If
            main.PBCommandStatus.Image = Nothing
        Else
            main.PBCommandStatus.Image = Nothing
        End If

        'Errors?  Display the error message (ex.message)
        If Not strErrorReturn = "" Then
            SetText_rtxtTerminal(strErrorReturn)
            strErrorReturn = ""
        End If

    End Sub

    'Export/Save/Create a file (httpd.conf/hosts/etc).  Option to send that file to the server (sendFiletoServer()).
    Public Function exporttoFile(ByVal Optional boolSendtoServer As Boolean = False, Optional ByVal strFile As String = "", Optional boolFromLogTab As Boolean = False, Optional strFileName As String = "")

        'Build and save to file

        Try
            Dim confFile As New SaveFileDialog()

            If strFile = "" Then
                'The file is httpd.conf
                confFile.DefaultExt = "*.conf"
                confFile.FileName = "httpd.conf"
                confFile.Filter = "Conf Files|*.conf"
            Else
                'The file is not httpd.conf
                confFile.FileName = strFileName
            End If

            If (confFile.ShowDialog() = System.Windows.Forms.DialogResult.OK) And (confFile.FileName.Length) > 0 Then
                If boolFromLogTab = False Then
                    httpdconf.rtxtHttpdconf.SaveFile(confFile.FileName, RichTextBoxStreamType.PlainText)
                Else
                    main.rtxtLogs.SaveFile(confFile.FileName, RichTextBoxStreamType.PlainText)
                End If
            Else
                MsgBox("Aborted the operation", MsgBoxStyle.Information, "Abort")
                Return False
            End If

            'Optional boolSendtoServer = True - parse control to SendFiletoServer() via thread contructor
            If boolSendtoServer = True Then
                Dim Thread_ConnectSSH As Thread
                If strFile = "" Then
                    SetText_rtxtTerminal(" - transfering file " & confFile.FileName & " to /etc/httpd/conf/httpd.conf")
                    Thread_ConnectSSH = New Thread(New ThreadStart(Function() SendFiletoServer(confFile.FileName, "httpd.conf", "/etc/httpd/conf")))
                Else
                    SetText_rtxtTerminal(" - transfering file " & confFile.FileName & " to " & strFile & "/" & strFileName)
                    Thread_ConnectSSH = New Thread(New ThreadStart(Function() SendFiletoServer(confFile.FileName, strFileName, strFile, True)))
                End If
                'Launch the thread
                Thread_ConnectSSH.SetApartmentState(ApartmentState.STA)
                Thread_ConnectSSH.Start()
            End If

        Catch ex As Exception
            'Error out
            strErrorReturn = ex.Message
            Return False
        End Try
        Return True
    End Function

    'Send the passed file to the SSH server via SFTP.  Build a filestream, send, close, check success.
    Public Function SendFiletoServer(ByVal strLocalFile As String, ByVal strRemoteFileName As String, ByVal strDestDir As String, Optional ByVal boolFromLog As Boolean = False)
        Try

            'Send the file to the connected SSH server via sftp (Renci.SSH Class)

            strErrorReturn = ""
            main.PBCommandStatus.Image = My.Resources.loadcirc
            main.Cursor = Cursors.AppStarting

            If boolFromLog = True Then
                boolCopyFromLog = True
            Else
                boolCopyFromLog = False
            End If

            boolFileCopy = True 'A copy operation has started

            Using sftp As New SftpClient(strHostname, 22, strUsername, strPassword)

                'Connect to the SSH Server
                sftp.Connect()

                Thread.Sleep(500)

                'Change the directory for the destination httpd.conf file
                sftp.ChangeDirectory(strDestDir)

                Thread.Sleep(500)

                'Build the local httpd.conf file into a filestream & upload the file
                Dim fileStream As IO.FileStream = IO.File.OpenRead(strLocalFile)
                sftp.UploadFile(fileStream, strRemoteFileName, True)

                Thread.Sleep(500)

                'Does the new file exist?  Copy success or failure.
                boolExists = sftp.Exists(strDestDir & "/" & strRemoteFileName)

                'Disconnect the sftp session & close the filestream.
                sftp.Disconnect()
                fileStream.Close()
            End Using

        Catch ex As Exception
            'Errors
            strErrorReturn = " - " & ex.Message & vbCrLf
            'Return intSSHStatus or 5000 (tell timerSSHStatus to output the graphic to the terminal window)
            intSSHStatus = 5000
            Return False
        End Try
        'Success
        main.PBCommandStatus.Image = Nothing
        'Return intSSHStatus or 5000 (tell timerSSHStatus to output the graphic to the terminal window)
        intSSHStatus = 5000
        Return True
    End Function

#End Region


End Module
