
$ErrorActionPreference = "Stop"

$solutionPath = "PlayerWalletSimulator.sln"
$testProjectPath = "tests/PlayerWalletSimulator.Tests/PlayerWalletSimulator.Tests.csproj"
$appWorkingDirectory = "src/PlayerWalletSimulator.Console"

 
Write-Host "Cleaning the solution..." -ForegroundColor Green
dotnet clean $solutionPath

Write-Host "Building the project..." -ForegroundColor Green
dotnet build $solutionPath --configuration Release

Write-Host "Running tests..." -ForegroundColor Green
dotnet test $testProjectPath --no-build --configuration Release

Write-Host "All tests passed! Starting application..." -ForegroundColor Green

Start-Process "dotnet" -ArgumentList "run --configuration Release" -WorkingDirectory $appWorkingDirectory
