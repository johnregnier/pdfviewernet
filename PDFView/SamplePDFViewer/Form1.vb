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
        If PdfViewer1.PageCount(OpenFileDialog1.FileName) < 10 Then 'load all pages into viewer at once if the file is small (under 10 pages)
            PdfViewer1.LoadAllPages = True
        Else
            PdfViewer1.LoadAllPages = False
        End If
        PdfViewer1.AllowBookmarks = True
        PdfViewer1.FileName = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PDFView.PrinterUtil.PrintPDFImagesToPrinter(TextBox1.Text)
    End Sub
End Class
