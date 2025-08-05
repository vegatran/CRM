using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISanPhamService
    {
        Task<SanPham?> GetByIdAsync(int id);
        Task<SanPham?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<IEnumerable<SanPham>> GetAllWithDetailsAsync();
        Task<SanPham> CreateAsync(SanPham sanPham);
        Task UpdateAsync(SanPham sanPham);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SanPham>> SearchAsync(string keyword);
    }
} 