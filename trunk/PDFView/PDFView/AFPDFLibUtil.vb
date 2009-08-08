Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Printing

Public Class AFPDFLibUtil

  'This uses an XPDF wrapper written by Jose Antonio Sandoval Soria of Guadalajara, México
  'The source is available at http://www.codeproject.com/KB/files/xpdf_csharp.aspx
  '
  'I have ported over to VB.NET select functionality from the C# PDF viewer in the above project

  Const RENDER_DPI As Integer = 200
  Const PRINT_DPI As Integer = 300

  Public Shared Sub DrawImageFromPDF(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByVal PageNumber As Integer, ByRef oPictureBox As PictureBox, Optional ByVal DPI As Integer = RENDER_DPI)
    Try
      If pdfDoc IsNot Nothing Then
        pdfDoc.CurrentPage = PageNumber
        pdfDoc.CurrentX = 0
        pdfDoc.CurrentY = 0
        pdfDoc.RenderDPI = DPI
        pdfDoc.RenderPage(oPictureBox.Handle)
        oPictureBox.Image = Render(pdfDoc, oPictureBox)

        oPictureBox.Refresh()
      End If
    Catch ex As Exception
      Throw ex
    End Try
  End Sub

  Public Shared Function Render(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByRef oPictureBox As PictureBox) As System.Drawing.Bitmap
    Try
      If pdfDoc IsNot Nothing Then
        Dim backbuffer As System.Drawing.Bitmap = New Bitmap(pdfDoc.PageWidth, pdfDoc.PageHeight)
        pdfDoc.ClientBounds = New Rectangle(0, 0, pdfDoc.PageWidth, pdfDoc.PageHeight)
        Dim g As Graphics = Graphics.FromImage(backbuffer)
        Using g
          Dim hdc As IntPtr = g.GetHdc()
          pdfDoc.DrawPageHDC(hdc)
          g.ReleaseHdc()
        End Using
        g.Dispose()
        Return backbuffer
      End If
    Catch ex As Exception
      Throw ex
      Return Nothing
    End Try
    Return Nothing
  End Function
  Public Shared Sub ExportPDF(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByVal fileName As String, Optional ByVal startPage As Integer = 1, Optional ByVal endPage As Integer = 0)
    If Not Nothing Is pdfDoc Then
      If endPage = 0 Or endPage > pdfDoc.PageCount Then
        endPage = pdfDoc.PageCount
      End If
      Try
        If fileName.EndsWith(".ps") Then
          pdfDoc.PrintToFile(fileName, startPage, endPage)
        ElseIf fileName.EndsWith(".jpg") Then
          pdfDoc.ExportJpg(fileName, 70)
        ElseIf fileName.EndsWith(".txt") Then
          pdfDoc.ExportText(fileName, startPage, endPage, True, True)
        ElseIf fileName.EndsWith(".html") Then
          pdfDoc.ExportHtml(fileName, startPage, endPage, True, True, False)
        End If
      Catch ex As Exception
        MessageBox.Show(ex.ToString())
      End Try
    End If
  End Sub

  Public Shared Function FillTree(ByRef tvwOutline As TreeView, ByRef pdfDoc As PDFLibNet.PDFWrapper) As Boolean
    FillTree = False
    tvwOutline.Nodes.Clear()
    For Each ol As PDFLibNet.OutlineItem In pdfDoc.Outline
      Dim tn As New TreeNode(ol.Title)
      tn.Tag = ol
      If ol.KidsCount > 0 Then
        tn.Nodes.Add(New TreeNode("dummy"))
      End If
      tvwOutline.Nodes.Add(tn)
      FillTree = True
    Next
  End Function

End Class

Public Class PDFOutline

  Public Title As String
  Public Item As PDFLibNet.OutlineItem
  Friend _doc As PDFLibNet.PDFWrapper = Nothing
  Friend _children As List(Of PDFOutline)

  Public ReadOnly Property Children() As List(Of PDFOutline)
    Get
      Return _children
    End Get
  End Property

  Friend Sub New(ByVal title__1 As String, ByVal outlineItem As PDFLibNet.OutlineItem, ByVal doc As PDFLibNet.PDFWrapper)
    Title = title__1
    Item = outlineItem
    _doc = doc
  End Sub

End Class

Public Class SearchArgs
  Inherits EventArgs
  Public Text As String
  Public FromBegin As Boolean
  Public Exact As Boolean
  Public WholeDoc As Boolean
  Public FindNext As Boolean
  Public Up As Boolean

  Friend Sub New(ByVal text__1 As String, ByVal frombegin__2 As Boolean, ByVal exact__3 As Boolean, ByVal wholedoc__4 As Boolean, ByVal findnext__5 As Boolean, ByVal up__6 As Boolean)
    Text = text__1
    FromBegin = frombegin__2
    Exact = exact__3
    WholeDoc = wholedoc__4
    FindNext = findnext__5
    Up = up__6
  End Sub
End Class


