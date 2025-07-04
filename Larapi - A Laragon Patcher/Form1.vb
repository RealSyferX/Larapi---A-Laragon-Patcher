Imports System.IO
Public Class Faceless
    Public Shared Function PatchAOB(filePath As String, originalPattern As String, patchPattern As String) As Boolean
        Try
            Dim fileBytes As Byte() = File.ReadAllBytes(filePath)
            Dim findBytes As Byte() = AOBStringToBytes(originalPattern)
            Dim replaceBytes As Byte() = AOBStringToBytes(patchPattern)

            If findBytes.Length <> replaceBytes.Length Then
                Throw New Exception("AOB lengths must match!")
            End If

            Dim index As Integer = FindPattern(fileBytes, findBytes)
            If index = -1 Then Return False

            Array.Copy(replaceBytes, 0, fileBytes, index, replaceBytes.Length)
            File.WriteAllBytes(filePath, fileBytes)
            Return True
        Catch ex As Exception
            MessageBox.Show("Patch failed: " & ex.Message)
            Return False
        End Try
    End Function

    Private Shared Function AOBStringToBytes(aob As String) As Byte()
        Return aob.Trim().Split(" "c).Select(Function(b) Convert.ToByte(b, 16)).ToArray()
    End Function

    Private Shared Function FindPattern(buffer As Byte(), pattern As Byte()) As Integer
        For i As Integer = 0 To buffer.Length - pattern.Length
            Dim matched As Boolean = True
            For j As Integer = 0 To pattern.Length - 1
                If buffer(i + j) <> pattern(j) Then
                    matched = False
                    Exit For
                End If
            Next
            If matched Then Return i
        Next
        Return -1
    End Function

    Public Sub GoPatch()
        Dim targetPath As String = "C:\laragon\laragon.exe"

        RichTextBox1.AppendText("Patching Laragon..." + vbNewLine)

        'Exception Error Patch
        ''If Not Faceless.PatchAOB(targetPath, "C3 48 89 E5 48 8D A4 24 70 FF", "55 48 89 E5 48 8D A4 24 70 FF") Then RichTextBox1.AppendText("Exception Error Patch Failed" + vbNewLine)
        ''Buggy As Hell

        'No Ads Patch
        ''NO FUCKING ANNOYING ADS IN LARAGON ANYMORE POPUP IN EVERY 5 MINUTES
        If Not Faceless.PatchAOB(targetPath, "E8 1A EC 04 00 66 90 EB 56 66", "90 90 90 90 90 66 90 EB 56 66") Then
            RichTextBox1.AppendText("No Ads Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads Patch Success" + vbNewLine)
        End If
        If Not Faceless.PatchAOB(targetPath, "E8 EE 84 04 00 E8 59 CD 03 00", "90 90 90 90 90 E8 59 CD 03 00") Then
            RichTextBox1.AppendText("No Ads Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads 2 Patch Success" + vbNewLine)
        End If

        If Not Faceless.PatchAOB(targetPath, "E8 B3 E2 FD FF 0F 1F 00 E8 4B", "90 90 90 90 90 0F 1F 00 E8 4B") Then
            RichTextBox1.AppendText("No Ads Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads 3 Patch Success" + vbNewLine)
        End If

        If Not Faceless.PatchAOB(targetPath, "74 4F 48 8D 4D F0 48 8D 15 CC", "75 4F 48 8D 4D F0 48 8D 15 CC") Then
            RichTextBox1.AppendText("No Ads Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads 4 Patch Success" + vbNewLine)
        End If

        'Invalid Key Patch
        If Not Faceless.PatchAOB(targetPath, "7D 0E 48 8D 0D 8B 62 2F 00 E8", "75 0E 48 8D 0D 8B 62 2F 00 E8") Then
            RichTextBox1.AppendText("Invalid Key Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("Invalid Key Patch Success" + vbNewLine)
        End If

        'Your License data is not valid Patch
        If Not Faceless.PatchAOB(targetPath, "0F 84 84 00 00 00 48 8B 55 B0", "0F 85 84 00 00 00 48 8B 55 B0") Then
            RichTextBox1.AppendText("Your License data is not valid Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("Your License data is not valid Patch Success" + vbNewLine)
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RichTextBox1.Clear()
        Dim targetExe As String = "C:\laragon\laragon.exe"
        Dim extractDir As String = "C:\Laragon\usr"

        If File.Exists(targetExe) Then
            GoPatch()

            ' Ensure directory exists
            Directory.CreateDirectory(extractDir)

            ' Extract license.info
            Dim infoPath As String = Path.Combine(extractDir, "license.info")
            File.WriteAllBytes(infoPath, My.Resources.licenseinfo)
            RichTextBox1.AppendText("Extracted: " & infoPath + vbNewLine)

            ' Extract license.key
            Dim keyPath As String = Path.Combine(extractDir, "license.key")
            File.WriteAllBytes(keyPath, My.Resources.licensekey)
            RichTextBox1.AppendText("Extracted: " & keyPath + vbNewLine)

            MessageBox.Show("Patching completed successfully!", "Larapi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Target file not found: " & targetExe, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MessageBox.Show("Just press that Patch button", "Larapi - A Laragon Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
        MessageBox.Show("Once you're done, just enjoy. No more ads & shit.", "Larapi - A Laragon Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
