Public Class Form1

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        PdfViewer1.Dispose() 'Necessary to free resources used by the PDF Viewer
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PdfViewer1.SelectFile()
        TextBox1.Text = PdfViewer1.FileName
    End Sub

    Private Sub rbXPDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbXPDF.CheckedChanged
        If rbXPDF.Checked Then
            PdfViewer1.UseXPDF = True
        Else
            PdfViewer1.UseXPDF = False
        End If
        PdfViewer1.FileName = PdfViewer1.FileName
    End Sub

  Private Sub btOCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btOCR.Click
    MsgBox(PdfViewer1.OCRCurrentPage)
  End Sub

  Private Sub cbPre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPre.CheckedChanged
    PdfViewer1.AllowGhostScriptPreRendering = cbPre.Checked
  End Sub
End Class
