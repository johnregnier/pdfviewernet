Imports System.Text.RegularExpressions
Imports System.Drawing

Public Class ExportOptions

  Dim mpdfDoc As PDFLibNet.PDFWrapper
  Dim mPdfFileName As String
  Dim mFilter As String = ""
  Dim mPassword As String = ""

  Const BW_TIFF_G4 As String = "tiffg4"
  Const BW_TIFF_LZW As String = "tifflzw"
  Const GRAY_TIFF_NC As String = "tiffgray"
  Const GRAY_PNG = "pnggray"
  Const GRAY_JPG = "jpeggray"
  Const COLOR_TIFF_RGB As String = "tiff24nc"
  Const COLOR_TIFF_CMYK As String = "tiff32nc"
  Const COLOR_TIFF_CMYK_SEP As String = "tiffsep"
  Const COLOR_PNG_RGB As String = "png16m"
  Const COLOR_JPEG = "jpeg"

  Public Sub New(ByVal pdfFileName As String, ByVal pdfDoc As PDFLibNet.PDFWrapper, Optional ByVal password As String = "")

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    mPdfFileName = pdfFileName
    mpdfDoc = pdfDoc
    mPassword = password
    nuStart.Maximum = mpdfDoc.PageCount
    nuStart.Value = 1
    nuDown.Maximum = mpdfDoc.PageCount
    nuDown.Value = mpdfDoc.PageCount
    nuDPI.Value = 150
    SaveFileDialog1.Filter = rbPostscript.Tag
  End Sub

  Private Sub btOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btOK.Click
    If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
      Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
      Dim filename As String = SaveFileDialog1.FileName
      If filename.EndsWith(".ps") Then
        mpdfDoc.PrintToFile(filename, nuStart.Value, nuDown.Value)
      ElseIf filename.EndsWith(".jpg") Then
        ConvertPDF.PDFConvert.ConvertPdfToGraphic(mPdfFileName, SaveFileDialog1.FileName, COLOR_JPEG, nuDPI.Value, nuStart.Value, nuDown.Value, False, mPassword)
      ElseIf filename.EndsWith(".tif") Then
        Dim returnFileName As String
        returnFileName = ConvertPDF.PDFConvert.ConvertPdfToGraphic(mPdfFileName, SaveFileDialog1.FileName, COLOR_TIFF_RGB, nuDPI.Value, nuStart.Value, nuDown.Value, False, mPassword)
        Try
          ImageUtil.CompressTiff(returnFileName)
        Catch
          MsgBox("An error occurred while applying LZW compression to the TIFF file. The TIFF file has been saved in an uncompressed format instead.", MsgBoxStyle.OkOnly, "TIFF Compression Error")
        End Try
      ElseIf filename.EndsWith(".png") Then
        ConvertPDF.PDFConvert.ConvertPdfToGraphic(mPdfFileName, SaveFileDialog1.FileName, COLOR_PNG_RGB, nuDPI.Value, nuStart.Value, nuDown.Value, False, mPassword)
      ElseIf filename.EndsWith(".txt") Then
        mpdfDoc.ExportText(filename, nuStart.Value, nuDown.Value, True, True)
      ElseIf filename.EndsWith(".html") And rbHtml.Checked Then
        mpdfDoc.ExportHtml(filename, nuStart.Value, nuDown.Value, True, True, False)
      ElseIf filename.EndsWith(".html") And rbHtmlImage.Checked Then
        ExportHTMLImages(nuDPI.Value, filename, "png")
      End If
      Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
    End If
    Me.Hide()
  End Sub

  Private Sub nuStart_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nuStart.ValueChanged
    If nuStart.Value > nuDown.Value Then
      nuStart.Value = nuDown.Value
    End If
  End Sub

  Private Sub nuDown_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nuDown.ValueChanged
    If nuDown.Value < nuStart.Value Then
      nuDown.Value = nuStart.Value
    End If
  End Sub

  Private Sub CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbHtml.CheckedChanged, rbJpeg.CheckedChanged, rbPostscript.CheckedChanged, rbText.CheckedChanged, rbPNG.CheckedChanged, rbTIFF.CheckedChanged, rbHtmlImage.CheckedChanged
    If rbHtml.Checked Then
      SaveFileDialog1.Filter = rbHtml.Tag
      GroupBox3.Enabled = False
    ElseIf rbHtmlImage.Checked Then
      SaveFileDialog1.Filter = rbHtmlImage.Tag
      GroupBox3.Enabled = True
    ElseIf rbJpeg.Checked Then
      SaveFileDialog1.Filter = rbJpeg.Tag
      GroupBox3.Enabled = True
    ElseIf rbPostscript.Checked Then
      SaveFileDialog1.Filter = rbPostscript.Tag
      GroupBox3.Enabled = False
    ElseIf rbText.Checked Then
      SaveFileDialog1.Filter = rbText.Tag
      GroupBox3.Enabled = False
    ElseIf rbPNG.Checked Then
      SaveFileDialog1.Filter = rbPNG.Tag
      GroupBox3.Enabled = True
    ElseIf rbTIFF.Checked Then
      SaveFileDialog1.Filter = rbTIFF.Tag
      GroupBox3.Enabled = True
    End If
  End Sub

  Private Sub btCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCancel.Click
    Me.Hide()
  End Sub

  Public Sub ExportHTMLImages(ByVal DPI As Integer, ByVal fileName As String, ByVal format As String)
    Dim folderPath As String = System.Text.RegularExpressions.Regex.Replace(fileName, "(^.+\\).+$", "$1")
    Dim contentFolder As String = folderPath & "content"
    Dim imagesFolder As String = contentFolder & "\images"

    Console.WriteLine("Extracting bookmarks ...")
    Dim bookMarkHtml As String = AFPDFLibUtil.BuildHTMLBookmarks(mpdfDoc)

    Dim sideFrame As String = My.Resources.BookmarkHtml
    sideFrame = sideFrame.Replace("{Body}", bookMarkHtml)
    sideFrame = sideFrame.Replace("{PageCount}", mpdfDoc.PageCount.ToString)
    sideFrame = sideFrame.Replace("{Extension}", format)

    Dim pageFrame As String = My.Resources.PageHtml
    pageFrame = pageFrame.Replace("{PageCount}", mpdfDoc.PageCount.ToString)
    pageFrame = pageFrame.Replace("{DPI}", DPI.ToString)
    pageFrame = pageFrame.Replace("{Extension}", format)

    Console.WriteLine("Extracting html links ...")
    Dim myDict1 As DictionaryEntry = AFPDFLibUtil.GetHtmlLinks(mpdfDoc)
    Dim htmlLinkCount As Integer = myDict1.Key
    If htmlLinkCount < 0 Then htmlLinkCount = 0
    pageFrame = pageFrame.Replace("{HtmlLinkCount-1}", htmlLinkCount.ToString)
    pageFrame = pageFrame.Replace("{HtmlLinkContent}", myDict1.Value)

    Console.WriteLine("Extracting page links ...")
    Dim myDict2 As DictionaryEntry = AFPDFLibUtil.GetPageLinks(mpdfDoc)
    Dim pageLinkCount As Integer = myDict2.Key
    If pageLinkCount < 0 Then pageLinkCount = 0
    pageFrame = pageFrame.Replace("{PageLinkCount-1}", pageLinkCount.ToString)
    pageFrame = pageFrame.Replace("{PageLinkContent}", myDict2.Value)

    Dim mainPage As String = ""
    If bookMarkHtml = "" Then
      mainPage = My.Resources.FrameNoBook
    Else
      mainPage = My.Resources.FrameHtml
    End If
    mainPage = mainPage.Replace("{DocumentName}", Regex.Replace(mPdfFileName, "^.+\\", ""))

    Console.WriteLine("Extracting page content ...")
    Dim pageSize As String = My.Resources.PagesizeHtml
    Dim myDict As DictionaryEntry = AFPDFLibUtil.GetPages(mpdfDoc)
    pageSize = pageSize.Replace("{PageCount-1}", myDict.Key.ToString)
    pageSize = pageSize.Replace("{PageCount}", mpdfDoc.PageCount.ToString)
    pageSize = pageSize.Replace("{PageContent}", myDict.Value)
    pageSize = pageSize.Replace("{DocumentName}", Regex.Replace(mPdfFileName, "^.+\\", ""))
    pageSize = pageSize.Replace("{Extension}", format)

    Console.WriteLine("Creating website directories ...")
    Dim di As System.IO.DirectoryInfo
    di = New System.IO.DirectoryInfo(contentFolder)
    If (Not di.Exists) Then
      di.Create()
    End If

    di = New System.IO.DirectoryInfo(imagesFolder)
    If (Not di.Exists) Then
      di.Create()
    End If

    Console.WriteLine("Copying navigation graphics ...")
    Dim myPic As Bitmap = My.Resources._Next
    myPic.Save(imagesFolder & "\Next.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.Previous
    myPic.Save(imagesFolder & "\Previous.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.ActualSize
    myPic.Save(imagesFolder & "\ActualSize.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.FitToScreen
    myPic.Save(imagesFolder & "\FitToScreen.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.FitToWidth
    myPic.Save(imagesFolder & "\FitToWidth.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.ZoomIn
    myPic.Save(imagesFolder & "\ZoomIn.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.ZoomOut
    myPic.Save(imagesFolder & "\ZoomOut.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.Search
    myPic.Save(imagesFolder & "\Search.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.SearchNext
    myPic.Save(imagesFolder & "\SearchNext.png", Imaging.ImageFormat.Png)

    myPic = My.Resources.SearchPrevious
    myPic.Save(imagesFolder & "\SearchPrevious.png", Imaging.ImageFormat.Png)

    Console.WriteLine("Copying pdf file ...")
    FileCopy(mPdfFileName, imagesFolder & "\" & Regex.Replace(mPdfFileName, "^.+\\", ""))

    Dim sw As New IO.StreamWriter(fileName, False)
    sw.Write(mainPage)
    sw.Close()

    Console.WriteLine("Writing html files ...")
    If bookMarkHtml <> "" Then
      Dim sw2 As New IO.StreamWriter(contentFolder & "\bookmark.html", False)
      sw2.Write(sideFrame)
      sw2.Close()
    End If

    Dim sw3 As New IO.StreamWriter(contentFolder & "\page.html", False)
    sw3.Write(pageFrame)
    sw3.Close()

    Dim sw4 As New IO.StreamWriter(contentFolder & "\pagesize.html", False)
    sw4.Write(pageSize)
    sw4.Close()

    Dim pageCount As Integer = mpdfDoc.PageCount

    Console.WriteLine("Rendering page graphics ...")
    If Regex.IsMatch(format, "jpg", RegexOptions.IgnoreCase) Then
      mpdfDoc.ExportJpg(imagesFolder & "\page%d.jpg", 1, pageCount, DPI, 90, -1)
      Console.WriteLine("Conversion completed")
    ElseIf Regex.IsMatch(format, "png", RegexOptions.IgnoreCase) Then
      For i As Integer = 1 To pageCount
        Dim bm As Bitmap = AFPDFLibUtil.GetImageFromPDF(mpdfDoc, i, DPI)
        bm.Save(imagesFolder & "\page" & i & ".png", Imaging.ImageFormat.Png)
        bm.Dispose()
      Next
      Console.WriteLine("Conversion completed")
    End If
  End Sub

  'Private Sub ExportHTMLImages(ByVal fileName As String)
  '  Dim folderPath As String = System.Text.RegularExpressions.Regex.Replace(fileName, "(^.+\\).+$", "$1")
  '  Dim contentFolder As String = folderPath & "content"
  '  Dim imagesFolder As String = contentFolder & "\images"

  '  Dim sideFrame As String = My.Resources.BookmarkHtml
  '  'Possible to allow some export from GhostScript renderer
  '  sideFrame = sideFrame.Replace("{Body}", AFPDFLibUtil.BuildHTMLBookmarks(mpdfDoc))
  '  sideFrame = sideFrame.Replace("{PageCount}", mpdfDoc.PageCount.ToString)

  '  Dim pageFrame As String = My.Resources.PageHtml
  '  pageFrame = pageFrame.Replace("{PageCount}", mpdfDoc.PageCount.ToString)
  '  pageFrame = pageFrame.Replace("{DPI}", nuDPI.Value.ToString)
  '  Dim myDict1 As DictionaryEntry = AFPDFLibUtil.GetHtmlLinks(mpdfDoc)
  '  pageFrame = pageFrame.Replace("{HtmlLinkCount-1}", myDict1.Key.ToString)
  '  pageFrame = pageFrame.Replace("{HtmlLinkContent}", myDict1.Value)

  '  Dim myDict2 As DictionaryEntry = AFPDFLibUtil.GetPageLinks(mpdfDoc)
  '  pageFrame = pageFrame.Replace("{PageLinkCount-1}", myDict2.Key.ToString)
  '  pageFrame = pageFrame.Replace("{PageLinkContent}", myDict2.Value)

  '  Dim mainPage As String = My.Resources.FrameHtml

  '  Dim pageSize As String = My.Resources.PagesizeHtml
  '  Dim myDict As DictionaryEntry = AFPDFLibUtil.GetPages(mpdfDoc)
  '  pageSize = pageSize.Replace("{PageCount-1}", myDict.Key.ToString)
  '  pageSize = pageSize.Replace("{PageCount}", mpdfDoc.PageCount.ToString)
  '  pageSize = pageSize.Replace("{PageContent}", myDict.Value)
  '  pageSize = pageSize.Replace("{DocumentName}", Regex.Replace(mPdfFileName, "^.+\\", ""))

  '  Dim di As System.IO.DirectoryInfo
  '  di = New System.IO.DirectoryInfo(contentFolder)
  '  If (Not di.Exists) Then
  '    di.Create()
  '  End If

  '  di = New System.IO.DirectoryInfo(imagesFolder)
  '  If (Not di.Exists) Then
  '    di.Create()
  '  End If

  '  Dim myPic As Bitmap = My.Resources._Next
  '  myPic.Save(imagesFolder & "\Next.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.Previous
  '  myPic.Save(imagesFolder & "\Previous.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.ActualSize
  '  myPic.Save(imagesFolder & "\ActualSize.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.FitToScreen
  '  myPic.Save(imagesFolder & "\FitToScreen.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.FitToWidth
  '  myPic.Save(imagesFolder & "\FitToWidth.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.ZoomIn
  '  myPic.Save(imagesFolder & "\ZoomIn.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.ZoomOut
  '  myPic.Save(imagesFolder & "\ZoomOut.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.Search
  '  myPic.Save(imagesFolder & "\Search.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.SearchNext
  '  myPic.Save(imagesFolder & "\SearchNext.png", Imaging.ImageFormat.Png)

  '  myPic = My.Resources.SearchPrevious
  '  myPic.Save(imagesFolder & "\SearchPrevious.png", Imaging.ImageFormat.Png)

  '  FileCopy(mPdfFileName, imagesFolder & "\" & Regex.Replace(mPdfFileName, "^.+\\", ""))

  '  Dim sw As New IO.StreamWriter(fileName, False)
  '  sw.Write(mainPage)
  '  sw.Close()

  '  Dim sw2 As New IO.StreamWriter(contentFolder & "\bookmark.html", False)
  '  sw2.Write(sideFrame)
  '  sw2.Close()

  '  Dim sw3 As New IO.StreamWriter(contentFolder & "\page.html", False)
  '  sw3.Write(pageFrame)
  '  sw3.Close()

  '  Dim sw4 As New IO.StreamWriter(contentFolder & "\pagesize.html", False)
  '  sw4.Write(pageSize)
  '  sw4.Close()

  '  ConvertPDF.PDFConvert.ConvertPdfToGraphic(mPdfFileName, imagesFolder & "\page.png", COLOR_PNG_RGB, nuDPI.Value, nuStart.Value, nuDown.Value, False, mPassword)
  'End Sub

End Class