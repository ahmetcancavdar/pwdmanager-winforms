# Rebuilds the single-file exe into .\publish and (re)creates the Desktop shortcut.
# Run after code changes:  powershell -ExecutionPolicy Bypass -File .\publish-and-shortcut.ps1

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$publishDir = Join-Path $root 'publish'
$exe = Join-Path $publishDir 'PwdManager.WinForms.exe'

Write-Host 'Publishing...' -ForegroundColor Cyan
dotnet publish (Join-Path $root 'src/PwdManager.WinForms') -c Release -r win-x64 `
    --self-contained false -p:PublishSingleFile=true -p:DebugType=none -o $publishDir

$lnk = [Environment]::GetFolderPath('Desktop') + '\PwdManager.lnk'
$ws = New-Object -ComObject WScript.Shell
$sc = $ws.CreateShortcut($lnk)
$sc.TargetPath = $exe
$sc.WorkingDirectory = $publishDir
$sc.Description = 'PwdManager - Sifre Yonetimi'
$sc.IconLocation = "$exe,0"
$sc.Save()

Write-Host "Done. Shortcut: $lnk" -ForegroundColor Green
