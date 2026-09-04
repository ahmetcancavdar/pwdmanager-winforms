# Rebuilds the single-file exe into .\publish and (re)creates the Desktop shortcut.
# Also generates Assets\logo.ico from the logo image so the exe / shortcut / taskbar
# all use the app logo.
# Run after code changes:  powershell -ExecutionPolicy Bypass -File .\publish-and-shortcut.ps1

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$publishDir = Join-Path $root 'publish'
$exe = Join-Path $publishDir 'PwdManager.WinForms.exe'
$assets = Join-Path $root 'src\PwdManager.WinForms\Assets'
$ico = Join-Path $assets 'logo.ico'

# --- generate Assets\logo.ico from the logo image (PNG-framed, multi-size) ----------
function New-IcoFromImage([string]$srcPath, [string]$destPath) {
    Add-Type -AssemblyName System.Drawing
    $src = [System.Drawing.Image]::FromFile($srcPath)
    try {
        $isPng = [System.IO.Path]::GetExtension($srcPath).ToLower() -eq '.png'
        $sizes = 256, 128, 64, 48, 32, 16
        $frames = New-Object System.Collections.Generic.List[byte[]]

        foreach ($s in $sizes) {
            $bmp = New-Object System.Drawing.Bitmap $s, $s
            $g = [System.Drawing.Graphics]::FromImage($bmp)
            $g.InterpolationMode  = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
            $g.SmoothingMode      = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
            $g.PixelOffsetMode    = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
            $g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
            if (-not $isPng) { $g.Clear([System.Drawing.Color]::White) } else { $g.Clear([System.Drawing.Color]::Transparent) }

            $ratio = [Math]::Min($s / $src.Width, $s / $src.Height)
            $w = [int][Math]::Round($src.Width  * $ratio)
            $h = [int][Math]::Round($src.Height * $ratio)
            $pad = if ($isPng) { 0 } else { [int]($s * 0.10) }
            $w2 = [Math]::Max(1, $w - $pad); $h2 = [Math]::Max(1, $h - $pad)
            $g.DrawImage($src, [int](($s - $w2) / 2), [int](($s - $h2) / 2), $w2, $h2)
            $g.Dispose()

            $ms = New-Object System.IO.MemoryStream
            $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
            $frames.Add($ms.ToArray())
            $bmp.Dispose(); $ms.Dispose()
        }
    } finally { $src.Dispose() }

    $out = New-Object System.IO.MemoryStream
    $bw = New-Object System.IO.BinaryWriter($out)
    $bw.Write([UInt16]0); $bw.Write([UInt16]1); $bw.Write([UInt16]$frames.Count)
    $offset = 6 + 16 * $frames.Count
    for ($i = 0; $i -lt $frames.Count; $i++) {
        $s = $sizes[$i]
        $byte = if ($s -ge 256) { [byte]0 } else { [byte]$s }
        $bw.Write($byte); $bw.Write($byte)         # width, height
        $bw.Write([byte]0); $bw.Write([byte]0)     # palette, reserved
        $bw.Write([UInt16]1); $bw.Write([UInt16]32)# planes, bpp
        $bw.Write([UInt32]$frames[$i].Length)
        $bw.Write([UInt32]$offset)
        $offset += $frames[$i].Length
    }
    foreach ($f in $frames) { $bw.Write($f) }
    $bw.Flush()
    [System.IO.File]::WriteAllBytes($destPath, $out.ToArray())
    $bw.Dispose(); $out.Dispose()
}

$logoImg = Get-ChildItem -Path $assets -File -ErrorAction SilentlyContinue |
    Where-Object { $_.Extension -match '^\.(png|jpg|jpeg|bmp|gif)$' } |
    Sort-Object { switch ($_.Name.ToLower()) { 'logo.png' {0} 'logo.jpg' {1} default {5} } }, Name |
    Select-Object -First 1

if ($logoImg) {
    Write-Host "Generating icon from $($logoImg.Name) ..." -ForegroundColor Cyan
    New-IcoFromImage $logoImg.FullName $ico
} else {
    Write-Host 'No logo image in Assets\ — using the default icon.' -ForegroundColor Yellow
}

# --- publish -----------------------------------------------------------------------
Write-Host 'Publishing...' -ForegroundColor Cyan
dotnet publish (Join-Path $root 'src/PwdManager.WinForms') -c Release -r win-x64 `
    --self-contained false -p:PublishSingleFile=true -p:DebugType=none -o $publishDir

# --- desktop shortcut ------------------------------------------------------------------
$lnk = [Environment]::GetFolderPath('Desktop') + '\PwdManager.lnk'
$ws = New-Object -ComObject WScript.Shell
$sc = $ws.CreateShortcut($lnk)
$sc.TargetPath = $exe
$sc.WorkingDirectory = $publishDir
$sc.Description = 'PwdManager - Sifre Yonetimi'
if (Test-Path (Join-Path $publishDir 'Assets\logo.ico')) {
    $sc.IconLocation = (Join-Path $publishDir 'Assets\logo.ico') + ',0'
} else {
    $sc.IconLocation = "$exe,0"
}
$sc.Save()

Write-Host "Done. Shortcut: $lnk" -ForegroundColor Green
