# PowerShell script de build va chay du an CRM
Write-Host "=== CRM He thong Quan ly May Mac ===" -ForegroundColor Green
Write-Host "Dang build va chay du an..." -ForegroundColor Yellow

# Kiem tra .NET SDK
Write-Host "Kiem tra .NET SDK..." -ForegroundColor Cyan
dotnet --version

# Restore packages
Write-Host "Restore packages..." -ForegroundColor Cyan
dotnet restore

# Build solution
Write-Host "Build solution..." -ForegroundColor Cyan
dotnet build --no-restore

# Kiem tra xem co thu muc wwwroot/dist khong
if (-not (Test-Path "Web/wwwroot/dist")) {
    Write-Host "AdminLTE chua duoc tai. Dang tai AdminLTE..." -ForegroundColor Yellow
    Set-Location "Web/wwwroot"
    
    # Thu tai tu GitHub truoc
    Write-Host "Thu tai AdminLTE tu GitHub..." -ForegroundColor Cyan
    powershell -ExecutionPolicy Bypass -File download-adminlte.ps1
    
    # Kiem tra xem co tai thanh cong khong
    if (-not (Test-Path "dist/css/adminlte.min.css")) {
        Write-Host "Khong the tai tu GitHub. Dang thu tai tu CDN..." -ForegroundColor Yellow
        powershell -ExecutionPolicy Bypass -File download-adminlte-cdn.ps1
    }
    
    Set-Location "../.."
}

# Tao database migration (neu chua co)
Write-Host "Kiem tra database migrations..." -ForegroundColor Cyan
Set-Location "Web"
if (-not (Test-Path "Migrations")) {
    Write-Host "Tao initial migration..." -ForegroundColor Yellow
    dotnet ef migrations add InitialCreate
}

# Update database
Write-Host "Update database..." -ForegroundColor Cyan
dotnet ef database update
Set-Location ".."

# Chay ung dung
Write-Host "Chay ung dung..." -ForegroundColor Cyan
Write-Host "Truy cap ung dung tai: https://localhost:5001" -ForegroundColor Green
Write-Host "Nhan Ctrl+C de dung ung dung" -ForegroundColor Yellow

Set-Location "Web"
dotnet run 