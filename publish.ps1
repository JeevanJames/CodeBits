[CmdletBinding()]
Param(
    [string]$Package,
    [string]$Version
)

If (-Not $Package -Or -Not $Version) {
    Write-Host "USAGE:"
    Write-Host "  ./deploy.ps1 -package <package name> -version <version>"
    exit 1
}

$NuGetDir = "./nuget/"
$OutputDir = "${NuGetDir}output/"

If (Test-Path -Path $OutputDir) {
    # Delete all subdirectories and root files recursively.
    # Sometimes Powershell has an issue deleting all items, including the 
    Get-ChildItem $OutputDir -Recurse | Remove-Item -Force

    # Delete the now empty directory
    Remove-Item -LiteralPath "$OutputDir" -Force -Recurse
}
New-Item -ItemType Directory -Path $OutputDir

dotnet build -c Release
Write-Host $LastExitCode
if ($LastExitCode -ne "0") {
    Write-Host "Build failed. Aborting."
    exit 1
}

gomplate -d data=file://$PWD/nuget/${Package}.json -f ./nuget/Package.nuspec.t -o ${OutputDir}${Package}.nuspec

nuget pack ${OutputDir}${Package}.nuspec -Version ${Version} -OutputDirectory ${OutputDir}
nuget push ${OutputDir}CodeBits.${Package}.${Version}.nupkg -Source https://api.nuget.org/v3/index.json
