<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExportImageOptions
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbPDF = New System.Windows.Forms.RadioButton
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.cbPageSize = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.nuDown = New System.Windows.Forms.NumericUpDown
        Me.Label2 = New System.Windows.Forms.Label
        Me.nuStart = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.btCancel = New System.Windows.Forms.Button
        Me.btOK = New System.Windows.Forms.Button
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.nuDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nuStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbPDF)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(260, 49)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File Format"
        '
        'rbPDF
        '
        Me.rbPDF.AutoSize = True
        Me.rbPDF.Checked = True
        Me.rbPDF.Location = New System.Drawing.Point(7, 19)
        Me.rbPDF.Name = "rbPDF"
        Me.rbPDF.Size = New System.Drawing.Size(46, 17)
        Me.rbPDF.TabIndex = 0
        Me.rbPDF.TabStop = True
        Me.rbPDF.Tag = "Portable Document Format (*.pdf)|*.pdf"
        Me.rbPDF.Text = "PDF"
        Me.rbPDF.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cbPageSize)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 67)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(260, 53)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "PDF Options"
        '
        'cbPageSize
        '
        Me.cbPageSize.FormattingEnabled = True
        Me.cbPageSize.Location = New System.Drawing.Point(70, 21)
        Me.cbPageSize.Name = "cbPageSize"
        Me.cbPageSize.Size = New System.Drawing.Size(121, 21)
        Me.cbPageSize.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Page Size:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.nuDown)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.nuStart)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 126)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(260, 57)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Page Range"
        '
        'nuDown
        '
        Me.nuDown.Location = New System.Drawing.Point(168, 24)
        Me.nuDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nuDown.Name = "nuDown"
        Me.nuDown.Size = New System.Drawing.Size(48, 20)
        Me.nuDown.TabIndex = 3
        Me.nuDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(120, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "to page"
        '
        'nuStart
        '
        Me.nuStart.Location = New System.Drawing.Point(65, 24)
        Me.nuStart.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nuStart.Name = "nuStart"
        Me.nuStart.Size = New System.Drawing.Size(48, 20)
        Me.nuStart.TabIndex = 1
        Me.nuStart.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "From page"
        '
        'btCancel
        '
        Me.btCancel.Location = New System.Drawing.Point(116, 189)
        Me.btCancel.Name = "btCancel"
        Me.btCancel.Size = New System.Drawing.Size(75, 23)
        Me.btCancel.TabIndex = 8
        Me.btCancel.Text = "Cancel"
        Me.btCancel.UseVisualStyleBackColor = True
        '
        'btOK
        '
        Me.btOK.Location = New System.Drawing.Point(197, 189)
        Me.btOK.Name = "btOK"
        Me.btOK.Size = New System.Drawing.Size(75, 23)
        Me.btOK.TabIndex = 7
        Me.btOK.Text = "OK"
        Me.btOK.UseVisualStyleBackColor = True
        '
        'ExportImageOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 218)
        Me.Controls.Add(Me.btCancel)
        Me.Controls.Add(Me.btOK)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "ExportImageOptions"
        Me.Text = "Export Image Options"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.nuDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nuStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbPDF As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cbPageSize As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents nuDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nuStart As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btCancel As System.Windows.Forms.Button
    Friend WithEvents btOK As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
End Class
