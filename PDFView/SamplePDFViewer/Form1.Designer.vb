<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
    Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
    Me.Panel1 = New System.Windows.Forms.Panel
    Me.btOCR = New System.Windows.Forms.Button
    Me.Panel3 = New System.Windows.Forms.Panel
    Me.rbMuPDF = New System.Windows.Forms.RadioButton
    Me.cbPre = New System.Windows.Forms.CheckBox
    Me.rbGS = New System.Windows.Forms.RadioButton
    Me.rbXPDF = New System.Windows.Forms.RadioButton
    Me.TextBox1 = New System.Windows.Forms.TextBox
    Me.Button1 = New System.Windows.Forms.Button
    Me.Panel2 = New System.Windows.Forms.Panel
    Me.PdfViewer1 = New PDFView.PDFViewer
    Me.Panel1.SuspendLayout()
    Me.Panel3.SuspendLayout()
    Me.Panel2.SuspendLayout()
    Me.SuspendLayout()
    '
    'OpenFileDialog1
    '
    Me.OpenFileDialog1.FileName = "OpenFileDialog1"
    '
    'Panel1
    '
    Me.Panel1.Controls.Add(Me.btOCR)
    Me.Panel1.Controls.Add(Me.Panel3)
    Me.Panel1.Controls.Add(Me.TextBox1)
    Me.Panel1.Controls.Add(Me.Button1)
    Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
    Me.Panel1.Location = New System.Drawing.Point(0, 0)
    Me.Panel1.Name = "Panel1"
    Me.Panel1.Size = New System.Drawing.Size(681, 30)
    Me.Panel1.TabIndex = 0
    '
    'btOCR
    '
    Me.btOCR.Location = New System.Drawing.Point(255, 3)
    Me.btOCR.Name = "btOCR"
    Me.btOCR.Size = New System.Drawing.Size(49, 23)
    Me.btOCR.TabIndex = 3
    Me.btOCR.Text = "OCR"
    Me.btOCR.UseVisualStyleBackColor = True
    '
    'Panel3
    '
    Me.Panel3.Controls.Add(Me.rbMuPDF)
    Me.Panel3.Controls.Add(Me.cbPre)
    Me.Panel3.Controls.Add(Me.rbGS)
    Me.Panel3.Controls.Add(Me.rbXPDF)
    Me.Panel3.Location = New System.Drawing.Point(307, 3)
    Me.Panel3.Name = "Panel3"
    Me.Panel3.Size = New System.Drawing.Size(285, 23)
    Me.Panel3.TabIndex = 2
    '
    'rbMuPDF
    '
    Me.rbMuPDF.AutoSize = True
    Me.rbMuPDF.Location = New System.Drawing.Point(62, 2)
    Me.rbMuPDF.Name = "rbMuPDF"
    Me.rbMuPDF.Size = New System.Drawing.Size(60, 17)
    Me.rbMuPDF.TabIndex = 3
    Me.rbMuPDF.Text = "muPDF"
    Me.rbMuPDF.UseVisualStyleBackColor = True
    '
    'cbPre
    '
    Me.cbPre.AutoSize = True
    Me.cbPre.Checked = True
    Me.cbPre.CheckState = System.Windows.Forms.CheckState.Checked
    Me.cbPre.Location = New System.Drawing.Point(207, 4)
    Me.cbPre.Name = "cbPre"
    Me.cbPre.Size = New System.Drawing.Size(77, 17)
    Me.cbPre.TabIndex = 2
    Me.cbPre.Text = "PreRender"
    Me.cbPre.UseVisualStyleBackColor = True
    '
    'rbGS
    '
    Me.rbGS.AutoSize = True
    Me.rbGS.Location = New System.Drawing.Point(128, 3)
    Me.rbGS.Name = "rbGS"
    Me.rbGS.Size = New System.Drawing.Size(80, 17)
    Me.rbGS.TabIndex = 1
    Me.rbGS.Text = "GhostScript"
    Me.rbGS.UseVisualStyleBackColor = True
    '
    'rbXPDF
    '
    Me.rbXPDF.AutoSize = True
    Me.rbXPDF.Checked = True
    Me.rbXPDF.Location = New System.Drawing.Point(3, 3)
    Me.rbXPDF.Name = "rbXPDF"
    Me.rbXPDF.Size = New System.Drawing.Size(53, 17)
    Me.rbXPDF.TabIndex = 0
    Me.rbXPDF.Text = "XPDF"
    Me.rbXPDF.UseVisualStyleBackColor = True
    '
    'TextBox1
    '
    Me.TextBox1.Enabled = False
    Me.TextBox1.Location = New System.Drawing.Point(4, 4)
    Me.TextBox1.Name = "TextBox1"
    Me.TextBox1.Size = New System.Drawing.Size(249, 20)
    Me.TextBox1.TabIndex = 1
    '
    'Button1
    '
    Me.Button1.Location = New System.Drawing.Point(598, 3)
    Me.Button1.Name = "Button1"
    Me.Button1.Size = New System.Drawing.Size(75, 23)
    Me.Button1.TabIndex = 0
    Me.Button1.Text = "Browse..."
    Me.Button1.UseVisualStyleBackColor = True
    '
    'Panel2
    '
    Me.Panel2.Controls.Add(Me.PdfViewer1)
    Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.Panel2.Location = New System.Drawing.Point(0, 30)
    Me.Panel2.Name = "Panel2"
    Me.Panel2.Size = New System.Drawing.Size(681, 434)
    Me.Panel2.TabIndex = 1
    '
    'PdfViewer1
    '
    Me.PdfViewer1.AllowBookmarks = True
    Me.PdfViewer1.AllowGhostScriptPreRendering = True
    Me.PdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.PdfViewer1.FileName = Nothing
    Me.PdfViewer1.Location = New System.Drawing.Point(0, 0)
    Me.PdfViewer1.Name = "PdfViewer1"
    Me.PdfViewer1.Size = New System.Drawing.Size(681, 434)
    Me.PdfViewer1.TabIndex = 0
    Me.PdfViewer1.UseMuPDF = False
    Me.PdfViewer1.UseXPDF = True
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(681, 464)
    Me.Controls.Add(Me.Panel2)
    Me.Controls.Add(Me.Panel1)
    Me.MinimumSize = New System.Drawing.Size(555, 500)
    Me.Name = "Form1"
    Me.Text = "Free PDF .NET Viewer"
    Me.Panel1.ResumeLayout(False)
    Me.Panel1.PerformLayout()
    Me.Panel3.ResumeLayout(False)
    Me.Panel3.PerformLayout()
    Me.Panel2.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbGS As System.Windows.Forms.RadioButton
    Friend WithEvents rbXPDF As System.Windows.Forms.RadioButton
  Friend WithEvents PdfViewer1 As PDFView.PDFViewer
  Friend WithEvents btOCR As System.Windows.Forms.Button
  Friend WithEvents cbPre As System.Windows.Forms.CheckBox
  Friend WithEvents rbMuPDF As System.Windows.Forms.RadioButton

End Class
