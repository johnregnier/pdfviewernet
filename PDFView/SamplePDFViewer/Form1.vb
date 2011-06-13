Public Class Form1

  Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    PdfViewer1.Dispose() 'Necessary to free resources used by the PDF Viewer
  End Sub

  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

  End Sub

  Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    PdfViewer1.SelectFile()
    SetupRenderer()
  End Sub

  Private Sub rbXPDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbXPDF.CheckedChanged, rbMuPDF.CheckedChanged
    SetupRenderer()
  End Sub

  Private Sub SetupRenderer()
    If rbXPDF.Checked Then
      PdfViewer1.UseXPDF = True
      PdfViewer1.UseMuPDF = False
    ElseIf rbMuPDF.Checked = True Then
      PdfViewer1.UseXPDF = True
      PdfViewer1.UseMuPDF = True
    Else
      PdfViewer1.UseXPDF = False
      PdfViewer1.UseMuPDF = False
    End If
    PdfViewer1.FileName = PdfViewer1.FileName
  End Sub

  Private Sub btOCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btOCR.Click
    'MsgBox(PdfViewer1.OCRCurrentPage)
    For i As Integer = 0 To 100
      PdfViewer1.FileName = "C:\Users\User1\Documents\Printer PDF Files\20091119_070133_924.pdf"
    Next
  End Sub

  Private Sub cbPre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPre.CheckedChanged
    PdfViewer1.AllowGhostScriptPreRendering = cbPre.Checked
  End Sub
End Class
