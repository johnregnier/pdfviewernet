Imports System.Drawing

Public Class TesseractOCR

  Public Shared Function OCRImage(ByVal bm As System.Drawing.Image) As String
    OCRImage = ""
    Dim oOCR As New tessnet2.Tesseract
    'Dim bm As New Bitmap(imgImage)
    'bm = ImageUtil.BitmapTo1Bpp(bm)
    oOCR.Init(Nothing, "eng", False)
    Dim WordList As New List(Of tessnet2.Word)
    WordList = oOCR.doOCR(bm, Rectangle.Empty)
    'bm.Dispose()
    Dim LineCount As Integer = tessnet2.Tesseract.LineCount(WordList)
    For i As Integer = 0 To LineCount - 1
      OCRImage &= tessnet2.Tesseract.GetLineText(WordList, i) & vbCrLf
    Next
    oOCR.Dispose()
  End Function

End Class
