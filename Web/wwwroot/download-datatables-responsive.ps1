# Download DataTables Responsive files
Write-Host "Downloading DataTables Responsive files..." -ForegroundColor Green

# Create directories if they don't exist
$cssDir = "plugins\datatables-responsive\css"
$jsDir = "plugins\datatables-responsive\js"

if (!(Test-Path $cssDir)) {
    New-Item -ItemType Directory -Path $cssDir -Force
}

if (!(Test-Path $jsDir)) {
    New-Item -ItemType Directory -Path $jsDir -Force
}

# Download CSS files
Write-Host "Downloading CSS files..." -ForegroundColor Yellow

$cssFiles = @{
    "responsive.bootstrap4.min.css" = "https://cdn.datatables.net/responsive/2.5.0/css/responsive.bootstrap4.min.css"
}

foreach ($file in $cssFiles.GetEnumerator()) {
    $outputPath = Join-Path $cssDir $file.Key
    Write-Host "Downloading $($file.Key)..." -ForegroundColor Cyan
    try {
        Invoke-WebRequest -Uri $file.Value -OutFile $outputPath
        Write-Host "✓ Downloaded $($file.Key)" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Failed to download $($file.Key): $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Download JS files
Write-Host "Downloading JS files..." -ForegroundColor Yellow

$jsFiles = @{
    "dataTables.responsive.min.js" = "https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js"
    "responsive.bootstrap4.min.js" = "https://cdn.datatables.net/responsive/2.5.0/js/responsive.bootstrap4.min.js"
}

foreach ($file in $jsFiles.GetEnumerator()) {
    $outputPath = Join-Path $jsDir $file.Key
    Write-Host "Downloading $($file.Key)..." -ForegroundColor Cyan
    try {
        Invoke-WebRequest -Uri $file.Value -OutFile $outputPath
        Write-Host "✓ Downloaded $($file.Key)" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Failed to download $($file.Key): $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "DataTables Responsive download completed!" -ForegroundColor Green 