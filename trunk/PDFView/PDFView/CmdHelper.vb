Public Class CmdHelper

  Public Shared Sub ExecuteCMD(ByVal cmd As String, ByVal args As String)
    Using myProcess As New Process()
      myProcess.StartInfo.FileName = cmd
      myProcess.StartInfo.Arguments = args
      myProcess.StartInfo.UseShellExecute = False
      myProcess.StartInfo.CreateNoWindow = True
      myProcess.Start()
      ProcessList.Add(myProcess.Id)
      myProcess.PriorityClass = ProcessPriorityClass.Normal
      myProcess.WaitForExit()
      ProcessList.Remove(myProcess.Id)
    End Using
  End Sub

End Class
