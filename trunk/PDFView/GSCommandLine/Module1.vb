Module Module1

  Sub Main()
    Dim myPDFConverter As GhostScriptCmd = New GhostScriptCmd
    Dim myArgs As String() = Environment.GetCommandLineArgs()
    Dim myPassword As String = ""
    Dim myDPI As Integer = 200
    If myArgs.Length > 4 Then
      myDPI = CInt(myArgs(4))
    End If
    If myArgs.Length > 5 Then
      myPassword = myArgs(5)
    End If
    Try
      myPDFConverter.WritePageFromPDF(myArgs(1), myArgs(2), CInt(myArgs(3)), myDPI, myPassword, False)
    Catch ex As Exception
      For Each item As String In myArgs
        Console.WriteLine(item)
      Next
      Console.WriteLine(ex.ToString)
    End Try
  End Sub

End Module
