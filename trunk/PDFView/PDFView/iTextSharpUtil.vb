Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

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

End Class
