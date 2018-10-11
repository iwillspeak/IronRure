function Test-LastExitCode
{
    param(
        [int]
        $ExitCode
    )

    if ($ExitCode -gt 0)
    {
        throw [System.InvalidOperationException]::new("Operation failed with exit code: {0}" -f $ExitCode)
    }
}

dotnet restore
Test-LastExitCode $lastexitcode
dotnet build --no-restore
Test-LastExitCode $lastexitcode



if (${env:APPVEYOR_REPO_TAG})
{
    dotnet pack --no-restore `
        -o artifacts_nuget `
        --configuration Release
    Test-LastExitCode $lastexitcode
}
else
{
    dotnet pack --no-restore `
        -o artifacts_nuget `
        --version-suffix=${env:APPVEYOR_REPO_BRANCH}${env:APPVEYOR_BUILD_NUMBER} `
        --configuration Release
    Test-LastExitCode $lastexitcode
}

ForEach ($proj in (Get-ChildItem -Path test -Recurse -Filter '*.csproj'))
{
    dotnet test --no-restore --no-build $proj.FullName
    Test-LastExitCode $lastexitcode
}
