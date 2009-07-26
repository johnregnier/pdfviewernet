Imports System.Drawing

Public Class TesseractOCR

  Public Shared Function OCRImage(ByVal imgImage As System.Drawing.Image) As String
    OCRImage = ""
    Dim oOCR As New tessnet2.Tesseract
    Dim bitmap As New Bitmap(imgImage)
    bitmap = ImageUtil.BitmapTo1Bpp(bitmap)
    oOCR.Init("eng", False, False)
    Dim WordList As New List(Of tessnet2.Word)
    WordList = oOCR.doOCR(bitmap, Rectangle.Empty)
    bitmap.Dispose()
    Dim LineCount As Integer = tessnet2.Tesseract.LineCount(WordList)
    For i As Integer = 0 To LineCount - 1
      OCRImage &= tessnet2.Tesseract.GetLineText(WordList, i) & vbCrLf
    Next
    oOCR.Dispose()
  End Function

End Class
