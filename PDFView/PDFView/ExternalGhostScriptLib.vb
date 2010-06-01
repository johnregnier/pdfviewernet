Public Class ExternalGhostScriptLib

  Public Shared Function GetPageFromPDF(ByVal inputFileName As String, ByVal PageNumber As Integer, Optional ByVal DPI As Integer = 150, Optional ByVal Password As String = "") As System.Drawing.Image
    GetPageFromPDF = Nothing
    Dim output As String = System.IO.Path.GetTempPath & Now.Ticks & ".png"
    Dim appPath As String = System.Windows.Forms.Application.StartupPath
    CmdHelper.ExecuteCMD(appPath & "\GSCommandLine.exe", String.Format("""{0}"" ""{1}"" {2} {3} {4}", inputFileName, output, PageNumber, DPI, Password))
    If System.IO.File.Exists(output) Then
      Try
        GetPageFromPDF = New System.Drawing.Bitmap(output)
      Catch ex As Exception
      End Try
      ImageUtil.DeleteFile(output)
    End If
  End Function

  Public Shared Sub KillAllGSProcesses()
    SyncLock ProcessList
      For Each item As Integer In ProcessList
        Try
          Dim myProcess As Process = System.Diagnostics.Process.GetProcessById(item)
          myProcess.Kill()
        Catch ex As Exception
          'Possible for process to exit before we try to kill it
        End Try
      Next
      ProcessList.Clear()
    End SyncLock
  End Sub


End Class
