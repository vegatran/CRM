# Download DataTables Responsive JS files
Write-Host "Downloading DataTables Responsive JS files..." -ForegroundColor Green

# Download dataTables.responsive.min.js
Write-Host "Downloading dataTables.responsive.min.js..." -ForegroundColor Yellow
try {
    Invoke-WebRequest -Uri "https://cdn.datatables.net/responsive/2.5.0/js/dataTables.responsive.min.js" -OutFile "plugins\datatables-responsive\js\dataTables.responsive.min.js"
    Write-Host "✓ Downloaded dataTables.responsive.min.js" -ForegroundColor Green
}
catch {
    Write-Host "✗ Failed to download dataTables.responsive.min.js" -ForegroundColor Red
}

# Download responsive.bootstrap4.min.js
Write-Host "Downloading responsive.bootstrap4.min.js..." -ForegroundColor Yellow
try {
    Invoke-WebRequest -Uri "https://cdn.datatables.net/responsive/2.5.0/js/responsive.bootstrap4.min.js" -OutFile "plugins\datatables-responsive\js\responsive.bootstrap4.min.js"
    Write-Host "✓ Downloaded responsive.bootstrap4.min.js" -ForegroundColor Green
}
catch {
    Write-Host "✗ Failed to download responsive.bootstrap4.min.js" -ForegroundColor Red
}

Write-Host "Download completed!" -ForegroundColor Green 