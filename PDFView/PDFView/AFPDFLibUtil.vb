Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Printing
Imports System.Text

Public Class AFPDFLibUtil

  'This uses an XPDF wrapper written by Jose Antonio Sandoval Soria of Guadalajara, México
  'The source is available at http://www.codeproject.com/KB/files/xpdf_csharp.aspx
  '
  'I have ported over to VB.NET select functionality from the C# PDF viewer in the above project

  Const RENDER_DPI As Integer = 200
  Const PRINT_DPI As Integer = 300

  Public Shared Function GetOptimalDPI(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByRef oPictureBox As PictureBox) As Integer
    GetOptimalDPI = 0
    If pdfDoc IsNot Nothing Then
      If pdfDoc.PageWidth > 0 And pdfDoc.PageHeight > 0 Then
        Dim DPIScalePercent As Single = 72 / pdfDoc.RenderDPI
        Dim picHeight As Integer = oPictureBox.Height
        Dim picWidth As Integer = oPictureBox.Width
        Dim docHeight As Integer = pdfDoc.PageHeight
        Dim docWidth As Integer = pdfDoc.PageWidth
        Dim dummyPicBox As New PictureBox
        dummyPicBox.Size = oPictureBox.Size
        If (picWidth > picHeight And docWidth < docHeight) Or (picWidth < picHeight And docWidth > docHeight) Then
          dummyPicBox.Width = picHeight
          dummyPicBox.Height = picWidth
        End If
        Dim HScale As Single = dummyPicBox.Width / (pdfDoc.PageWidth * DPIScalePercent)
        Dim VScale As Single = dummyPicBox.Height / (pdfDoc.PageHeight * DPIScalePercent)
        dummyPicBox.Dispose()
        If VScale > HScale Then
          GetOptimalDPI = Math.Floor(72 * HScale)
        Else
          GetOptimalDPI = Math.Floor(72 * VScale)
        End If
      End If
    End If
  End Function

  Public Shared Function GetImageFromPDF(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByVal PageNumber As Integer, ByVal DPI As Integer) As System.Drawing.Image
    GetImageFromPDF = Nothing
    Try
      If pdfDoc IsNot Nothing Then
        pdfDoc.CurrentPage = PageNumber
        pdfDoc.CurrentX = 0
        pdfDoc.CurrentY = 0
        If DPI < 1 Then DPI = RENDER_DPI
        pdfDoc.RenderDPI = DPI
        Dim oPictureBox As New PictureBox
        pdfDoc.RenderPage(oPictureBox.Handle)
        GetImageFromPDF = Render(pdfDoc)
        oPictureBox.Dispose()
      End If
    Catch ex As Exception
      Throw ex
    End Try
  End Function

  Public Shared Function Render(ByRef pdfDoc As PDFLibNet.PDFWrapper) As System.Drawing.Bitmap
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
          Dim myParams As New ExportHtmlParams
          myParams.JpegQuality = 90
          myParams.ImageExtension = "png"
          myParams.HtmlLinks = True
          pdfDoc.ExportHtml(fileName, startPage, endPage, myParams)
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
      FillTree = True
      Dim tn As New TreeNode(ol.Title)
      tn.Tag = ol
      tvwOutline.Nodes.Add(tn)
      If ol.KidsCount > 0 Then
        FillTreeRecursive(ol.Childrens, tn)
      End If
    Next
  End Function

  Public Shared Sub FillTreeRecursive(ByVal olParent As PDFLibNet.OutlineItemCollection(Of PDFLibNet.OutlineItem), ByVal treeNode As TreeNode)
    For Each ol As PDFLibNet.OutlineItem In olParent
      Dim tn As New TreeNode(ol.Title)
      tn.Tag = ol
      treeNode.Nodes.Add(tn)
      If ol.KidsCount > 0 Then
        FillTreeRecursive(ol.Childrens, tn)
      End If
    Next
  End Sub

  Public Shared Function BuildHTMLBookmarks(ByRef pdfDoc As PDFLibNet.PDFWrapper) As String

    Dim pageCount As Integer = pdfDoc.PageCount

    If pdfDoc.Outline.Count <= 0 Then
StartPageList:
      BuildHTMLBookmarks = "<!--PageNumberOnly--><ul>"
      'For i As Integer = 1 To pageCount
      '  BuildHTMLBookmarks &= "<li><a href=""javascript:changeImage('" & i & "')"">Page " & i & "</a></li>"
      'Next
      BuildHTMLBookmarks &= "</ul>"
      Exit Function
    Else
      BuildHTMLBookmarks = ""
      FillHTMLTreeRecursive(pdfDoc.Outline, BuildHTMLBookmarks, pdfDoc)
      If System.Text.RegularExpressions.Regex.IsMatch(BuildHTMLBookmarks, "\d") = False Then
        BuildHTMLBookmarks = ""
        GoTo StartPageList
      End If
      Exit Function
    End If

  End Function

  Public Shared Sub FillHTMLTreeRecursive(ByVal olParent As PDFLibNet.OutlineItemCollection(Of PDFLibNet.OutlineItem), ByRef htmlString As String, ByRef pdfDoc As PDFLibNet.PDFWrapper)
    htmlString &= "<ul>"
    For Each ol As PDFLibNet.OutlineItem In olParent
      htmlString &= "<li><a href=""javascript:changeImage('" & ol.Destination.Page & "')"">" & Web.HttpUtility.HtmlEncode(ol.Title) & "</a></li>"
      If ol.KidsCount > 0 Then
        FillHTMLTreeRecursive(ol.Childrens, htmlString, pdfDoc)
      End If
    Next
    htmlString &= "</ul>"
  End Sub

  Public Shared Function GetWords(ByRef pdfDoc As PDFLibNet.PDFWrapper) As DictionaryEntry
    Dim count As Long = 0
    Dim content As New StringBuilder()
    Dim contentPattern As String = "content[{0}]=[{1},'{2}',{3},{4},{5},{6}];"
    For Each item In pdfDoc.Pages
      For Each word In item.Value.WordList
        count += 1
        content.Append(String.Format(contentPattern, count - 1, item.Key, Replace(Replace(word.Word, "\", "\\"), "'", "\'"), word.Bounds.X, word.Bounds.Y, word.Bounds.Width, word.Bounds.Height) & vbCrLf)
      Next
    Next
    Return New DictionaryEntry(count - 1, content.ToString)
  End Function

  Public Shared Function GetPages(ByRef pdfDoc As PDFLibNet.PDFWrapper) As DictionaryEntry
    Dim content As New StringBuilder()
    Dim contentPattern As String = "pageContent[{0}]=[{1},'{2}'];"
    For Each item In pdfDoc.Pages
      content.Append(String.Format(contentPattern, item.Key - 1, item.Key, _
                                   Replace(Replace(Replace(Replace(Replace(Replace(item.Value.Text, ControlChars.Cr, " "), ControlChars.Lf, " "), ControlChars.NewLine, " "), ControlChars.FormFeed, " "), "\", "\\"), "'", "\'") _
                                   ) & vbCrLf)
    Next
    Return New DictionaryEntry(pdfDoc.PageCount - 1, content.ToString)
  End Function

  Public Shared Function GetHtmlLinks(ByRef pdfDoc As PDFLibNet.PDFWrapper) As DictionaryEntry
    Dim htmlLinkContent As New StringBuilder()
    Dim htmlPattern As String = "htmlLink[{0}]=[{1},'{2}',{3},{4},{5},{6}];{7}"
    Dim htmlCount As Integer = 0
    For Each item In pdfDoc.Pages
      If pdfDoc.GetLinks(item.Key).Count > 0 Then
        For Each link In pdfDoc.GetLinks(item.Key)
          Dim myRect As Rectangle = link.Bounds
          Dim myAction As PDFLibNet.PageLinkAction = link.Action
          If myAction.Kind = PDFLibNet.LinkActionKind.actionURI Then
            htmlCount += 1
            Dim myUrl As String = Replace(Replace(CType(myAction, PDFLibNet.PageLinkURI).URL(), "\", "\\"), "'", "\'")
            htmlLinkContent.Append(String.Format(htmlPattern, _
                                   htmlCount - 1, item.Key, _
                                   myUrl, _
                                   myRect.Left, myRect.Top, myRect.Right, myRect.Bottom, _
                                   vbCrLf))
          End If
        Next
      End If
    Next
    Return New DictionaryEntry(htmlCount - 1, htmlLinkContent.ToString)
  End Function

  Public Shared Function GetPageLinks(ByRef pdfDoc As PDFLibNet.PDFWrapper) As DictionaryEntry
    Dim pageLinkContent As New StringBuilder()
    Dim pagePattern As String = "pageLink[{0}]=[{1},{2},{3},{4},{5},{6}];{7}"
    Dim pageLinkCount As Integer = 0
    For Each item In pdfDoc.Pages
      If pdfDoc.GetLinks(item.Key).Count > 0 Then
        For Each link In pdfDoc.GetLinks(item.Key)
          Dim myRect As Rectangle = link.Bounds
          Dim myAction As PDFLibNet.PageLinkAction = link.Action
          If myAction.Kind = PDFLibNet.LinkActionKind.actionGoTo Then
            pageLinkCount += 1
            Dim myDest As PDFLibNet.LinkDest = CType(myAction, PDFLibNet.PageLinkGoTo).Destination
            pageLinkContent.Append(String.Format(pagePattern, _
                                   pageLinkCount - 1, item.Key, _
                                   myDest.Page(), _
                                   myRect.Left, myRect.Top, myRect.Right, myRect.Bottom, _
                                   vbCrLf))
          End If
        Next
      End If
    Next
    Return New DictionaryEntry(pageLinkCount - 1, pageLinkContent.ToString)
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


