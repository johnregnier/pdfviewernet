Public Class ImportOptions

  Public FileName As String = ""

  Private Sub CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbURL.CheckedChanged, rbHtml.CheckedChanged, rbImages.CheckedChanged
    If rbURL.Checked Then
      GroupBox2.Visible = True
    Else
      GroupBox2.Visible = False
    End If
  End Sub

  Private Sub btImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btImport.Click
    Me.Cursor = Windows.Forms.Cursors.WaitCursor
    Windows.Forms.Application.DoEvents()
    If rbURL.Checked And tbUrl.Text <> "" Then
      SaveFileDialog1.Filter = "Portable Document Format (*.pdf)|*.pdf"
      SaveFileDialog1.FileName = ""
      If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) And SaveFileDialog1.FileName <> "" Then
        Dim htmlConverter As New HTML2PDF
        If htmlConverter.URLToPDF(tbUrl.Text, SaveFileDialog1.FileName) Then
          FileName = SaveFileDialog1.FileName
        Else
          MsgBox("URL to PDF conversion failed.", MsgBoxStyle.Critical, "PDF import failed")
        End If
      End If
    End If

    If rbHtml.Checked Then
      OpenFileDialog1.Filter = "Html Files(*.html;*.htm)|*.html;*.htm"
      OpenFileDialog1.FileName = ""
      If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK And OpenFileDialog1.FileName <> "" Then
        SaveFileDialog1.Filter = "Portable Document Format (*.pdf)|*.pdf"
        SaveFileDialog1.FileName = ""
        If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) And SaveFileDialog1.FileName <> "" Then
          Dim htmlConverter As New HTML2PDF
          If htmlConverter.HTMLToPDF(OpenFileDialog1.FileName, SaveFileDialog1.FileName) Then
            FileName = SaveFileDialog1.FileName
          Else
            MsgBox("HTML to PDF conversion failed.", MsgBoxStyle.Critical, "PDF import failed")
          End If
        End If
      End If
    End If

    If rbImages.Checked Then
      ConvertGraphicsToPDF()
    End If

    Me.Cursor = Windows.Forms.Cursors.Default

    Me.Hide()

  End Sub

  Private Sub ImportOptions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    GroupBox2.Visible = False
    rbImages.Checked = True
  End Sub

  Private Sub ConvertGraphicsToPDF()
    OpenFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.TIF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF"
    OpenFileDialog1.FileName = ""
    OpenFileDialog1.Title = "Select multiple image files to convert to PDF"
    OpenFileDialog1.Multiselect = True
    If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
      Dim exportOptionsDialog As New ExportImageOptions(OpenFileDialog1.FileNames)
      exportOptionsDialog.ShowDialog()
      Try
        FileName = exportOptionsDialog.SavedFileName
      Catch ex As Exception
        'do nothing
      End Try
    End If

  End Sub

End Class