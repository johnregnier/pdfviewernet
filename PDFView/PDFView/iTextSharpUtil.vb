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

  Public Shared Function GraphicListToPDF(ByVal psFilenames As String(), ByVal outputFileName As String, ByVal psPageSize As iTextSharp.text.Rectangle)
    ' creation of the document with a certain size and certain margins
    Dim document As New Document(psPageSize, 0, 0, 0, 0)
    'Document.compress = false;

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
          Dim img As iTextSharp.text.Image
          If Regex.IsMatch(psFileName, "\.jpg$", RegexOptions.IgnoreCase) Then img = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Jpeg)
          If Regex.IsMatch(psFileName, "\.png$", RegexOptions.IgnoreCase) Then img = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Png)
          If Regex.IsMatch(psFileName, "\.bmp$", RegexOptions.IgnoreCase) Then img = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Bmp)
          If Regex.IsMatch(psFileName, "\.tif$", RegexOptions.IgnoreCase) Then img = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Tiff)
          If Regex.IsMatch(psFileName, "\.gif$", RegexOptions.IgnoreCase) Then img = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Gif)
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
          cb.AddImage(img)
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
