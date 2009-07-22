Public Class Password

    Public UserPassword As String = ""
    Public OwnerPassword As String = ""

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub UsernameTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UsernameTextBox.TextChanged
        UserPassword = UsernameTextBox.Text
    End Sub

    Private Sub PasswordTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PasswordTextBox.TextChanged
        OwnerPassword = PasswordTextBox.Text
    End Sub
End Class
