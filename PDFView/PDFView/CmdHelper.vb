Public Class CmdHelper

  Public Shared Sub ExecuteCMD(ByVal cmd As String, ByVal args As String)
    Using myProcess As New Process()
      myProcess.StartInfo.FileName = cmd
      myProcess.StartInfo.Arguments = args
      myProcess.StartInfo.UseShellExecute = False
      myProcess.StartInfo.CreateNoWindow = True
      myProcess.Start()
      SyncLock ProcessList
        ProcessList.Add(myProcess.Id)
      End SyncLock
      myProcess.PriorityClass = ProcessPriorityClass.Normal
      myProcess.WaitForExit()
      SyncLock ProcessList
        ProcessList.Remove(myProcess.Id)
      End SyncLock
    End Using
  End Sub

End Class
