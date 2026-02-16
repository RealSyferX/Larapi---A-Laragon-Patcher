# Build Instructions

## Prerequisites

- .NET SDK 10.0 or later
- Visual Studio 2017 or later (for Windows), or .NET SDK with VB support (for Linux/Mac)

## Building on Windows

1. Open `Larapi - A Laragon Patcher.sln` in Visual Studio
2. Select "Release" configuration
3. Build -> Build Solution (or press F6)
4. The compiled executable will be in `Larapi - A Laragon Patcher/bin/Release/Larapi - A Laragon Patcher.exe`

## Building on Linux/Mac

1. Install .NET SDK 10.0 or later
2. Navigate to the repository directory
3. Run the following commands:

```bash
dotnet restore "Larapi - A Laragon Patcher.sln"
dotnet build "Larapi - A Laragon Patcher.sln" --configuration Release
```

4. The compiled executable will be in `Larapi - A Laragon Patcher/bin/Release/Larapi - A Laragon Patcher.exe`

## Notes

- The project targets .NET Framework 4.7.2
- On non-Windows platforms, the Microsoft.NETFramework.ReferenceAssemblies package is used to provide the necessary reference assemblies
- The compiled executable is a Windows PE32 executable and requires Windows or Wine to run
- Administrator privileges are required to run the patcher as it modifies system files

## Output

The build produces:
- `Larapi - A Laragon Patcher.exe` - The main executable (≈37 KB)
- `Larapi - A Laragon Patcher.pdb` - Debug symbols
- `System.Resources.Extensions.dll` - Required dependency
- `Larapi - A Laragon Patcher.exe.config` - Configuration file
