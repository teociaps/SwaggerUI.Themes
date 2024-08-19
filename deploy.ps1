# deploy.ps1
param (
    [string]$Configuration = "Release",
    [string]$PackageOutput = "$(pwd)/artifacts",
    [string]$NuGetSourceUrl = "https://api.nuget.org/v3/index.json",
    [string]$NuGetApiKey
)

# Ensure the output directory exists
if (-not (Test-Path -Path $PackageOutput)) {
    New-Item -ItemType Directory -Path $PackageOutput | Out-Null
}

# Iterate over each project to pack
Get-ChildItem -Path src -Filter *.Themes -Directory | ForEach-Object {
    $projectPath = $_.FullName
    $projectName = $_.Name
    $csprojPath = Join-Path -Path $projectPath -ChildPath "${projectName}.csproj"

    try {
        $version = dotnet msbuild $csprojPath -nologo -t:GetVersion -p:Configuration=$configuration | ForEach-Object { $_.Trim() }

        if (-not $version) {
            throw "Failed to retrieve version for project ${projectName}"
        }

        Write-Host "> Packing ${projectName} version: ${version}" -ForegroundColor "Cyan"

        dotnet pack $projectPath --configuration $Configuration --output $PackageOutput

        $packageName = "${projectName}.${version}.nupkg"
        $packagePath = Join-Path -Path $PackageOutput -ChildPath $packageName

        if (Test-Path -Path $packagePath) {
            dotnet nuget push $packagePath -k $NuGetApiKey -s $NuGetSourceUrl --skip-duplicate
            Write-Host "Package ${packageName} pushed." -ForegroundColor "Green"
        } else {
            Write-Host "Package ${packageName} already exists on NuGet. Skipping push." -ForegroundColor "Yellow"
        }
    } catch {
        Write-Error "Error processing project ${projectName}: $_"
    }
}

Write-Host "Deploy finished."