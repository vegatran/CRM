using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDinhMucNguyenLieuService
    {
        Task<DinhMucNguyenLieu?> GetByIdAsync(int id);
        Task<IEnumerable<DinhMucNguyenLieu>> GetAllAsync();
        Task<IEnumerable<DinhMucNguyenLieu>> GetAllWithDetailsAsync();
        Task<IEnumerable<DinhMucNguyenLieu>> GetBySanPhamIdAsync(int sanPhamId);
        Task<DinhMucNguyenLieu> CreateAsync(DinhMucNguyenLieu dinhMucNguyenLieu);
        Task UpdateAsync(DinhMucNguyenLieu dinhMucNguyenLieu);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTongChiPhiNguyenLieuAsync(int sanPhamId);
    }
} 