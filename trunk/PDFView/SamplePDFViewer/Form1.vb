Public Class Form1

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        PdfViewer1.Dispose() 'Necessary to delete the Tiff file used in the Temp directory if LoadAllPages = True
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "PDF files (*.pdf)|*.pdf"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()
        TextBox1.Text = OpenFileDialog1.FileName
        PdfViewer1.LoadAllPages = False
        PdfViewer1.AllowBookmarks = True
        ' To use Ghostscript, UseXPDF = False
        ' Ghostscript is slower, but is more compatible and has higher quality rendering
        ' To use XPDF, UseXPDF = True
        ' XPDF is quite a bit faster than Ghostscript since there is no file i/o involved
        ' XPDF requires AFPDFLib.dll to be registered with COM
        ' regsrv32 AFPDFLib.dll
        'PdfViewer1.UseXPDF = True
        PdfViewer1.FileName = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PDFView.PrinterUtil.PrintPDFImagesToPrinter(TextBox1.Text)
    End Sub

    Private Sub rbXPDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbXPDF.CheckedChanged
        If rbXPDF.Checked Then
            PdfViewer1.UseXPDF = True
        Else
            PdfViewer1.UseXPDF = False
        End If
        PdfViewer1.FileName = OpenFileDialog1.FileName
    End Sub
End Class
