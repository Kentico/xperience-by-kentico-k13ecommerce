param([switch]$Stop)

$k13Cs = 'Data Source=(localdb)\Kentico;Initial Catalog=Kentico13_DancingGoat;Integrated Security=True;Persist Security Info=False;Connect Timeout=60;Encrypt=False;Current Language=English;'
$xbkCs = 'Data Source=(localdb)\Kentico;Initial Catalog=XByK_DancingGoat_K13Ecommerce;Integrated Security=True;Persist Security Info=False;Connect Timeout=60;Encrypt=False;Current Language=English;'

Get-Job -Name K13, XbK -ErrorAction SilentlyContinue | Stop-Job -PassThru | Remove-Job -Force

if ($Stop) { Write-Host "Stopped"; return }

$repoRoot = Split-Path -Parent $PSScriptRoot

Start-Job -Name K13 -ScriptBlock {
    param($cs, $root)
    $env:ConnectionStrings__CMSConnectionString = $cs
    Set-Location (Join-Path $root 'publish\K13')
    dotnet .\Kentico13_DancingGoat.dll 2>&1
} -ArgumentList $k13Cs, $repoRoot | Out-Null

Start-Job -Name XbK -ScriptBlock {
    param($cs, $root)
    $env:ConnectionStrings__CMSConnectionString = $cs
    Set-Location (Join-Path $root 'publish\XbK')
    dotnet .\DancingGoat.dll 2>&1
} -ArgumentList $xbkCs, $repoRoot | Out-Null

Write-Host "Jobs started"
Get-Job | Format-Table Id, Name, State -AutoSize
