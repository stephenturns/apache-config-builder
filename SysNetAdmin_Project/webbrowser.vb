Public Class webbrowser

    'Simple webbrowser to view the live offering from the web server.

    Private Sub webbrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txturl.Text = strHostname
        doBrowse(txturl.Text)
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        doBrowse(txturl.Text)
    End Sub

    Private Sub txturl_KeyDown(sender As Object, e As KeyEventArgs) Handles txturl.KeyDown
        If e.KeyCode = Keys.Return Then
            doBrowse(txturl.Text)
        End If
    End Sub

    Private Sub doBrowse(ByVal strUrl As String)
        Try
            WebBrowser1.Navigate(strUrl)
        Catch ex As Exception
            MsgBox("There was a problem launching the URL with the browser: " & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Browser Error")
        End Try

    End Sub


End Class