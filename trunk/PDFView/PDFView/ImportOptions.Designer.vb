<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportOptions
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
    Me.rbURL = New System.Windows.Forms.RadioButton
    Me.rbHtml = New System.Windows.Forms.RadioButton
    Me.rbImages = New System.Windows.Forms.RadioButton
    Me.GroupBox2 = New System.Windows.Forms.GroupBox
    Me.tbUrl = New System.Windows.Forms.TextBox
    Me.Label1 = New System.Windows.Forms.Label
    Me.btImport = New System.Windows.Forms.Button
    Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
    Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
    Me.GroupBox1.SuspendLayout()
    Me.GroupBox2.SuspendLayout()
    Me.SuspendLayout()
    '
    'GroupBox1
    '
    Me.GroupBox1.Controls.Add(Me.rbURL)
    Me.GroupBox1.Controls.Add(Me.rbHtml)
    Me.GroupBox1.Controls.Add(Me.rbImages)
    Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(350, 96)
    Me.GroupBox1.TabIndex = 0
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Import Source"
    '
    'rbURL
    '
    Me.rbURL.AutoSize = True
    Me.rbURL.Location = New System.Drawing.Point(7, 66)
    Me.rbURL.Name = "rbURL"
    Me.rbURL.Size = New System.Drawing.Size(77, 17)
    Me.rbURL.TabIndex = 2
    Me.rbURL.TabStop = True
    Me.rbURL.Text = "URL String"
    Me.rbURL.UseVisualStyleBackColor = True
    '
    'rbHtml
    '
    Me.rbHtml.AutoSize = True
    Me.rbHtml.Location = New System.Drawing.Point(7, 43)
    Me.rbHtml.Name = "rbHtml"
    Me.rbHtml.Size = New System.Drawing.Size(62, 17)
    Me.rbHtml.TabIndex = 1
    Me.rbHtml.TabStop = True
    Me.rbHtml.Text = "Html file"
    Me.rbHtml.UseVisualStyleBackColor = True
    '
    'rbImages
    '
    Me.rbImages.AutoSize = True
    Me.rbImages.Location = New System.Drawing.Point(7, 20)
    Me.rbImages.Name = "rbImages"
    Me.rbImages.Size = New System.Drawing.Size(75, 17)
    Me.rbImages.TabIndex = 0
    Me.rbImages.TabStop = True
    Me.rbImages.Text = "Image files"
    Me.rbImages.UseVisualStyleBackColor = True
    '
    'GroupBox2
    '
    Me.GroupBox2.Controls.Add(Me.tbUrl)
    Me.GroupBox2.Controls.Add(Me.Label1)
    Me.GroupBox2.Location = New System.Drawing.Point(16, 117)
    Me.GroupBox2.Name = "GroupBox2"
    Me.GroupBox2.Size = New System.Drawing.Size(346, 57)
    Me.GroupBox2.TabIndex = 1
    Me.GroupBox2.TabStop = False
    Me.GroupBox2.Text = "Import Options"
    '
    'tbUrl
    '
    Me.tbUrl.Location = New System.Drawing.Point(56, 25)
    Me.tbUrl.Name = "tbUrl"
    Me.tbUrl.Size = New System.Drawing.Size(284, 20)
    Me.tbUrl.TabIndex = 1
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(18, 28)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(32, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "URL:"
    '
    'btImport
    '
    Me.btImport.Location = New System.Drawing.Point(287, 183)
    Me.btImport.Name = "btImport"
    Me.btImport.Size = New System.Drawing.Size(75, 23)
    Me.btImport.TabIndex = 2
    Me.btImport.Text = "Import"
    Me.btImport.UseVisualStyleBackColor = True
    '
    'OpenFileDialog1
    '
    Me.OpenFileDialog1.FileName = "OpenFileDialog1"
    '
    'ImportOptions
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(374, 212)
    Me.Controls.Add(Me.btImport)
    Me.Controls.Add(Me.GroupBox2)
    Me.Controls.Add(Me.GroupBox1)
    Me.Name = "ImportOptions"
    Me.Text = "Import Options"
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    Me.GroupBox2.ResumeLayout(False)
    Me.GroupBox2.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents rbImages As System.Windows.Forms.RadioButton
  Friend WithEvents rbURL As System.Windows.Forms.RadioButton
  Friend WithEvents rbHtml As System.Windows.Forms.RadioButton
  Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
  Friend WithEvents tbUrl As System.Windows.Forms.TextBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents btImport As System.Windows.Forms.Button
  Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
End Class
