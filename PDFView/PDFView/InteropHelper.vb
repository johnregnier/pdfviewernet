Imports System.Runtime.InteropServices

Public Class InteropHelper

  Private Shared Function StringToByteArray(ByVal s As String) As IntPtr
    Dim p As New IntPtr()
    Dim b As Byte() = New Byte(s.Length) {}
    Dim i As Integer
    For i = 0 To s.Length - 1
      b(i) = CByte(AscW(s.ToCharArray()(i)))
    Next
    b(s.Length) = 0
    p = Marshal.AllocCoTaskMem(s.Length + 1)
    Marshal.Copy(b, 0, p, s.Length + 1)
    Return p
  End Function

  Public Shared Function MakeInteropArgs(ByVal strArray As String()) As IntPtr()
    Dim argv(strArray.Length) As IntPtr
    argv(0) = StringToByteArray("appname")
    For i As Integer = 0 To strArray.Length - 1
      argv(i + 1) = StringToByteArray(strArray(i))
    Next
    Return argv
  End Function

End Class
