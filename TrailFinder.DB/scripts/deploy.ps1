param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('development', 'staging', 'production')]
    [string]$Environment,

    [Parameter(Mandatory=$false)]
    [string]$SupabaseKey
)

$ErrorActionPreference = 'Stop'

# Load environment variables
if (Test-Path ".env.$Environment") {
    Get-Content ".env.$Environment" | ForEach-Object {
        if ($_ -match '^([^=]+)=(.*)$') {
            [Environment]::SetEnvironmentVariable($matches[1], $matches[2])
        }
    }
}

Write-Host "Deploying to $Environment environment..."

# Apply migrations
Get-ChildItem -Path "supabase/migrations" -Filter "*.sql" | Sort-Object Name | ForEach-Object {
    Write-Host "Applying migration: $($_.Name)"
    supabase db reset --db-url $env:DATABASE_URL
}

Write-Host "Deployment completed successfully!"