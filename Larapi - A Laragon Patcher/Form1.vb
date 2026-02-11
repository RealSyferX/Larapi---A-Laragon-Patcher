Imports System.IO
Imports Microsoft.Win32

' Larapi - A Laragon Patcher
' This tool patches Laragon to remove ads, license checks, and integrity/hash validation
' Features:
' - Hash/Integrity Check Bypass: Bypasses MD5/SHA file integrity validation
' - Windows API Hash Bypass: Intercepts CryptCreateHash, CryptHashData, BCryptHash, and memcmp calls
' - License Validation Bypass: Removes license key checks
' - Ad Removal: Eliminates all advertisement popups and nags
'
' Windows Crypto API functions targeted:
' - CryptCreateHash, CryptHashData, CryptGetHashParam (legacy Crypto API)
' - BCryptHash, BCryptFinishHash (Cryptography API: Next Generation)
' - memcmp/strcmp for hash comparison bypass

Public Class Faceless
    Public Shared Function PatchAOB(filePath As String, originalPattern As String, patchPattern As String) As Boolean
        Try
            Dim fileBytes As Byte() = File.ReadAllBytes(filePath)

            Dim findMask As Byte?() = AOBStringToBytesWithWildcards(originalPattern)
            Dim patchMask As Byte?() = AOBStringToBytesWithWildcards(patchPattern)

            If findMask.Length <> patchMask.Length Then
                Throw New Exception("AOB lengths must match!")
            End If

            Dim index As Integer = FindPatternWithWildcards(fileBytes, findMask)
            If index = -1 Then Return False

            For i As Integer = 0 To patchMask.Length - 1
                If patchMask(i).HasValue Then
                    fileBytes(index + i) = patchMask(i).Value
                End If
            Next

            File.WriteAllBytes(filePath, fileBytes)
            Return True
        Catch ex As Exception
            MessageBox.Show("Patch failed: " & ex.Message)
            Return False
        End Try
    End Function

    Private Shared Function AOBStringToBytesWithWildcards(aob As String) As Byte?()
        Dim tokens = aob.Trim().Split(" "c)
        Dim result As New List(Of Byte?)(tokens.Length)

        For Each token In tokens
            If token = "??" OrElse token = "?" Then
                result.Add(Nothing)
            Else
                result.Add(Convert.ToByte(token, 16))
            End If
        Next

        Return result.ToArray()
    End Function

    Private Shared Function FindPatternWithWildcards(buffer As Byte(), pattern As Byte?()) As Integer
        For i As Integer = 0 To buffer.Length - pattern.Length
            Dim matched As Boolean = True
            For j As Integer = 0 To pattern.Length - 1
                If pattern(j).HasValue AndAlso buffer(i + j) <> pattern(j).Value Then
                    matched = False
                    Exit For
                End If
            Next
            If matched Then Return i
        Next
        Return -1
    End Function



    Public Sub SetLaragonLicense()
        Try
            Dim keyPath As String = "Software\Laragon"
            Dim valueName As String = "license.key"
            Dim valueData As String = "ACF5-44F7-A927-38B074625C55|531395EF-F61C-EC11-810D-E070EAEF3C24"

            Using regKey As RegistryKey = Registry.CurrentUser.CreateSubKey(keyPath)
                If regKey IsNot Nothing Then
                    regKey.SetValue(valueName, valueData, RegistryValueKind.String)
                    RichTextBox1.AppendText("License key written to registry successfully!" + vbNewLine)
                Else
                    RichTextBox1.AppendText("Failed to open registry key!" + vbNewLine)
                End If
            End Using

        Catch ex As Exception
            RichTextBox1.AppendText("Failed to write license key to registry: " & ex.Message + vbNewLine)
        End Try
    End Sub

    Public Sub GoPatch()
        Dim targetPath As String = "C:\laragon\laragon.exe"

        RichTextBox1.AppendText("Patching Laragon..." + vbNewLine)
        SetLaragonLicense()
        'Exception Error Patch
        ''If Not Faceless.PatchAOB(targetPath, "C3 48 89 E5 48 8D A4 24 70 FF", "55 48 89 E5 48 8D A4 24 70 FF") Then RichTextBox1.AppendText("Exception Error Patch Failed" + vbNewLine)
        ''Buggy As Hell

        'No Ads Patch
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 66 90 EB ?? 66 90 48 ?? ?? ?? 48 8B",
                                 "90 90 90 90 90 66 90 EB ?? 66 90 48 ?? ?? ?? 48 8B") Then
            RichTextBox1.AppendText("No Ads Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads Patch Success" + vbNewLine)
        End If

        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 84 C0 75 ?? 48 83", "90 90 90 90 90 E8 ?? ?? ?? ?? 84 C0 75 ?? 48 83") Then
            RichTextBox1.AppendText("No Ads 2 Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads 2 Patch Success" + vbNewLine)
        End If

        If Not Faceless.PatchAOB(targetPath, "55 48 ?? ?? 48 ?? ?? ?? ?? 48 ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? ?? 90 80 ?? ?? ?? ?? ?? ?? 75",
                                 "C3 48 ?? ?? 48 ?? ?? ?? ?? 48 ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? ?? 90 80 ?? ?? ?? ?? ?? ?? 75") Then
            RichTextBox1.AppendText("No Ads 3 Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads 3 Patch Success" + vbNewLine)
        End If

        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 90 48 ?? ?? ?? 5D C3 00 00 00 00 00 00 00 00 00 55 ?? ?? ?? 48 ?? ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? E8 ?? ?? ?? ?? 90 48 ?? ?? ?? 5D C3",
                                 "E8 ?? ?? ?? ?? 90 48 ?? ?? ?? 5D C3 00 00 00 00 00 00 00 00 00 C3 ?? ?? ?? 48 ?? ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? E8 ?? ?? ?? ?? 90 48 ?? ?? ?? 5D C3") Then
            RichTextBox1.AppendText("No Ads 4 Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("No Ads 4 Patch Success" + vbNewLine)
        End If


        'Invalid Key Patch - Universal for all Laragon versions
        If Not Faceless.PatchAOB(targetPath, "7D ?? 48 ?? ?? ?? ?? ?? ?? E8 ?? ?? ?? ?? EB ?? 48 ?? ?? ?? 48",
                                 "75 ?? 48 ?? ?? ?? ?? ?? ?? E8 ?? ?? ?? ?? EB ?? 48 ?? ?? ?? 48") Then
            RichTextBox1.AppendText("Invalid Key Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("Invalid Key Patch Success" + vbNewLine)
        End If


        'Your License data is not valid Patch
        If Not Faceless.PatchAOB(targetPath, "0F 84 ?? ?? ?? ?? 48 ?? ?? ?? 48 ?? ?? ?? E8 ?? ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? 48 ?? ?? ?? E8 ?? ?? ?? ?? 48",
                                 "0F 85 ?? ?? ?? ?? 48 ?? ?? ?? 48 ?? ?? ?? E8 ?? ?? ?? ?? 48 ?? ?? ?? ?? ?? ?? 48 ?? ?? ?? E8 ?? ?? ?? ?? 48") Then
            RichTextBox1.AppendText("Your License data is not valid Patch Failed" + vbNewLine)
        Else
            RichTextBox1.AppendText("Your License data is not valid Patch Success" + vbNewLine)
        End If


        'Hash/Integrity Check Bypass Patch 1 - MD5/SHA hash comparison bypass
        'This patches the conditional jump after hash comparison to always succeed
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 84 C0 0F 84 ?? ?? ?? ?? 48 ?? ?? ?? E8",
                                 "E8 ?? ?? ?? ?? 84 C0 90 E9 ?? ?? ?? ?? 48 ?? ?? ?? E8") Then
            RichTextBox1.AppendText("Hash Check Bypass 1 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Hash Check Bypass 1 Patch Success" + vbNewLine)
        End If

        'Hash/Integrity Check Bypass Patch 2 - Skip file integrity validation
        'Common pattern for file integrity check with JE (jump if equal) -> JNE (jump if not equal)
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 85 C0 74 ?? 48 ?? ?? ?? 48 ?? ?? E8",
                                 "E8 ?? ?? ?? ?? 85 C0 75 ?? 48 ?? ?? ?? 48 ?? ?? E8") Then
            RichTextBox1.AppendText("Hash Check Bypass 2 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Hash Check Bypass 2 Patch Success" + vbNewLine)
        End If

        'Hash/Integrity Check Bypass Patch 3 - Return early from hash validation function
        'This patches a function to return immediately without performing hash validation
        'Changed to NOP out the function call itself rather than modifying the prologue
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 48 89 C? 48 85 ?? 74 ?? 48 8B",
                                 "90 90 90 90 90 48 89 C? 48 85 ?? EB ?? 48 8B") Then
            RichTextBox1.AppendText("Hash Check Bypass 3 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Hash Check Bypass 3 Patch Success" + vbNewLine)
        End If

        'Hash/Integrity Check Bypass Patch 4 - Skip hash verification check
        'Changes conditional jump to unconditional jump to skip hash verification
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 48",
                                 "90 90 90 90 90 48 85 C0 90 E9 ?? ?? ?? ?? 48") Then
            RichTextBox1.AppendText("Hash Check Bypass 4 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Hash Check Bypass 4 Patch Success" + vbNewLine)
        End If

        'Windows API Hash Check Bypass Patch 1 - CryptCreateHash/CryptHashData bypass
        'NOPs out calls to Windows Crypto API functions that create hash objects
        'Pattern looks for CALL instruction followed by hash validation logic
        If Not Faceless.PatchAOB(targetPath, "FF 15 ?? ?? ?? ?? 85 C0 74 ?? 48 8D ?? ?? ?? 48 8D",
                                 "90 90 ?? ?? ?? ?? 85 C0 EB ?? 48 8D ?? ?? ?? 48 8D") Then
            RichTextBox1.AppendText("Windows API Hash Bypass 1 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Windows API Hash Bypass 1 Patch Success" + vbNewLine)
        End If

        'Windows API Hash Check Bypass Patch 2 - CryptGetHashParam result check bypass
        'Bypasses the check of hash parameter retrieval results
        'Changes JE (jump if equal/zero) to JNE (jump if not equal) after API call
        If Not Faceless.PatchAOB(targetPath, "FF 15 ?? ?? ?? ?? 85 C0 0F 84 ?? ?? ?? ?? 48 8B",
                                 "FF 15 ?? ?? ?? ?? 85 C0 0F 85 ?? ?? ?? ?? 48 8B") Then
            RichTextBox1.AppendText("Windows API Hash Bypass 2 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Windows API Hash Bypass 2 Patch Success" + vbNewLine)
        End If

        'Windows API Hash Check Bypass Patch 3 - BCrypt hash comparison bypass
        'Bypasses BCryptHash or BCryptFinishHash validation (newer Crypto API)
        'NOPs out the API call and forces success path
        If Not Faceless.PatchAOB(targetPath, "FF 15 ?? ?? ?? ?? 89 ?? 85 C0 0F 8? ?? ?? ?? ?? 48",
                                 "90 90 ?? ?? ?? ?? 89 ?? 31 C0 90 90 ?? ?? ?? ?? 48") Then
            RichTextBox1.AppendText("Windows API Hash Bypass 3 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Windows API Hash Bypass 3 Patch Success" + vbNewLine)
        End If

        'Windows API Hash Check Bypass Patch 4 - memcmp/strcmp hash comparison bypass
        'Bypasses hash string comparison after calculation
        'Forces comparison to always return 0 (equal) by XORing EAX with itself
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 85 C0 75 ?? 48 8B ?? ?? 48 85",
                                 "E8 ?? ?? ?? ?? 31 C0 90 90 48 8B ?? ?? 48 85") Then
            RichTextBox1.AppendText("Windows API Hash Bypass 4 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Windows API Hash Bypass 4 Patch Success" + vbNewLine)
        End If

        'Windows API Hash Check Bypass Patch 5 - Direct hash buffer comparison bypass
        'Bypasses direct memory comparison of calculated hash vs expected hash
        'Replaces TEST/JE with XOR EAX,EAX/NOP to force success
        If Not Faceless.PatchAOB(targetPath, "E8 ?? ?? ?? ?? 84 C0 74 ?? 48 8D ?? ?? E8",
                                 "E8 ?? ?? ?? ?? 31 C0 EB ?? 48 8D ?? ?? E8") Then
            RichTextBox1.AppendText("Windows API Hash Bypass 5 Patch Failed (pattern not found - may not be needed)" + vbNewLine)
        Else
            RichTextBox1.AppendText("Windows API Hash Bypass 5 Patch Success" + vbNewLine)
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
