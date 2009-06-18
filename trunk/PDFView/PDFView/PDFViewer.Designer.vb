<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PDFViewer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PDFViewer))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsPageLabel = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsPrevious = New System.Windows.Forms.ToolStripButton
        Me.tsNext = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel
        Me.tsPageNum = New System.Windows.Forms.ToolStripTextBox
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsZoomOut = New System.Windows.Forms.ToolStripButton
        Me.tsZoomIn = New System.Windows.Forms.ToolStripButton
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel
        Me.tscbZoom = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.tsPrint = New System.Windows.Forms.ToolStripButton
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.tsRotateCC = New System.Windows.Forms.ToolStripButton
        Me.tsRotateC = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsPrint, Me.ToolStripSeparator4, Me.tsPageLabel, Me.ToolStripSeparator1, Me.tsPrevious, Me.tsNext, Me.ToolStripSeparator2, Me.ToolStripLabel2, Me.tsPageNum, Me.ToolStripSeparator3, Me.tsZoomOut, Me.tsZoomIn, Me.tsRotateCC, Me.tsRotateC, Me.ToolStripLabel3, Me.tscbZoom})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(543, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsPageLabel
        '
        Me.tsPageLabel.Name = "tsPageLabel"
        Me.tsPageLabel.Size = New System.Drawing.Size(112, 22)
        Me.tsPageLabel.Text = "Page Number 1 of 1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsPrevious
        '
        Me.tsPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsPrevious.Image = CType(resources.GetObject("tsPrevious.Image"), System.Drawing.Image)
        Me.tsPrevious.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsPrevious.Name = "tsPrevious"
        Me.tsPrevious.Size = New System.Drawing.Size(23, 22)
        Me.tsPrevious.Text = "ToolStripButton1"
        Me.tsPrevious.ToolTipText = "Previous Page"
        '
        'tsNext
        '
        Me.tsNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsNext.Image = CType(resources.GetObject("tsNext.Image"), System.Drawing.Image)
        Me.tsNext.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsNext.Name = "tsNext"
        Me.tsNext.Size = New System.Drawing.Size(23, 22)
        Me.tsNext.Text = "ToolStripButton2"
        Me.tsNext.ToolTipText = "Next Page"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(65, 22)
        Me.ToolStripLabel2.Text = "Go to page"
        '
        'tsPageNum
        '
        Me.tsPageNum.Name = "tsPageNum"
        Me.tsPageNum.Size = New System.Drawing.Size(40, 25)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tsZoomOut
        '
        Me.tsZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsZoomOut.Image = CType(resources.GetObject("tsZoomOut.Image"), System.Drawing.Image)
        Me.tsZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsZoomOut.Name = "tsZoomOut"
        Me.tsZoomOut.Size = New System.Drawing.Size(23, 22)
        Me.tsZoomOut.Text = "ToolStripButton3"
        Me.tsZoomOut.ToolTipText = "Zoom Out"
        '
        'tsZoomIn
        '
        Me.tsZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsZoomIn.Image = CType(resources.GetObject("tsZoomIn.Image"), System.Drawing.Image)
        Me.tsZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsZoomIn.Name = "tsZoomIn"
        Me.tsZoomIn.Size = New System.Drawing.Size(23, 22)
        Me.tsZoomIn.Text = "ToolStripButton4"
        Me.tsZoomIn.ToolTipText = "Zoom In"
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(10, 22)
        Me.ToolStripLabel3.Text = " "
        '
        'tscbZoom
        '
        Me.tscbZoom.Items.AddRange(New Object() {"Fit To Screen", "Fit To Width", "Actual Size"})
        Me.tscbZoom.Name = "tscbZoom"
        Me.tscbZoom.Size = New System.Drawing.Size(100, 25)
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tsPrint
        '
        Me.tsPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsPrint.Image = CType(resources.GetObject("tsPrint.Image"), System.Drawing.Image)
        Me.tsPrint.ImageTransparentColor = System.Drawing.Color.Black
        Me.tsPrint.Name = "tsPrint"
        Me.tsPrint.Size = New System.Drawing.Size(23, 22)
        Me.tsPrint.Text = "ToolStripButton1"
        Me.tsPrint.ToolTipText = "Print"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel3})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 404)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(543, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(0, 17)
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(426, 370)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(111, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(429, 373)
        Me.Panel1.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TreeView1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 25)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(543, 379)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'TreeView1
        '
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView1.Location = New System.Drawing.Point(3, 3)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(102, 373)
        Me.TreeView1.TabIndex = 2
        '
        'tsRotateCC
        '
        Me.tsRotateCC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsRotateCC.Image = CType(resources.GetObject("tsRotateCC.Image"), System.Drawing.Image)
        Me.tsRotateCC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsRotateCC.Name = "tsRotateCC"
        Me.tsRotateCC.Size = New System.Drawing.Size(23, 22)
        Me.tsRotateCC.Text = "Rotate the page counterclockwise"
        '
        'tsRotateC
        '
        Me.tsRotateC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsRotateC.Image = CType(resources.GetObject("tsRotateC.Image"), System.Drawing.Image)
        Me.tsRotateC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsRotateC.Name = "tsRotateC"
        Me.tsRotateC.Size = New System.Drawing.Size(23, 22)
        Me.tsRotateC.Text = "Rotate the page clockwise"
        '
        'PDFViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "PDFViewer"
        Me.Size = New System.Drawing.Size(543, 426)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
  Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
  Friend WithEvents tsPageLabel As System.Windows.Forms.ToolStripLabel
  Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents tsPrevious As System.Windows.Forms.ToolStripButton
  Friend WithEvents tsNext As System.Windows.Forms.ToolStripButton
  Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
  Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
  Friend WithEvents tsPageNum As System.Windows.Forms.ToolStripTextBox
  Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents tsZoomOut As System.Windows.Forms.ToolStripButton
  Friend WithEvents tsZoomIn As System.Windows.Forms.ToolStripButton
  Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
  Friend WithEvents tscbZoom As System.Windows.Forms.ToolStripComboBox
  Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
  Friend WithEvents tsPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsRotateCC As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsRotateC As System.Windows.Forms.ToolStripButton

End Class
