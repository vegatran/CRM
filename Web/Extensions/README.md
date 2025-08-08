# Service Registration Extensions

Các extension methods để tự động đăng ký services sử dụng Scrutor.

## Cách sử dụng

### 1. Đăng ký tất cả services từ một assembly
```csharp
// Đăng ký tất cả services từ Application layer
builder.Services.AddServicesFromAssembly<ISanPhamService>();
```

### 2. Đăng ký services từ namespace cụ thể
```csharp
// Đăng ký tất cả services từ namespace Application.Services
builder.Services.AddServicesFromNamespace<ISanPhamService>("Application.Services");
```

### 3. Đăng ký với lifetime khác
```csharp
// Đăng ký với Singleton lifetime
builder.Services.AddServicesFromAssembly<ISanPhamService>(ServiceLifetime.Singleton);

// Đăng ký với Transient lifetime
builder.Services.AddServicesFromNamespace<ISanPhamService>("Application.Services", ServiceLifetime.Transient);
```

## Lợi ích

1. **Tự động hóa**: Không cần đăng ký từng service một cách thủ công
2. **Giảm lỗi**: Tránh quên đăng ký service mới
3. **Dễ bảo trì**: Khi thêm service mới, chỉ cần đảm bảo nó implement interface và nằm trong namespace đúng
4. **Linh hoạt**: Có thể chọn assembly, namespace hoặc lifetime phù hợp

## Yêu cầu

- Package `Scrutor` đã được cài đặt
- Services phải implement interface
- Services phải là class (không abstract)
- Services phải nằm trong assembly được chỉ định

## Ví dụ

```csharp
// Program.cs
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký tất cả services từ Application layer
builder.Services.AddServicesFromNamespace<ISanPhamService>("Application.Services");

// Đăng ký Unit of Work riêng (vì nó không implement interface từ Application.Interfaces)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
``` 