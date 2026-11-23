param(
    [Parameter(Mandatory = $true)][string]$TargetPath
)

$signtool = Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin\*\x64\signtool.exe" -ErrorAction SilentlyContinue |
    Sort-Object FullName -Descending | Select-Object -First 1

if (-not $signtool) {
    Write-Warning "signtool.exe not found, skipping code signing."
    exit 0
}

$pfx = "$env:USERPROFILE\codesign\PycCodeTools.pfx"
$pwdFile = "$env:USERPROFILE\codesign\pfx-password.txt"

if (-not (Test-Path $pfx -ErrorAction SilentlyContinue) -or -not (Test-Path $pwdFile -ErrorAction SilentlyContinue)) {
    Write-Warning "Code signing certificate not found at $pfx, skipping code signing."
    exit 0
}

$pfxPassword = (Get-Content -Path $pwdFile -Raw).Trim()

& $signtool.FullName sign /fd SHA256 /f $pfx /p $pfxPassword /tr http://timestamp.digicert.com /td SHA256 $TargetPath
if ($LASTEXITCODE -ne 0) {
    Write-Warning "Code signing failed for $TargetPath (signtool exit code $LASTEXITCODE)."
}

exit 0
