# CRM Hệ thống Quản lý May Mặc

Hệ thống CRM quản lý may mặc được xây dựng theo kiến trúc Clean Architecture với ASP.NET Core 8.0 và Entity Framework Core.

## Cấu trúc Dự án

```
CRM/
├── Domain/                 # Domain Layer - Entities, Interfaces
├── Application/           # Application Layer - Services, DTOs
├── Infrastructure/        # Infrastructure Layer - Data Access, External Services
├── Web/                  # Presentation Layer - Controllers, Views
└── Utilities/            # Shared Utilities
```

## Công nghệ sử dụng

- **.NET 8.0**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **AdminLTE 3.2.0** (UI Framework)
- **DataTables** (jQuery Plugin)
- **Chart.js** (Biểu đồ)

## Các chức năng chính

1. **Quản lý Sản phẩm** - CRUD operations cho sản phẩm may mặc
2. **Quản lý Nhà cung cấp** - Quản lý thông tin nhà cung cấp nguyên liệu
3. **Quản lý Nguyên liệu** - Quản lý nguyên liệu sản xuất
4. **Quản lý Nhập kho** - Quản lý phiếu nhập kho
5. **Quản lý Thành phần cấu hình** - Cấu hình nguyên liệu cho sản phẩm
6. **Quản lý Bán hàng** - Quản lý phiếu bán hàng
7. **Quản lý Khách hàng** - Quản lý khách hàng theo khu vực
8. **Báo cáo & Thống kê** - Báo cáo nhập xuất tồn, bảng cân đối kế toán

## Cài đặt và Chạy

### Yêu cầu hệ thống

- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server (LocalDB hoặc SQL Server Express)
- PowerShell (để chạy script tải AdminLTE)

### Bước 1: Clone repository

```bash
git clone <repository-url>
cd CRM
```

### Bước 2: Cài đặt dependencies

```bash
dotnet restore
```

### Bước 3: Tạo database

```bash
cd Web
dotnet ef database update
```

### Bước 4: Tải AdminLTE

```bash
cd wwwroot
powershell -ExecutionPolicy Bypass -File download-adminlte.ps1
```

### Bước 5: Chạy ứng dụng

```bash
cd ..
dotnet run
```

Truy cập ứng dụng tại: `https://localhost:5001` hoặc `http://localhost:5000`

## Cấu hình Database

Cập nhật connection string trong `Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CRMMayMac;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Kiến trúc Clean Architecture

### Domain Layer
- **Entities**: Các entity chính của hệ thống
- **Interfaces**: Repository và Unit of Work interfaces

### Application Layer
- **Services**: Business logic services
- **DTOs**: Data Transfer Objects
- **Interfaces**: Service interfaces

### Infrastructure Layer
- **Data**: Entity Framework DbContext
- **Repositories**: Repository implementations
- **External Services**: Email, logging services

### Web Layer
- **Controllers**: MVC Controllers
- **Views**: Razor Views với AdminLTE
- **Models**: View Models

## Migration Commands

```bash
# Tạo migration mới
dotnet ef migrations add MigrationName

# Cập nhật database
dotnet ef database update

# Xóa migration cuối
dotnet ef migrations remove
```

## Tính năng bổ sung

### Authentication & Authorization
- Tích hợp Identity Server 4 (IDS4)
- JWT Bearer Authentication
- Role-based Authorization

### Reporting
- Báo cáo nhập xuất tồn
- Bảng cân đối kế toán
- Biểu đồ thống kê

### API Endpoints
- RESTful API cho mobile app
- Swagger documentation

## Đóng góp

1. Fork dự án
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## License

Dự án này được phân phối dưới MIT License. Xem file `LICENSE` để biết thêm chi tiết.

## Hỗ trợ

Nếu có vấn đề hoặc câu hỏi, vui lòng tạo issue trong repository hoặc liên hệ team phát triển. 