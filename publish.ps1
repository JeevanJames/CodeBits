[CmdletBinding()]
Param(
    [string]$Package,
    [string]$Version
)

If (-Not $Package -Or -Not $Version) {
    Write-Host -ForegroundColor Cyan "USAGE:"
    $ScriptName = $MyInvocation.MyCommand.Name
    Write-Host "  To publish the package"
    Write-Host -ForegroundColor Yellow "      ${ScriptName} -package <package name> -version <version>"
    Write-Host "  To get latest published version of the package"
    Write-Host -ForegroundColor Yellow "      ${ScriptName} -package <package name>"

    Write-Host

    # If just package name is specified, be helpful and list the latest stable version of the CodeBits package
    If (-Not ([string]::IsNullOrEmpty($Package))) {
        Write-Host -ForegroundColor Cyan "Latest version of package:"
        nuget list CodeBits.${Package}
    } Else {
        Write-Host -ForegroundColor Cyan "Available packages for publishing"
        Get-ChildItem -Path ./nuget -File -Filter *.json | select -exp Name
    }

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
