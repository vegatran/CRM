using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserId()
        {
            // Có thể mở rộng để lấy từ JWT token hoặc session
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public string? GetCurrentUserName()
        {
            // Có thể mở rộng để lấy từ JWT token hoặc session
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }
    }
} 