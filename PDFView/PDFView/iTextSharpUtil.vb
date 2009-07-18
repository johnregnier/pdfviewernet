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
        Dim document As New Document(psPageSize, 50, 50, 50, 50)
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
                Dim img As Image = Image.GetInstance(bm, ImageFormat.Tiff)
                img.ScalePercent(72.0F / bm.HorizontalResolution * 100)
                img.SetAbsolutePosition(0, 0)
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


End Class
