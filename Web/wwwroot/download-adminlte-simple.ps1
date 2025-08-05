# PowerShell script de tai AdminLTE tu CDN
Write-Host "Dang tai AdminLTE tu CDN..." -ForegroundColor Green

# Tao thu muc can thiet
New-Item -ItemType Directory -Force -Path "plugins"
New-Item -ItemType Directory -Force -Path "dist"
New-Item -ItemType Directory -Force -Path "dist/css"
New-Item -ItemType Directory -Force -Path "dist/js"
New-Item -ItemType Directory -Force -Path "plugins/fontawesome-free/css"
New-Item -ItemType Directory -Force -Path "plugins/jquery"
New-Item -ItemType Directory -Force -Path "plugins/bootstrap/js"
New-Item -ItemType Directory -Force -Path "plugins/datatables-bs4/css"
New-Item -ItemType Directory -Force -Path "plugins/datatables-bs4/js"
New-Item -ItemType Directory -Force -Path "plugins/datatables-responsive/css"
New-Item -ItemType Directory -Force -Path "plugins/datatables-responsive/js"
New-Item -ItemType Directory -Force -Path "plugins/chart.js"

# Tai AdminLTE CSS
Write-Host "Dang tai AdminLTE CSS..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://cdn.jsdelivr.net/npm/admin-lte@3.2/dist/css/adminlte.min.css" -UseBasicParsing
    $response.Content | Out-File -FilePath "dist/css/adminlte.min.css" -Encoding UTF8
    Write-Host "Da tai thanh cong: adminlte.min.css" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai adminlte.min.css: $($_.Exception.Message)" -ForegroundColor Red
}

# Tai AdminLTE JS
Write-Host "Dang tai AdminLTE JS..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://cdn.jsdelivr.net/npm/admin-lte@3.2/dist/js/adminlte.min.js" -UseBasicParsing
    $response.Content | Out-File -FilePath "dist/js/adminlte.min.js" -Encoding UTF8
    Write-Host "Da tai thanh cong: adminlte.min.js" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai adminlte.min.js: $($_.Exception.Message)" -ForegroundColor Red
}

# Tai FontAwesome
Write-Host "Dang tai FontAwesome..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/fontawesome-free/css/all.min.css" -Encoding UTF8
    Write-Host "Da tai thanh cong: FontAwesome" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai FontAwesome: $($_.Exception.Message)" -ForegroundColor Red
}

# Tai jQuery
Write-Host "Dang tai jQuery..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://code.jquery.com/jquery-3.6.0.min.js" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/jquery/jquery.min.js" -Encoding UTF8
    Write-Host "Da tai thanh cong: jQuery" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai jQuery: $($_.Exception.Message)" -ForegroundColor Red
}

# Tai Bootstrap
Write-Host "Dang tai Bootstrap..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/bootstrap/js/bootstrap.bundle.min.js" -Encoding UTF8
    Write-Host "Da tai thanh cong: Bootstrap" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai Bootstrap: $($_.Exception.Message)" -ForegroundColor Red
}

# Tai DataTables
Write-Host "Dang tai DataTables..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://cdn.datatables.net/1.11.5/css/dataTables.bootstrap4.min.css" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/datatables-bs4/css/dataTables.bootstrap4.min.css" -Encoding UTF8
    
    $response = Invoke-WebRequest -Uri "https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/datatables/jquery.dataTables.min.js" -Encoding UTF8
    
    $response = Invoke-WebRequest -Uri "https://cdn.datatables.net/1.11.5/js/dataTables.bootstrap4.min.js" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/datatables-bs4/js/dataTables.bootstrap4.min.js" -Encoding UTF8
    
    Write-Host "Da tai thanh cong: DataTables" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai DataTables: $($_.Exception.Message)" -ForegroundColor Red
}

# Tai Chart.js
Write-Host "Dang tai Chart.js..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://cdn.jsdelivr.net/npm/chart.js@3.7.0/dist/chart.min.js" -UseBasicParsing
    $response.Content | Out-File -FilePath "plugins/chart.js/Chart.min.js" -Encoding UTF8
    Write-Host "Da tai thanh cong: Chart.js" -ForegroundColor Green
}
catch {
    Write-Host "Loi khi tai Chart.js: $($_.Exception.Message)" -ForegroundColor Red
}

# Tao file CSS tuy chinh
$customCSS = "/* Custom CSS de fix mot so van de voi AdminLTE tu CDN */`n"
$customCSS += ".sidebar-mini .main-sidebar { width: 250px; }`n"
$customCSS += ".sidebar-mini.sidebar-collapse .main-sidebar { width: 4.6rem; }`n"
$customCSS += ".content-wrapper { margin-left: 250px; }`n"
$customCSS += ".sidebar-collapse .content-wrapper { margin-left: 4.6rem; }`n"
$customCSS += "@media (max-width: 767.98px) { .content-wrapper { margin-left: 0; } }`n"

$customCSS | Out-File -FilePath "dist/css/custom.css" -Encoding UTF8

Write-Host "Hoan thanh! AdminLTE da duoc tai tu CDN." -ForegroundColor Green
Write-Host "Cac file da duoc dat trong thu muc wwwroot/dist va wwwroot/plugins" -ForegroundColor Green 