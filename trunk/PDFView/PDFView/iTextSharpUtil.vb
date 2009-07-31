Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Drawing.Imaging

Public Class iTextSharpUtil

  Public Shared mBookmarkList As New ArrayList

  Public Shared Function GetPDFPageCount(ByVal filepath As String) As Integer
    Dim oPdfReader As PdfReader = New PdfReader(filepath)
    Dim page_count As Integer = oPdfReader.NumberOfPages
    oPdfReader.Close()
    Return page_count
  End Function

  Public Shared Sub InsertInvisibleTextIntoPDF(ByVal pdfFilename As String, ByVal indexList As List(Of PDFWordIndex))
    Dim doc As iTextSharp.text.Document = New iTextSharp.text.Document
    Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(pdfFilename, FileMode.Create))
    doc.Open()
    Dim cb As PdfContentByte = writer.DirectContent
    Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED)
    cb.BeginText()
    For Each item As PDFWordIndex In indexList
      Dim text As String = item.Text
      cb.SetFontAndSize(bf, item.FontSize)
      'Must convert image x,y (x units per inch) to PDF x,y (72 units per inch)
      'Must know PDF page size to calculate the scale factor
      'Must invert Y coordinate so we go top -> bottom
      cb.SetTextMatrix(item.X, item.Y)
      cb.ShowText(text)
    Next
    cb.EndText()
    doc.Close()
  End Sub

  Public Shared Function BuildBookmarkTreeFromPDF(ByVal FileName As String, ByVal TreeNodes As TreeNodeCollection) As Boolean
    TreeNodes.Clear()
    Dim oPdfReader As PdfReader = New PdfReader(FileName)
    Dim arList As ArrayList = New ArrayList()
    arList = SimpleBookmark.GetBookmark(oPdfReader)
    oPdfReader.Close()
    If Nothing Is arList Then
      Return False
    End If
    Using ms As New MemoryStream()
      SimpleBookmark.ExportToXML(arList, ms, "UTF-8", False)
      ms.Position = 0
      Using xr As XmlReader = XmlReader.Create(ms)
        xr.MoveToContent()
        Dim page As String = Nothing
        Dim text As String = Nothing
        Dim CurrentNode As New TreeNode
        CurrentNode = TreeNodes.Add("Bookmarks")
        Dim re As New Regex("^\d+")
        While xr.Read()
          If xr.NodeType = XmlNodeType.Element AndAlso xr.Name = "Title" AndAlso xr.IsStartElement() Then
            If Not Nothing = xr.GetAttribute("Page") Then
              page = re.Match(xr.GetAttribute("Page")).Captures(0).Value
            End If
            xr.Read()
            If xr.NodeType = XmlNodeType.Text Then
              text = xr.Value.Trim()
              CurrentNode = CurrentNode.Nodes.Add(page, text, page)
            End If
          End If
          If xr.NodeType = XmlNodeType.EndElement AndAlso xr.Name = "Title" Then
            If Not Nothing Is CurrentNode.Parent Then
              CurrentNode = CurrentNode.Parent
            End If
          End If
        End While
      End Using
    End Using
    Return True
  End Function

  Public Shared Function TiffToPDF(ByVal psFilename As String, ByVal outputFileName As String, ByVal startPage As Integer, ByVal endPage As Integer, ByVal psPageSize As iTextSharp.text.Rectangle)
    ' creation of the document with a certain size and certain margins
    Dim document As New Document(psPageSize, 0, 0, 0, 0)
    'Document.compress = false;

    Try
      ' creation of the different writers
      Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(outputFileName, FileMode.Create))

      Dim bm As New System.Drawing.Bitmap(psFilename)
      Dim total As Integer = bm.GetFrameCount(FrameDimension.Page)

      Console.WriteLine("Number of images in this TIFF: " & total)

      If endPage > total Then
        endPage = total
      End If

      If startPage < 1 Then
        startPage = 1
      End If

      ' Which of the multiple images in the TIFF file do we want to load
      ' 0 refers to the first, 1 to the second and so on.
      document.Open()
      Dim cb As PdfContentByte = writer.DirectContent
      For k As Integer = startPage To endPage
        bm.SelectActiveFrame(FrameDimension.Page, k - 1)
        Dim img As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Tiff)
        Dim Xpercent As Single = document.PageSize.Width / img.Width
        Dim Ypercent As Single = document.PageSize.Height / img.Height
        Dim ScalePercentage As Single
        If Xpercent < Ypercent Then
          ScalePercentage = Xpercent
        Else
          ScalePercentage = Ypercent
        End If
        img.ScalePercent(ScalePercentage * 100)
        Dim xPos As Integer = (document.PageSize.Width - (img.Width * ScalePercentage)) / 2
        Dim yPos As Integer = (document.PageSize.Height - (img.Height * ScalePercentage)) / 2
        img.SetAbsolutePosition(xPos, yPos)
        Console.WriteLine("Image: " & k)
        Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, BaseFont.EMBEDDED)
        cb.BeginText()
        Dim indexList As List(Of PDFWordIndex)
        indexList = TesseractOCR.GetPDFIndex(bm)
        For Each item As PDFWordIndex In indexList
          Dim text As String = item.Text
          cb.SetFontAndSize(bf, item.FontSize * 1.7)
          'Must convert image x,y (x units per inch) to PDF x,y (72 units per inch)
          'Must know PDF page size to calculate the scale factor
          'Must invert Y coordinate so we go top -> bottom
          Dim x As Integer = (item.X * ScalePercentage) + xPos
          Dim y As Integer = (item.Y * ScalePercentage) + yPos
          y = (document.PageSize.Height - y) - item.FontSize
          cb.SetTextMatrix(x, y)
          cb.ShowText(text)
        Next
        cb.EndText()
        cb.AddImage(img)
        document.NewPage()
      Next
      bm.Dispose()
      document.Close()
      TiffToPDF = outputFileName
    Catch de As Exception
      Console.[Error].WriteLine(de.Message)
      Console.[Error].WriteLine(de.StackTrace)
      TiffToPDF = ""
    End Try
  End Function

  Public Shared Function GraphicListToPDF(ByVal psFilenames As String(), ByVal outputFileName As String, ByVal psPageSize As iTextSharp.text.Rectangle, ByVal DoOCR As Boolean)
    ' creation of the document with a certain size and certain margins
    Dim document As New Document(psPageSize, 0, 0, 0, 0)

    Try
      Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(outputFileName, FileMode.Create))
      document.Open()
      Dim cb As PdfContentByte = writer.DirectContent
      For Each psFileName As String In psFilenames
        ' creation of the different writers

        Dim bm As New System.Drawing.Bitmap(psFileName)
        Dim total As Integer = bm.GetFrameCount(FrameDimension.Page)

        For k As Integer = 1 To total
          bm.SelectActiveFrame(FrameDimension.Page, k - 1)
          If DoOCR = False Then 'Will need to OCR before rotating and make PDF text generation do a horizontal adjust on the OCR word coordinates
            'Auto Rotate counterclockwise if needed
            If document.PageSize.Height > document.PageSize.Width And bm.Width > bm.Height Then
              bm.RotateFlip(Drawing.RotateFlipType.Rotate270FlipNone)
            End If
          End If
          Dim img As iTextSharp.text.Image
          img = iTextSharp.text.Image.GetInstance(bm, bm.RawFormat)
          Dim Xpercent As Single = document.PageSize.Width / img.Width
          Dim Ypercent As Single = document.PageSize.Height / img.Height
          Dim ScalePercentage As Single
          If Xpercent < Ypercent Then
            ScalePercentage = Xpercent
          Else
            ScalePercentage = Ypercent
          End If
          img.ScalePercent(ScalePercentage * 100)
          Dim xPos As Integer = (document.PageSize.Width - (img.Width * ScalePercentage)) / 2
          Dim yPos As Integer = (document.PageSize.Height - (img.Height * ScalePercentage)) / 2
          img.SetAbsolutePosition(xPos, yPos)
          Try
            If DoOCR Then
              Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED)
              cb.BeginText()
              Dim indexList As List(Of PDFWordIndex)
              indexList = TesseractOCR.GetPDFIndex(bm)

              For Each item As PDFWordIndex In indexList
                Dim fontSize As Integer = item.FontSize + (ScalePercentage * item.FontSize)
                Dim text As String = item.Text
                cb.SetFontAndSize(bf, fontSize)
                'Must convert image x,y (x units per inch) to PDF x,y (72 units per inch)
                'Must know PDF page size to calculate the scale factor
                'Must invert Y coordinate so we go top -> bottom
                Dim x As Integer = (item.X * ScalePercentage) + xPos
                Dim y As Integer = (item.Y * ScalePercentage) + yPos
                y = (document.PageSize.Height - y) - item.FontSize
                Dim a As Integer = x + (item.Width * ScalePercentage)
                Dim b As Integer = y - (item.Height * ScalePercentage)
                'cb.SetTextMatrix(x, y)
                cb.ShowTextAlignedKerned(Element.ALIGN_JUSTIFIED_ALL, text, x, y, 0)
                'Dim ct As ColumnText = New ColumnText(cb)
                'ct.SetSimpleColumn(x, y, a, b, 0, Element.ALIGN_JUSTIFIED)
                'ct.AddText(New Phrase(text))
                'While ct.Go(True) = ColumnText.NO_MORE_COLUMN
                '  cb.SetFontAndSize(bf, fontSize - 1)
                'End While
                'Dim ct1 As ColumnText = New ColumnText(cb)
                'ct1.SetSimpleColumn(x, y, a, b, 0, Element.ALIGN_JUSTIFIED)
                'ct1.AddText(New Phrase(text))
                'ct1.Go()
              Next
              cb.EndText()
            End If
          Catch ex As Exception
            MsgBox(ex.ToString)
          End Try
          Try
            cb.AddImage(img)
          Catch ex As Exception
            MsgBox(ex.ToString)
          End Try
          document.NewPage()
        Next
        bm.Dispose()
      Next
      document.Close()
      GraphicListToPDF = outputFileName
    Catch de As Exception
      Console.[Error].WriteLine(de.Message)
      Console.[Error].WriteLine(de.StackTrace)
      GraphicListToPDF = ""
    End Try
  End Function


End Class
