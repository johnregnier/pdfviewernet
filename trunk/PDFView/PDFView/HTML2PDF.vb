Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing

Public Class HTML2PDF

  Dim _bitmap As Bitmap

  Public Function URLToPDF(ByVal url As String, ByVal outputFilename As String) As Boolean
    Return GetPDFOfWebsite(url, outputFilename)
  End Function

  Public Function HTMLToPDF(ByVal htmlFile As String, ByVal outputFilename As String) As Boolean
    Return GetPDFOfWebsite(htmlFile, outputFilename)
  End Function

  Public Function GetImageOfWebsite(ByVal url As String, ByVal outPAth As String) As String
    Using browser As New WebBrowser
      browser.Width = 1024
      browser.ScrollBarsEnabled = False
      browser.Navigate(url)
      AddHandler browser.DocumentCompleted, AddressOf browser_DocumentCompleted
      While browser.ReadyState <> WebBrowserReadyState.Complete
        Application.DoEvents()
      End While
    End Using
    Dim output As String = outPAth & "\" & Now.Ticks.ToString & ".png"
    _bitmap.Save(output, Imaging.ImageFormat.Png)
    Return output
  End Function

  Private Sub browser_DocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
    Dim browser As WebBrowser = DirectCast(sender, WebBrowser)
    Dim myRect As New Rectangle(0, 0, browser.Document.Window.Size.Width, browser.Document.Window.Size.Height)
    Dim mySize As New Size(browser.Document.Window.Size.Width, browser.Document.Window.Size.Height)
    browser.ClientSize = mySize
    browser.ScrollBarsEnabled = False
    _bitmap = New Bitmap(browser.Document.Window.Size.Width, browser.Document.Window.Size.Height)
    browser.BringToFront()
    browser.DrawToBitmap(_bitmap, myRect)
    '_bitmap = DirectCast(_bitmap.GetThumbnailImage(browser.ClientSize.Width, browser.ClientSize.Height, Nothing, IntPtr.Zero), Bitmap)
  End Sub

  '  <DllImport("wkhtmltopdf.dll", EntryPoint:="main", CallingConvention:=CallingConvention.StdCall, CharSet:=CharSet.Ansi)> _
  'Private Shared Function HtmlToPDF(ByVal argc As Integer, <[In]()> ByVal argv As IntPtr()) As Integer
  '  End Function

  'Public Function GetPDFOfWebsite(ByVal url As String, ByVal outPath As String) As String
  '  Dim myArgs(1) As String
  '  myArgs(0) = url
  '  myArgs(1) = outPath
  '  Try
  '    HTMLToPDF(InteropHelper.MakeInteropArgs(myArgs).Length, InteropHelper.MakeInteropArgs(myArgs))
  '  Catch ex As Exception
  '  End Try
  '  Return IO.File.Exists(outPath)
  'End Function

  Public Function GetPDFOfWebsite(ByVal url As String, ByVal outPath As String) As String
    Dim appPath As String = System.Windows.Forms.Application.StartupPath
    CmdHelper.ExecuteCMD(appPath & "\wkhtmltopdf.exe", String.Format("""{0}"" ""{1}""", url, outPath))
    Return IO.File.Exists(outPath)
  End Function


End Class
