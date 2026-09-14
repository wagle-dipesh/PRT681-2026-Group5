<#
.SYNOPSIS
    Provisions Azure App Service (Linux, Free F1 tier) + Azure SQL Database (Free tier)
    and deploys TaskManagerMVC to it directly from source (no container registry needed).

.DESCRIPTION
    The Free (F1) App Service tier does not support custom containers, so this script
    deploys the published app code via `az webapp up`, which Azure builds and runs using
    its built-in .NET runtime. The Dockerfile in this repo is still used for local dev
    and for container-based hosts such as Render (see README.md).

.NOTES
    Requires the Azure CLI (az) and an active subscription: `az login` first.
    Run from anywhere; the script cd's into the project folder itself.
#>

param(
    [string]$ResourceGroup = "taskmanagermvc-rg",
    [string]$Location = "australiaeast",
    [string]$AppServicePlan = "taskmanagermvc-plan",
    [string]$WebAppName = "taskmanagermvc-$(Get-Random -Minimum 1000 -Maximum 9999)",
    [string]$SqlServerName = "taskmanagermvc-sql-$(Get-Random -Minimum 1000 -Maximum 9999)",
    [string]$SqlAdminUser = "tmvcadmin",
    [string]$DatabaseName = "TaskManagerMvcDb"
)

$ErrorActionPreference = "Stop"

Write-Host "==> Checking Azure CLI login..."
az account show | Out-Null

$SqlAdminPassword = Read-Host "Enter a new SQL admin password (12+ chars, upper/lower/digit/symbol)" -AsSecureString
$SqlAdminPasswordPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SqlAdminPassword)
)

Write-Host "==> Creating resource group '$ResourceGroup' in $Location..."
az group create --name $ResourceGroup --location $Location | Out-Null

Write-Host "==> Creating Azure SQL logical server '$SqlServerName'..."
az sql server create `
    --name $SqlServerName `
    --resource-group $ResourceGroup `
    --location $Location `
    --admin-user $SqlAdminUser `
    --admin-password $SqlAdminPasswordPlain | Out-Null

Write-Host "==> Allowing Azure services to reach the SQL server..."
az sql server firewall-rule create `
    --resource-group $ResourceGroup `
    --server $SqlServerName `
    --name AllowAzureServices `
    --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0 | Out-Null

Write-Host "==> Creating free-tier Azure SQL Database '$DatabaseName'..."
az sql db create `
    --resource-group $ResourceGroup `
    --server $SqlServerName `
    --name $DatabaseName `
    --use-free-limit `
    --free-limit-exhaustion-behavior AutoPause | Out-Null

Write-Host "==> Applying Database/Setup.sql..."
$sqlcmdPath = (Get-Command sqlcmd -ErrorAction SilentlyContinue)
if ($sqlcmdPath) {
    sqlcmd -S "$SqlServerName.database.windows.net" -d $DatabaseName -U $SqlAdminUser -P $SqlAdminPasswordPlain `
        -i "$PSScriptRoot\..\TaskManagerMVC\Database\Setup.sql"
} else {
    Write-Warning "sqlcmd not found locally. Run Database/Setup.sql against the new database manually (Azure Data Studio, SSMS, or the Azure Portal query editor) before using the app."
}

$connectionString = "Server=tcp:$SqlServerName.database.windows.net,1433;Initial Catalog=$DatabaseName;User ID=$SqlAdminUser;Password=$SqlAdminPasswordPlain;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

Write-Host "==> Creating Free (F1) App Service plan '$AppServicePlan'..."
az appservice plan create `
    --name $AppServicePlan `
    --resource-group $ResourceGroup `
    --location $Location `
    --sku F1 `
    --is-linux | Out-Null

Write-Host "==> Creating Web App '$WebAppName'..."
az webapp create `
    --name $WebAppName `
    --resource-group $ResourceGroup `
    --plan $AppServicePlan `
    --runtime "DOTNETCORE:10.0" | Out-Null

Write-Host "==> Wiring up the connection string and health check..."
az webapp config appsettings set `
    --name $WebAppName `
    --resource-group $ResourceGroup `
    --settings "ConnectionStrings__DefaultConnection=$connectionString" "ASPNETCORE_ENVIRONMENT=Production" | Out-Null

az webapp config set `
    --name $WebAppName `
    --resource-group $ResourceGroup `
    --health-check-path "/health" | Out-Null

Write-Host "==> Deploying source from ../TaskManagerMVC (this builds via Oryx, no Docker needed for App Service Free tier)..."
Push-Location "$PSScriptRoot\..\TaskManagerMVC"
az webapp up `
    --name $WebAppName `
    --resource-group $ResourceGroup `
    --plan $AppServicePlan `
    --sku F1 `
    --runtime "DOTNETCORE:10.0"
Pop-Location

Write-Host ""
Write-Host "==> Done. App URL: https://$WebAppName.azurewebsites.net"
Write-Host "==> SQL server: $SqlServerName.database.windows.net (admin user: $SqlAdminUser)"
Write-Host "==> Keep the SQL admin password somewhere safe - it was not saved anywhere by this script."
