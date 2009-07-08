Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Printing

Public Class AFPDFLibUtil

    Const RENDER_DPI As Integer = 180
    Const PRINT_DPI As Integer = 300

    Public Shared Sub DrawImageFromPDF(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByVal PageNumber As Integer, ByRef oPictureBox As PictureBox, Optional ByVal DPI As Integer = RENDER_DPI)
        If pdfDoc IsNot Nothing Then
            pdfDoc.CurrentPage = PageNumber
            pdfDoc.CurrentX = 0
            pdfDoc.CurrentY = 0
            pdfDoc.RenderDPI = DPI
            pdfDoc.RenderPage(oPictureBox.Handle.ToInt32())
            oPictureBox.Image = Render(pdfDoc)
        End If
    End Sub

    Public Shared Function Render(ByRef pdfDoc As PDFLibNet.PDFWrapper) As Bitmap
        If pdfDoc IsNot Nothing Then
            Dim backbuffer As New Bitmap(pdfDoc.PageWidth, pdfDoc.PageHeight)
            Dim g As Graphics = Graphics.FromImage(backbuffer)
            Using g
                Dim lhdc As Integer = g.GetHdc().ToInt32()
                pdfDoc.DrawPageHDC(lhdc)
                g.ReleaseHdc()
            End Using
            g.Dispose()
            Return backbuffer
        End If
        Return Nothing
    End Function

    Public Shared Sub PrintPDFImagesToPrinter(ByRef pdfDoc As PDFLibNet.PDFWrapper, ByRef picbox As PictureBox)
        If pdfDoc IsNot Nothing Then
            Dim PD As New PrintDialog
            Dim PageCount As Integer = pdfDoc.PageCount
            PD.AllowSomePages = True
            PD.PrinterSettings.FromPage = 1
            PD.PrinterSettings.ToPage = PageCount
            PD.PrinterSettings.MaximumPage = PageCount
            PD.PrinterSettings.MinimumPage = 1
            If PD.ShowDialog = DialogResult.OK Then
                Dim BeginningPage As Integer = 1
                Dim EndingPage As Integer = PageCount
                If PD.PrinterSettings.PrintRange = PrintRange.SomePages Then
                    BeginningPage = PD.PrinterSettings.FromPage
                    EndingPage = PD.PrinterSettings.ToPage
                End If
                For i As Integer = (BeginningPage - 1) To (EndingPage - 1)
                    pdfDoc.CurrentPage = i + 1
                    pdfDoc.CurrentX = 0
                    pdfDoc.CurrentY = 0
                    pdfDoc.RenderDPI = PRINT_DPI
                    pdfDoc.RenderPage(picbox.Handle.ToInt32())
                    PrinterUtil.PrintImageToPrinter(ImageUtil.CropBitmap(Render(pdfDoc), 0, 0, pdfDoc.PageWidth, pdfDoc.PageHeight - 2), PD.PrinterSettings)
                    pdfDoc.RenderDPI = RENDER_DPI
                Next
            End If
        End If
    End Sub

    'Public Shared Function LoadBookmarks(ByRef tvwOutline As TreeView, ByRef pdfDoc As PDFLibNet.PDFWrapper) As Boolean
    '    Dim ol As PDFLibNet.OutlineItem = Nothing
    '    Dim outline As New List(Of PDFOutline)()
    '    If pdfDoc IsNot Nothing AndAlso pdfDoc.OutlineCount > 0 Then
    '        tvwOutline.SuspendLayout()
    '        For i As Integer = 1 To pdfDoc.OutlineCount
    '            Dim lPtr As Integer = pdfDoc.GetOutlinePtr(i)
    '            ol = New PDFLibNet.OutlineItem()
    '            ol.SetOutlineItemXPDF(lPtr)
    '            Dim pdo As New PDFOutline(ol.Title, ol, pdfDoc)
    '            outline.Add(pdo)
    '        Next
    '        tvwOutline.ResumeLayout()
    '    End If
    '    FillTree(tvwOutline, outline)
    '    If outline.Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

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

    'Friend Sub LoadChildren()
    '    If _children Is Nothing Then
    '        _children = New List(Of PDFOutline)()
    '        For i As Integer = 1 To Item.KidsCount
    '            Dim lPtr As Integer = Item.GetKidPtr(i)
    '            If lPtr > 0 Then
    '                Dim ol As New PDFLibNet.OutlineItem()
    '                ol.SetOutlineItemXPDF(lPtr)
    '                Dim pdo As New PDFOutline(ol.Title, ol, _doc)
    '                _children.Add(pdo)
    '            End If
    '        Next
    '    End If
    'End Sub

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


