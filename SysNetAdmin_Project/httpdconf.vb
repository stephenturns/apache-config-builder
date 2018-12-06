Imports System.Threading
Public Class httpdconf

#Region "Form Handlers"
    Private Sub httpdconf_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'On window load, build and display the contents for the proposed httpd.conf file - Display to text
        loadHttpdFromProgramConfig()
    End Sub
#End Region

#Region "Menu System"

    'User driven menu clicks

    Private Sub LoadFromProgramConfigToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadFromProgramConfigToolStripMenuItem.Click
        If loadHttpdFromProgramConfig() = False Then

            MsgBox("There has been a problem retrieving the program values." & vbCrLf & vbCrLf & "Problem: " & strErrorReturn, MsgBoxStyle.Exclamation, "Load values - Error")
            strErrorReturn = ""
        End If
    End Sub
    Private Sub PullFromSSHServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PullFromSSHServerToolStripMenuItem.Click
        If boolSSHConnection = True Then
            main.doSend("cat /etc/httpd/conf/httpd.conf")
            strSentCmd = "cathttpdconf"
        Else
            MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
        End If
    End Sub
    Private Sub ImportFromLocalFilehttpdconfToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportFromLocalFilehttpdconfToolStripMenuItem.Click
        Dim confFile As New OpenFileDialog()

        confFile.DefaultExt = "*.conf"
        confFile.Filter = "Conf Files|*.conf"

        If (confFile.ShowDialog() = DialogResult.OK And confFile.FileName.Length > 0) Then
            rtxtHttpdconf.LoadFile(confFile.FileName, RichTextBoxStreamType.PlainText)
        End If
    End Sub
    Private Sub PushToSSHServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PushToSSHServerToolStripMenuItem.Click
        Dim msgres As MsgBoxResult = MsgBox("Pushing to the SSH Server will overwright /etc/httpd/conf/httpd.conf.  The config must be saved locally first." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
        If msgres = MsgBoxResult.Yes Then
            If boolSSHConnection = True Then
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf)
                doCommandSSH("cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "apachectl stop" & vbCrLf)
                doCommandSSH("apachectl stop")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "rm -f /etc/httpd/conf/httpd.conf" & vbCrLf)
                doCommandSSH("rm -f /etc/httpd/conf/httpd.conf")
                exporttoFile(True)
            Else
                MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
            End If

        End If
    End Sub
    Private Sub ExportToLocalFilehttpdconfToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportToLocalFilehttpdconfToolStripMenuItem.Click
        exporttoFile()
    End Sub
    Private Sub CommitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CommitToolStripMenuItem.Click
        Dim msgres As MsgBoxResult = MsgBox("Pushing to the SSH Server will overwright /etc/httpd/conf/httpd.conf.  The config must be saved locally first." & vbCrLf & vbCrLf & "The current file will be backed up prior with: " & vbCrLf & " # cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf & vbCrLf & "Push to SSH Server?", vbYesNo, "Push to SSH Server: Yes/No")
        If msgres = MsgBoxResult.Yes Then
            If boolSSHConnection = True Then
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back" & vbCrLf)
                doCommandSSH("cp /etc/httpd/conf/httpd.conf /etc/httpd/conf/httpd.conf.back")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "apachectl stop" & vbCrLf)
                doCommandSSH("apachectl stop")
                SetText_rtxtTerminal(strUsername & "@" & strHostname & " # " & "rm -f /etc/httpd/conf/httpd.conf" & vbCrLf)
                doCommandSSH("rm -f /etc/httpd/conf/httpd.conf")
                exporttoFile(True)
            Else
                MsgBox("Connect to server first, then try again.", MsgBoxStyle.Information, "No Connection")
            End If

        End If
    End Sub
#End Region

#Region "Subs/Functions"
    'exporttofile - exports/creats/saved file (httpd.conf ONLY)
    Public Function exporttoFile(ByVal Optional boolSendtoServer As Boolean = False)
        Try
            Dim confFile As New SaveFileDialog()

            confFile.DefaultExt = "*.conf"
            confFile.FileName = "httpd.conf"
            confFile.Filter = "Conf Files|*.conf"

            If (confFile.ShowDialog() = System.Windows.Forms.DialogResult.OK) And (confFile.FileName.Length) > 0 Then
                rtxtHttpdconf.SaveFile(confFile.FileName, RichTextBoxStreamType.PlainText)
            End If

            If boolSendtoServer = True Then
                SetText_rtxtTerminal(" - transfering file " & confFile.FileName & " to /etc/httpd/conf/httpd.conf")
                Dim Thread_ConnectSSH As Thread = New Thread(New ThreadStart(Function() SendFiletoServer(confFile.FileName, "httpd.conf", "/etc/httpd/conf")))
                Thread_ConnectSSH.SetApartmentState(ApartmentState.STA)
                Thread_ConnectSSH.Start()
            End If

        Catch ex As Exception
            strErrorReturn = ex.Message
            Return False
        End Try
        Return True
    End Function


#End Region

End Class