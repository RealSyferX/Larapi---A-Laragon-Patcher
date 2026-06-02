
# Laragon License Bypass - Patch Documentation

![image](https://github.com/RealSyferX/Larapi---A-Laragon-Patcher/blob/main/lol.png?raw=true)

## Binary Information
- **Target:** `laragon.exe`
- **Architecture:** x86_64 (PE64)
- **Base:** 0x140000000

## Patch Summary

Total **13 patches** across license verification, initialization, and UI display.

---

## License Verification Patches (`verify_license_button_click`)

| # | RVA | Offset | Original | Patched | Description |
|---|-----|--------|----------|---------|-------------|
| 1 | 0x1000BEB70 | 0xBEB70 | Multiple bytes | `B0 01 C3 90 90` | Format check always returns true (`mov al,1; ret`) |
| 2 | 0x1000BE6CC | 0xBE6CC | `E8 xx xx xx xx` | `90 90 90 90 90` | Skip curl API call to api.laragon.org (NOP call) |
| 3 | 0x1000BE79E | 0xBE79E | `74 54` | `90 90` | Force success path regardless of API response (NOP jle) |
| 4 | 0x1000BE7AF | 0xBE7AF | `E8 xx xx xx xx` | `90 90 90 90 90` | Skip JSON response parser (NOP call) |
| 5 | 0x1000BE806 | 0xBE806 | `E8 xx xx xx xx` | `90 90 90 90 90` | Skip UUID processing function (NOP call) |
| 6 | 0x1000BE816 | 0xBE816 | `E8 xx xx xx xx` | `90 90 90 90 90` | Skip license save to file (NOP call) |
| 7 | 0x1000BE822 | 0xBE822 | `E8 xx xx xx xx` | `90 90 90 90 90` | Skip UI update call (NOP call) |
| 8 | 0x1000BE832 | 0xBE832 | `E8 xx xx xx xx` | `90 90 90 90 90` | Skip form refresh call (NOP call) |
| 9 | 0x1000BE837 | 0xBE837 | `xx xx` | `EB 17` | JMP over AAD0 check (prevents null crash) |
| 10 | 0x1000BE850 | 0xBE850 | `xx xx` | `EB 12` | JMP directly to success message display |

---

## Initialization & License Check Patches

| # | RVA | Offset | Original | Patched | Description |
|---|-----|--------|----------|---------|-------------|
| 11 | 0x1000C01F0 | 0xC01F0 | Multiple bytes | `B0 01 C3` | Init check always returns true (prevents "Initialization error") |
| 12 | 0x100061AC0 | 0x61AC0 | Multiple bytes | `31 C0 C3` | License file check always returns false (`xor eax,eax; ret` - prevents crash when no license.key file exists) |

---

## UI/UX Modifications

| # | RVA | Offset | Original | Patched | Description |
|---|-----|--------|----------|---------|-------------|
| 13 | 0x1003B7208 | 0x37208 | `"All set! Thank you so much."` | `"Premium License Activated!"` | Changed success message text |

---

## How It Works

### Verification Flow
1. **Format check bypassed** - Any license string accepted
2. **Network call skipped** - No API request to api.laragon.org
3. **Response validation skipped** - No JSON parsing
4. **Success forced** - Direct jump to success path
5. **Data processing skipped** - UUID, save, UI update all NOP'd
6. **Message displayed** - Shows "Premium License Activated!"
7. **Flag set** - `byte_100603BA0` set to 1, unlocking premium features

### Initialization
- Startup checks bypassed to prevent "Initialization error" dialog
- License file validation disabled (returns false = no file, safe path)

---

## Files Modified
- `laragon.exe` - Main executable with all patches applied directly

## Notes
- All patches use NOP (90) or short JMP (EB xx) for minimal footprint
- No code caves or additional sections required
- `byte_100603BA0` acts as the master license flag (0 = free, 1 = premium)

---

## Verification
After applying patches:
1. Any license key format is accepted
2. "Premium License Activated!" message displays
3. Premium features unlock automatically
4. No network calls made during verification
