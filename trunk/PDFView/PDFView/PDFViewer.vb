Imports System.Text.RegularExpressions
Imports System.Drawing.Imaging
Imports FreeImageAPI
Imports System.Drawing
Imports System.Windows.Forms
Imports PDFLibNet

Public Class PDFViewer
    Private mOriginalFileName
    Private mPDFFileName As String
    Private mPDFPageCount As Integer
    Private mCurrentPageNumber As Integer
    Private m_PanStartPoint As Point
    Private mBookmarks As ArrayList
    Private mAllowBookmarks As Boolean = True
    Private mUseXPDF As Boolean = True
    Private mPDFDoc As PDFLibNet.PDFWrapper
    Private FromBeginning As Boolean = True
    Private XPDFPrintingPicBox As New PictureBox
    Private mContinuousPages As Boolean = False
    Private PageInitDone As Boolean = False
    Private ScrollBarPosition As Integer = 0
    Private ScrollUnitsPerPage As Integer = 0
    Private ContinuousImages As List(Of Image)

    Public Property FileName() As String
        Get
            Return mOriginalFileName
        End Get
        Set(ByVal value As String)
            If Nothing = value Or value = "" Then
                Exit Property
            End If
            mOriginalFileName = value
            If ImageUtil.IsTiff(value) Then
                'Tiff Specific behavior
                InitBottomToolbar("TIFF")
            ElseIf ImageUtil.IsPDF(value) Then
                If mUseXPDF Then
                    If Not Nothing Is mPDFDoc Then
                        mPDFDoc.Dispose()
                    End If
                    mPDFDoc = New PDFLibNet.PDFWrapper("")
                    mPDFDoc.LoadPDF(value)
                    InitBottomToolbar("XPDF")
                Else
                    InitBottomToolbar("GS")
                End If
            Else
                Me.Enabled = False
            End If
            mPDFFileName = value
            Cursor.Current = Cursors.WaitCursor
            InitPageRange()
            InitializePageView(ViewMode.FIT_WIDTH)
            If mAllowBookmarks And ImageUtil.IsPDF(mOriginalFileName) Then
                InitBookmarks()
            Else
                HideBookmarks()
            End If
            FitToScreen()
            DisplayCurrentPage()
            tscbZoom.SelectedIndex = 0
            Me.Enabled = True
            Cursor.Current = Cursors.Default
        End Set
    End Property

    'Public Property ContinuousPages() As Boolean
    '    Get
    '        Return mContinuousPages
    '    End Get
    '    Set(ByVal value As Boolean)
    '        mContinuousPages = value
    '    End Set
    'End Property

    Public Property UseXPDF() As Boolean
        Get
            Return mUseXPDF
        End Get
        Set(ByVal value As Boolean)
            mUseXPDF = value
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

    Public Sub SelectFile()
        OpenFileDialog1.Filter = "PDF or TIFF files (*.pdf;*.tif)|*.pdf;*.tif"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Title = "Select a PDF or TIFF file to open"
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.ShowDialog()
        FileName = OpenFileDialog1.FileName
    End Sub

    Private Sub ConvertGraphicsToPDF()
        OpenFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.TIF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Title = "Select multiple image files to convert to PDF"
        OpenFileDialog1.Multiselect = True
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim exportOptionsDialog As New ExportImageOptions(OpenFileDialog1.FileNames, True)
            exportOptionsDialog.ShowDialog()
            Try
                FileName = exportOptionsDialog.SavedFileName
            Catch ex As Exception
                'do nothing
            End Try
        End If

    End Sub

    Private Sub PDFViewer_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlRemoved
        If Not Nothing Is mPDFDoc Then
            mPDFDoc.Dispose()
        End If
    End Sub

    Private Sub PDFViewer_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If Not Nothing Is mPDFDoc Then
            mPDFDoc.Dispose()
        End If
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
        ImageUtil.PictureBoxZoomOut(FindPictureBox(mCurrentPageNumber))
        tscbZoom.Text = GetCurrentScalePercentage() & " %"
    End Sub

    Private Sub tsZoomIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsZoomIn.Click
        If GetCurrentScalePercentage() > 500 Then
            Exit Sub
        End If
        ImageUtil.PictureBoxZoomIn(FindPictureBox(mCurrentPageNumber))
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
        If PageInitDone = True Then
            FitToScreen()
        End If
    End Sub

    Private Sub tscbZoom_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tscbZoom.SelectedIndexChanged
        Select Case tscbZoom.Text
            Case "Fit To Screen"
                If mContinuousPages Then
                    InitializePageView(ViewMode.FIT_TO_SCREEN)
                    DisplayCurrentPage()
                Else
                    ApplyZoom(ViewMode.FIT_TO_SCREEN)
                End If
            Case "Actual Size"
                If mContinuousPages Then
                    InitializePageView(ViewMode.ACTUAL_SIZE)
                    DisplayCurrentPage()
                Else
                    ApplyZoom(ViewMode.ACTUAL_SIZE)
                End If
            Case "Fit To Width"
                If mContinuousPages Then
                    InitializePageView(ViewMode.FIT_WIDTH)
                    DisplayCurrentPage()
                Else
                    ApplyZoom(ViewMode.FIT_WIDTH)
                End If
        End Select
    End Sub

    Private Sub tsPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsPrint.Click
        If mUseXPDF And ImageUtil.IsPDF(mOriginalFileName) Then
            AFPDFLibUtil.PrintPDFImagesToPrinter(mPDFDoc, XPDFPrintingPicBox)
        Else
            PrinterUtil.PrintPDFImagesToPrinter(mOriginalFileName)
        End If
    End Sub

    Private Sub tsRotateCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsRotateCC.Click
        ImageUtil.RotateImageCounterclockwise(FindPictureBox(mCurrentPageNumber))
    End Sub

    Private Sub tsRotateC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsRotateC.Click
        ImageUtil.RotateImageClockwise(FindPictureBox(mCurrentPageNumber))
    End Sub

    Private Sub tsExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsExport.Click
        If ImageUtil.IsPDF(mOriginalFileName) Then
            Dim exportOptionsDialog As New ExportOptions(mPDFFileName, mPDFDoc)
            exportOptionsDialog.ShowDialog()
        ElseIf ImageUtil.IsTiff(mOriginalFileName) Then
            Dim FileArray(0) As String
            FileArray(0) = mPDFFileName
            Dim exportOptionsDialog As New ExportImageOptions(FileArray, False)
            exportOptionsDialog.ShowDialog()
        End If
        Me.Focus()
    End Sub

    Private Sub tsImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsImport.Click
        ConvertGraphicsToPDF()
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

    Private Function ShowImageFromFile(ByVal sFileName As String, ByVal iFrameNumber As Integer, ByRef oPictureBox As PictureBox, Optional ByVal XPDFDPI As Integer = 0) As Image
        oPictureBox.Invalidate()
        'Cursor.Current = Cursors.WaitCursor
        If mUseXPDF And ImageUtil.IsPDF(sFileName) Then 'Use AFPDFLib (XPDF)
            If XPDFDPI > 0 Then
                AFPDFLibUtil.DrawImageFromPDF(mPDFDoc, iFrameNumber + 1, oPictureBox, XPDFDPI)
            Else
                AFPDFLibUtil.DrawImageFromPDF(mPDFDoc, iFrameNumber + 1, oPictureBox)
            End If
        Else 'Use Ghostscript
            If ImageUtil.IsPDF(sFileName) Then 'convert one frame to a tiff for viewing
                oPictureBox.Image = ConvertPDF.PDFConvert.GetPageFromPDF(sFileName, iFrameNumber + 1)
            ElseIf ImageUtil.IsTiff(sFileName) Then
                oPictureBox.Image = ImageUtil.GetFrameFromTiff(sFileName, iFrameNumber)
            End If
        End If
        oPictureBox.Update()
        'Cursor.Current = Cursors.Default
        Return oPictureBox.Image
    End Function

    Private Sub InitPageRange()
        mPDFPageCount = ImageUtil.GetImageFrameCount(mPDFFileName)
        mCurrentPageNumber = 1
    End Sub

    Private Sub InitBookmarks()
        TreeView1.Nodes.Clear()
        Dim HasBookmarks As Boolean = False
        Try
            If mUseXPDF Then
                HasBookmarks = AFPDFLibUtil.FillTree(TreeView1, mPDFDoc)
                AddHandler TreeView1.BeforeExpand, AddressOf AFPDFLib_BeforeExpand
                AddHandler TreeView1.NodeMouseClick, AddressOf AFPDFLib_NodeMouseClick
            Else
                HasBookmarks = iTextSharpUtil.BuildBookmarkTreeFromPDF(mOriginalFileName, TreeView1.Nodes)
                AddHandler TreeView1.NodeMouseClick, AddressOf ItextSharp_NodeMouseClick
            End If
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

    Private Sub InitBottomToolbar(ByVal Mode As String)
        If Mode = "TIFF" Then
            btSearch.Visible = False
            btNext.Visible = False
            tbSearchText.Visible = False
            ToolStripSeparator5.Visible = False
            tsExport.Visible = True
            tsExport.ToolTipText = "Export TIFF file to the PDF file format"
        ElseIf Mode = "GS" Then
            btSearch.Visible = False
            btNext.Visible = False
            tbSearchText.Visible = False
            ToolStripSeparator5.Visible = False
            tsExport.Visible = False
        ElseIf Mode = "XPDF" Then
            btSearch.Visible = True
            btNext.Visible = True
            tbSearchText.Visible = True
            ToolStripSeparator5.Visible = True
            tsExport.Visible = True
            tsExport.ToolTipText = "Export PDF to another file format"
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

    Public Enum ViewMode
        FIT_TO_SCREEN
        FIT_WIDTH
        ACTUAL_SIZE
    End Enum



    Private Sub InitializePageView(Optional ByVal Mode As ViewMode = ViewMode.FIT_TO_SCREEN)
        Dim myFlowLayoutPanel As New FlowLayoutPanel
        Panel1.SuspendLayout()
        Panel1.Controls.Clear()
        myFlowLayoutPanel.Dock = DockStyle.Fill
        myFlowLayoutPanel.FlowDirection = FlowDirection.TopDown
        myFlowLayoutPanel.AutoScroll = True
        myFlowLayoutPanel.Width = Panel1.Width - 20
        myFlowLayoutPanel.Height = Panel1.Height - 20
        myFlowLayoutPanel.WrapContents = False
        If mContinuousPages Then
            AddHandler myFlowLayoutPanel.Scroll, AddressOf FlowPanel_Scroll
            For i As Integer = 1 To mPDFPageCount
                Dim ObjPictureBox As New PictureBox
                ObjPictureBox.Name = i.ToString
                ObjPictureBox.SizeMode = PictureBoxSizeMode.Zoom
                ObjPictureBox.Height = myFlowLayoutPanel.Height - 14
                ObjPictureBox.Width = myFlowLayoutPanel.Width - 14
                ObjPictureBox.Location = New Point(0, 0)
                AddHandler ObjPictureBox.MouseUp, AddressOf picImage_MouseUp
                AddHandler ObjPictureBox.MouseDown, AddressOf picImage_MouseDown
                AddHandler ObjPictureBox.MouseMove, AddressOf picImage_MouseMove
                'ObjPictureBox.Image = New System.Drawing.Bitmap(1, 1)
                'ShowImageFromFile(mPDFFileName, i - 1, ObjPictureBox)
                myFlowLayoutPanel.Controls.Add(ObjPictureBox)
            Next
            Dim EndPictureBox As New PictureBox
            EndPictureBox.Name = "0"
            EndPictureBox.Height = 1
            EndPictureBox.Width = 1
            EndPictureBox.Location = New Point(0, 0)
            myFlowLayoutPanel.Controls.Add(EndPictureBox)
        Else
            Dim ObjPictureBox As New PictureBox
            ObjPictureBox.Name = "0"
            ObjPictureBox.SizeMode = PictureBoxSizeMode.Zoom
            ObjPictureBox.Height = myFlowLayoutPanel.Height - 14
            ObjPictureBox.Width = myFlowLayoutPanel.Width - 14
            ObjPictureBox.Location = New Point(0, 0)
            AddHandler ObjPictureBox.MouseUp, AddressOf picImage_MouseUp
            AddHandler ObjPictureBox.MouseDown, AddressOf picImage_MouseDown
            AddHandler ObjPictureBox.MouseMove, AddressOf picImage_MouseMove
            myFlowLayoutPanel.Controls.Add(ObjPictureBox)
        End If
        ApplyToAllPictureBoxes(myFlowLayoutPanel, Mode)
        Panel1.Controls.Add(myFlowLayoutPanel)
        Panel1.ResumeLayout()
        ScrollUnitsPerPage = FindFlowLayoutPanel().VerticalScroll.Maximum / mPDFPageCount
        PageInitDone = True
    End Sub

    Private Sub ApplyZoom(ByVal Mode As ViewMode)
        If Mode = ViewMode.FIT_TO_SCREEN Then
            FindPictureBox(0).Height = FindPictureBox(0).Parent.ClientSize.Height - 14
            FindPictureBox(0).Width = FindPictureBox(0).Parent.ClientSize.Width - 14
        ElseIf Mode = ViewMode.FIT_WIDTH And Not Nothing Is FindPictureBox(0).Image Then
            FindPictureBox(0).Width = FindPictureBox(0).Parent.ClientSize.Width - 18
            Dim ScaleAmount As Double = (FindPictureBox(0).Width / FindPictureBox(0).Image.Width)
            FindPictureBox(0).Height = CInt(FindPictureBox(0).Image.Height * ScaleAmount)
        ElseIf Mode = ViewMode.ACTUAL_SIZE And Not Nothing Is FindPictureBox(0).Image Then
            FindPictureBox(0).Width = FindPictureBox(0).Image.Width
            FindPictureBox(0).Height = FindPictureBox(0).Image.Height
        End If
        tscbZoom.Text = GetCurrentScalePercentage() & " %"
    End Sub

    Private Delegate Sub ShowImage(ByVal sFileName As String, ByVal iFrameNumber As Integer, ByRef oPictureBox As PictureBox, ByVal XPDFDPI As Integer)

    Private Sub FlowPanel_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        ScrollBarPosition = e.NewValue()
        Dim ImagesWereLoaded As Boolean = False
        For i As Integer = 0 To 1
            Dim pageNumber As Integer = (System.Math.Floor(ScrollBarPosition / ScrollUnitsPerPage) + 1) + i
            If pageNumber >= 1 And pageNumber <= mPDFPageCount Then
                If Nothing Is FindPictureBox(pageNumber).Image Then
                    ShowImageFromFile(mPDFFileName, pageNumber - 1, FindPictureBox(pageNumber), 72)
                    ImagesWereLoaded = True
                End If
            End If
        Next
        If ImagesWereLoaded Then
            Dim lastPage As Integer = System.Math.Floor(ScrollBarPosition / ScrollUnitsPerPage) + 2
            If lastPage > mPDFPageCount Then
                lastPage = mPDFPageCount
            End If
            Dim firstPage As Integer = System.Math.Floor(ScrollBarPosition / ScrollUnitsPerPage) + 1
            If firstPage < 1 Then
                firstPage = 1
            End If
            ClearAllPictureBoxes(firstPage, lastPage)
        End If
        mCurrentPageNumber = System.Math.Floor(ScrollBarPosition / ScrollUnitsPerPage) + 1
        UpdatePageLabel()
    End Sub

    Private Sub UpdatePageLabel()
        tsPageLabel.Text = "Page " & mCurrentPageNumber & " of " & mPDFPageCount
        tsPageNum.Text = mCurrentPageNumber
    End Sub

    Private Sub DisplayCurrentPage()
        CheckPageBounds()
        UpdatePageLabel()
        ShowImageFromFile(mPDFFileName, mCurrentPageNumber - 1, FindPictureBox(mCurrentPageNumber))
        If mContinuousPages Then
            FindFlowLayoutPanel().ScrollControlIntoView(FindPictureBox(mCurrentPageNumber))
            ClearAllPictureBoxes(mCurrentPageNumber, mCurrentPageNumber)
        End If
    End Sub

    Private Function GetCurrentScalePercentage() As Integer
        GetCurrentScalePercentage = 0
        If Not Nothing Is FindPictureBox(0).Image Then
            Dim OriginalWidth As Integer = FindPictureBox(0).Image.Width
            Dim CurrentWidth As Integer = FindPictureBox(0).Width
            GetCurrentScalePercentage = CInt((CurrentWidth / OriginalWidth) * 100)
        End If
    End Function

    Private Function FindPictureBox(ByVal PageNumber As Integer) As PictureBox
        For Each oControl In Panel1.Controls
            For Each childControl In oControl.Controls
                If TypeOf childControl Is PictureBox Then
                    If mContinuousPages = True And childControl.Name = PageNumber.ToString Then
                        Return childControl
                    ElseIf mContinuousPages = False And childControl.Name = "0" Then
                        Return childControl
                    End If
                End If
            Next
        Next
    End Function

    Private Sub ClearAllPictureBoxes(ByVal StartingPageNumber As Integer, ByVal EndingPageNumber As Integer)
        Dim PagesToKeep As New List(Of String)
        PagesToKeep.Add("0")
        For i As Integer = StartingPageNumber To EndingPageNumber
            PagesToKeep.Add(i.ToString)
        Next
        For Each oControl In Panel1.Controls
            For Each childControl In oControl.Controls
                If TypeOf childControl Is PictureBox And Not PagesToKeep.Contains(childControl.Name) Then
                    childControl.Image = Nothing
                End If
            Next
        Next
        GC.Collect()
    End Sub

    Private Sub ApplyToAllPictureBoxes(ByRef oFlowLAyoutPanel As FlowLayoutPanel, ByVal Mode As ViewMode)
        Dim dummyPictureBox As New PictureBox
        ShowImageFromFile(mPDFFileName, mCurrentPageNumber - 1, dummyPictureBox)
        For Each childControl In oFlowLAyoutPanel.Controls
            If TypeOf childControl Is PictureBox Then
                If Mode = ViewMode.FIT_TO_SCREEN Then
                    childControl.Height = childControl.Parent.ClientSize.Height - 14
                    childControl.Width = childControl.Parent.ClientSize.Width - 14
                ElseIf Mode = ViewMode.FIT_WIDTH And Not Nothing Is dummyPictureBox.Image Then
                    childControl.Width = childControl.Parent.ClientSize.Width - 18
                    Dim ScaleAmount As Double = (childControl.Width / dummyPictureBox.Image.Width)
                    childControl.Height = CInt(dummyPictureBox.Image.Height * ScaleAmount)
                ElseIf Mode = ViewMode.ACTUAL_SIZE And Not Nothing Is dummyPictureBox.Image Then
                    childControl.Width = dummyPictureBox.Image.Width
                    childControl.Height = dummyPictureBox.Image.Height
                End If
            End If
        Next
        GC.Collect()
    End Sub

    Private Function FindFlowLayoutPanel() As FlowLayoutPanel
        For Each oControl In Panel1.Controls
            If TypeOf oControl Is FlowLayoutPanel Then
                Return oControl
            End If
        Next
    End Function

    Private Sub FitToScreen()
        If mContinuousPages Then
            InitializePageView(ViewMode.FIT_TO_SCREEN)
            DisplayCurrentPage()
        Else
            ApplyZoom(ViewMode.FIT_TO_SCREEN)
        End If
        UpdatePageLabel()
    End Sub

    Private Function TrapKey(ByVal KCode As String) As Boolean
        If (KCode >= 48 And KCode <= 57) Or KCode = 8 Then
            TrapKey = False
        Else
            TrapKey = True
        End If
    End Function

#End Region

#Region "XPDF specific events"

    Private Sub AFPDFLib_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs)
        Dim ol As OutlineItem = DirectCast(e.Node.Tag, OutlineItem)
        If ol IsNot Nothing Then
            Dim ret As Long = ol.DoAction()
            Select Case ol.GetKind()
                Case LinkActionKind.actionGoTo, LinkActionKind.actionGoToR
                    If Not mContinuousPages Then
                        ScrolltoTop(CInt(ret))
                    End If
                    Exit Select
                Case LinkActionKind.actionLaunch
                    Exit Select
                Case LinkActionKind.actionMovie
                    Exit Select
                Case LinkActionKind.actionURI
                    Exit Select
            End Select
            mCurrentPageNumber = mPDFDoc.CurrentPage
            DisplayCurrentPage()
        End If
    End Sub

    Private Sub ScrolltoTop(ByVal y As Integer)
        Dim dr As Point = FindFlowLayoutPanel().AutoScrollPosition
        If mPDFDoc.PageHeight > FindFlowLayoutPanel().Height Then
            dr.Y = y * (GetCurrentScalePercentage() / 100)
        End If
        FindFlowLayoutPanel().AutoScrollPosition = dr
    End Sub

    Private Sub AFPDFLib_BeforeExpand(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs)

        Dim ol As OutlineItem = DirectCast(e.Node.Tag, OutlineItem)
        If ol IsNot Nothing Then

            If e.Node.Nodes.Count > 0 AndAlso e.Node.Nodes(0).Text = "dummy" Then
                e.Node.Nodes.Clear()
                For Each col As OutlineItem In ol.Childrens
                    Dim tn As New TreeNode(col.Title)
                    tn.Tag = col
                    If col.KidsCount > 0 Then
                        tn.Nodes.Add(New TreeNode("dummy"))
                    End If
                    e.Node.Nodes.Add(tn)
                Next
            End If
        End If
    End Sub

    Private Sub btSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSearch.Click
        Dim res As Integer = 0
        Dim searchArgs As New SearchArgs(tbSearchText.Text, True, False, True, False, False)
        res = SearchCallBack(sender, searchArgs)
    End Sub

    Private Sub btNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNext.Click
        Dim res As Integer = 0
        res = SearchCallBack(sender, New SearchArgs(tbSearchText.Text, FromBeginning, False, True, True, False))
        FromBeginning = False
        If res = 0 Then
            If MessageBox.Show("No results were found. Would you like to start from the beginning?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                FromBeginning = True
                btNext_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Function SearchCallBack(ByVal sender As Object, ByVal e As SearchArgs) As Integer
        Cursor.Current = Cursors.WaitCursor
        Dim lFound As Integer = 0
        If mPDFDoc IsNot Nothing Then
            mPDFDoc.SearchCaseSensitive = e.Exact

            If e.FromBegin Then
                lFound = mPDFDoc.FindFirst(e.Text, If(e.WholeDoc, PDFLibNet.PDFSearchOrder.PDFSearchFromdBegin, PDFLibNet.PDFSearchOrder.PDFSearchFromCurrent), e.Up)
            ElseIf e.FindNext Then

                If e.Up Then
                    lFound = mPDFDoc.FindPrevious(e.Text)
                Else
                    lFound = mPDFDoc.FindNext(e.Text)

                End If
            Else


                lFound = mPDFDoc.FindText(e.Text, mPDFDoc.CurrentPage, (If(e.WholeDoc, PDFLibNet.PDFSearchOrder.PDFSearchFromdBegin, PDFLibNet.PDFSearchOrder.PDFSearchFromCurrent)), e.Exact, e.Up, True, _
                 e.WholeDoc)
            End If
            If lFound > 0 Then

                mPDFDoc.CurrentPage = mPDFDoc.SearchResults(0).Page
                mCurrentPageNumber = mPDFDoc.CurrentPage
                DisplayCurrentPage()
                If Not mContinuousPages Then
                    FocusSearchResult(mPDFDoc.SearchResults(0))
                End If
            End If
        End If
        Return lFound
        Cursor.Current = Cursors.Default
    End Function

    Private Sub FocusSearchResult(ByVal res As PDFLibNet.PDFSearchResult)
        Dim dr As Point = FindFlowLayoutPanel.AutoScrollPosition
        If mPDFDoc.PageWidth > FindFlowLayoutPanel.Width Then
            dr.X = res.Position.Left - FindFlowLayoutPanel.Width - FindFlowLayoutPanel.AutoScrollMargin.Width
        End If
        If mPDFDoc.PageHeight > FindFlowLayoutPanel.Height Then
            dr.Y = res.Position.Top - FindFlowLayoutPanel.Height - FindFlowLayoutPanel.AutoScrollMargin.Height
        End If

        FindFlowLayoutPanel.AutoScrollPosition = dr
    End Sub


#End Region

#Region "ITextSharp specific events"

    Private Sub ItextSharp_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs)
        If e.Node.ImageKey <> "" Then
            mCurrentPageNumber = e.Node.ImageKey
            DisplayCurrentPage()
        End If
    End Sub

#End Region

#Region "Panning the image with left mouse click held down"

    Private Sub picImage_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Capture the initial point 	
        Cursor = Cursors.Hand
        m_PanStartPoint = New Point(e.X, e.Y)
    End Sub

    Private Sub picImage_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Capture the initial point 	
        Cursor = Cursors.Default
    End Sub

    Private Sub picImage_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Verify Left Button is pressed while the mouse is moving	
        If e.Button = Windows.Forms.MouseButtons.Left Then
            'Here we get the change in coordinates.		
            Dim DeltaX As Integer = (m_PanStartPoint.X - e.X)
            Dim DeltaY As Integer = (m_PanStartPoint.Y - e.Y)
            'Then we set the new autoscroll position.		
            'ALWAYS pass positive integers to the panels autoScrollPosition method		
            sender.Parent.AutoScrollPosition = New Drawing.Point((DeltaX - sender.Parent.AutoScrollPosition.X), (DeltaY - sender.Parent.AutoScrollPosition.Y))
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

