Imports System.Text.RegularExpressions
Imports System.Drawing.Imaging
Imports FreeImageAPI
Imports System.Drawing
Imports System.Windows.Forms

Public Class PDFViewer
    Private mOriginalFileName
    Private mPDFFileName As String
    Private mPDFFrameFileName As String
    Private mPDFPageCount As Integer
    Private mCurrentPageNumber As Integer
    Private m_PanStartPoint As Point
    Private mTempPDFTiffFile As String
    Private mAllPages As Boolean
    Private mBookmarks As ArrayList
    Private mAllowBookmarks As Boolean


    Public Property FileName() As String
        Get
            Return mOriginalFileName
        End Get
        Set(ByVal value As String)
            If Nothing = value Or value = "" Then
                GoTo LoadError
            End If
            mOriginalFileName = value
            ImageUtil.DeleteFile(mTempPDFTiffFile)
            If Regex.IsMatch(value, "\.pdf$", RegexOptions.IgnoreCase) Then
                If mAllPages = True Then
                    value = ConvertPDF.PDFConvert.ConvertPdfToTiff(value, 0)
                    mTempPDFTiffFile = value
                End If
                mPDFFileName = value
                Cursor.Current = Cursors.WaitCursor
                PictureBox1.Image = Nothing
                PictureBox1.Refresh()
                InitPageRange()
                If mAllowBookmarks Then
                    InitBookmarks()
                End If
                DisplayCurrentPage()
                FitToScreen()
                Me.Enabled = True
                Cursor.Current = Cursors.Default
            Else
LoadError:
                Me.Enabled = False
            End If
        End Set
    End Property

    Public Property LoadAllPages() As Boolean
        Get
            Return mAllPages
        End Get
        Set(ByVal value As Boolean)
            mAllPages = value
        End Set
    End Property

    Public Property AllowBookmarks() As Boolean
        Get
            Return mAllowBookmarks
        End Get
        Set(ByVal value As Boolean)
            mAllowBookmarks = value
            If value = False Then
                HideBookmarks()
            End If
        End Set
    End Property

    Public ReadOnly Property PageCount(ByVal FileName As String)
        Get
            Return ImageUtil.GetImageFrameCount(FileName)
        End Get
    End Property

    Public ReadOnly Property Print(ByVal FileName As String)
        Get
            PrinterUtil.PrintPDFImagesToPrinter(FileName)
            Return 1
        End Get
    End Property

    Private Sub PDFViewer_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlRemoved
        ImageUtil.DeleteFile(mTempPDFTiffFile)
        ImageUtil.DeleteFile(mPDFFrameFileName)
    End Sub

    Private Sub PDFViewer_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        ImageUtil.DeleteFile(mTempPDFTiffFile)
        ImageUtil.DeleteFile(mPDFFrameFileName)
    End Sub

    Private Sub PDFViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Do something on control load
        HideBookmarks()
    End Sub

    Private Sub tsPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsPrevious.Click
        mCurrentPageNumber -= 1
        DisplayCurrentPage()
    End Sub

    Private Sub tsNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsNext.Click
        mCurrentPageNumber += 1
        DisplayCurrentPage()
    End Sub

    Private Sub tsZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsZoomOut.Click
        ImageUtil.PictureBoxZoomOut(PictureBox1)
        tscbZoom.Text = GetCurrentScalePercentage() & " %"
    End Sub

    Private Sub tsZoomIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsZoomIn.Click
        If GetCurrentScalePercentage() > 500 Then
            Exit Sub
        End If
        ImageUtil.PictureBoxZoomIn(PictureBox1)
        tscbZoom.Text = GetCurrentScalePercentage() & " %"
    End Sub

    Private Sub tsPageNum_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tsPageNum.KeyPress
        If (Asc(e.KeyChar) = Keys.Enter) Then
            If tsPageNum.Text = "" Then
                mCurrentPageNumber = 1
            Else
                mCurrentPageNumber = tsPageNum.Text
            End If
            DisplayCurrentPage()
        Else
            e.Handled = TrapKey(Asc(e.KeyChar))
        End If
    End Sub

    Private Sub tsPageNum_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsPageNum.Leave
        mCurrentPageNumber = tsPageNum.Text
        DisplayCurrentPage()
    End Sub

    Private Sub PDFViewer_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        FitToScreen()
    End Sub

    Private Sub tscbZoom_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tscbZoom.SelectedIndexChanged
        Select Case tscbZoom.Text
            Case "Fit To Screen"
                ImageUtil.PictureBoxZoomFit(PictureBox1)
            Case "Actual Size"
                ImageUtil.PictureBoxZoomActual(PictureBox1)
            Case "Fit To Width"
                ImageUtil.PictureBoxZoomPageWidth(PictureBox1)
        End Select
    End Sub

    Private Sub TreeView1_NodeMouseClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Node.ImageKey <> "" Then
            mCurrentPageNumber = e.Node.ImageKey
            DisplayCurrentPage()
        End If
    End Sub

    Private Sub tsPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsPrint.Click
        PrinterUtil.PrintPDFImagesToPrinter(mOriginalFileName)
    End Sub

    Private Sub tsRotateCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsRotateCC.Click
        ImageUtil.RotateImageCounterclockwise(PictureBox1)
    End Sub

    Private Sub tsRotateC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsRotateC.Click
        ImageUtil.RotateImageClockwise(PictureBox1)
    End Sub

#Region "Constraints"

    Private Sub CheckPageBounds()

        If mCurrentPageNumber >= mPDFPageCount Then
            mCurrentPageNumber = mPDFPageCount
            tsNext.Enabled = False
        ElseIf mCurrentPageNumber <= 1 Then
            mCurrentPageNumber = 1
            tsPrevious.Enabled = False
        End If

        If mCurrentPageNumber < mPDFPageCount And mPDFPageCount > 1 And mCurrentPageNumber > 1 Then
            tsNext.Enabled = True
            tsPrevious.Enabled = True
        End If

        If mCurrentPageNumber = mPDFPageCount And mPDFPageCount > 1 And mCurrentPageNumber > 1 Then
            tsPrevious.Enabled = True
        End If

        If mCurrentPageNumber = 1 And mPDFPageCount > 1 Then
            tsNext.Enabled = True
        End If

        If mPDFPageCount = 1 Then
            tsNext.Enabled = False
            tsPrevious.Enabled = False
        End If

    End Sub

#End Region

#Region "Helper Functions"

    Private Sub ShowImageFromFile(ByVal sFileName As String, ByVal iFrameNumber As Integer, ByRef oPictureBox As PictureBox)
        oPictureBox.Invalidate()
        Cursor.Current = Cursors.WaitCursor
        If mPDFFrameFileName <> "" Then
            ImageUtil.DeleteFile(mPDFFrameFileName)
        End If
        If ImageUtil.IsPDF(sFileName) Then 'convert one frame to a tiff for viewing
            mPDFFrameFileName = ConvertPDF.PDFConvert.ConvertPdfToTiff(sFileName, iFrameNumber + 1)
            sFileName = mPDFFrameFileName
            oPictureBox.Image = ImageUtil.GetFrameFromTiff2(sFileName, 0)
            ImageUtil.DeleteFile(sFileName)
        ElseIf ImageUtil.IsTiff(sFileName) Then
            oPictureBox.Image = ImageUtil.GetFrameFromTiff2(sFileName, iFrameNumber)
        End If
        oPictureBox.Update()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub InitPageRange()
        mPDFPageCount = ImageUtil.GetImageFrameCount(mPDFFileName)
        mCurrentPageNumber = 1
    End Sub

    Private Sub InitBookmarks()
        TreeView1.Nodes.Clear()
        Dim HasBookmarks As Boolean = False
        Try
            HasBookmarks = iTextSharpUtil.BuildBookmarkTreeFromPDF(mOriginalFileName, TreeView1.Nodes)
        Catch ex As Exception
            'Some bookmark structures do not parse from XML yet.
            'TODO
        End Try
        If HasBookmarks Then
            ShowBookmarks()
            TreeView1.ExpandAll()
            TreeView1.SelectedNode = TreeView1.Nodes.Item(0)
        Else
            HideBookmarks()
        End If
    End Sub

    Private Sub HideBookmarks()
        TreeView1.Visible = False
        TableLayoutPanel1.SetColumnSpan(TreeView1, 2)
        TableLayoutPanel1.SetColumnSpan(Panel1, 2)
        TableLayoutPanel1.SetColumn(Panel1, 0)
        TableLayoutPanel1.SetColumn(TreeView1, 1)
    End Sub

    Private Sub ShowBookmarks()
        TreeView1.Visible = True
        TableLayoutPanel1.SetColumnSpan(TreeView1, 1)
        TableLayoutPanel1.SetColumnSpan(Panel1, 1)
        TableLayoutPanel1.SetColumn(Panel1, 1)
        TableLayoutPanel1.SetColumn(TreeView1, 0)
    End Sub

    Private Sub UpdatePageLabel()
        tsPageLabel.Text = "Page " & mCurrentPageNumber & " of " & mPDFPageCount
        tsPageNum.Text = mCurrentPageNumber
    End Sub

    Private Sub DisplayCurrentPage()
        CheckPageBounds()
        UpdatePageLabel()
        ShowImageFromFile(mPDFFileName, mCurrentPageNumber - 1, PictureBox1)
    End Sub

    Private Function GetCurrentScalePercentage() As Integer
        Dim OriginalWidth As Integer = PictureBox1.Image.Width
        Dim CurrentWidth As Integer = PictureBox1.Width
        GetCurrentScalePercentage = CInt((CurrentWidth / OriginalWidth) * 100)
    End Function

    Private Sub FitToScreen()
        ImageUtil.PictureBoxZoomFit(PictureBox1)
        tscbZoom.SelectedIndex = 0
    End Sub

    Private Function TrapKey(ByVal KCode As String) As Boolean
        If (KCode >= 48 And KCode <= 57) Or KCode = 8 Then
            TrapKey = False
        Else
            TrapKey = True
        End If
    End Function

#End Region

#Region "Panning the image with left mouse click held down"

    Private Sub picImage_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        'Capture the initial point 	
        Cursor = Cursors.Hand
        m_PanStartPoint = New Point(e.X, e.Y)
    End Sub

    Private Sub picImage_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        'Capture the initial point 	
        Cursor = Cursors.Default
    End Sub

    Private Sub picImage_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        'Verify Left Button is pressed while the mouse is moving	
        If e.Button = Windows.Forms.MouseButtons.Left Then
            'Here we get the change in coordinates.		
            Dim DeltaX As Integer = (m_PanStartPoint.X - e.X)
            Dim DeltaY As Integer = (m_PanStartPoint.Y - e.Y)
            'Then we set the new autoscroll position.		
            'ALWAYS pass positive integers to the panels autoScrollPosition method		
            Panel1.AutoScrollPosition = New Drawing.Point((DeltaX - Panel1.AutoScrollPosition.X), (DeltaY - Panel1.AutoScrollPosition.Y))
        End If
    End Sub

    'Private Sub MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
    '  Dim iClicks As Integer = e.Delta
    '  If iClicks > 0 Then
    '    ImageUtil.PictureBoxZoomOut(PictureBox1)
    '  Else
    '    ImageUtil.PictureBoxZoomIn(PictureBox1)
    '  End If
    '  ShowCurrentScalePercentage()
    'End Sub

#End Region

End Class

