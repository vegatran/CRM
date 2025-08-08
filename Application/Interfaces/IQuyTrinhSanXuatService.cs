using Domain.Entities;

namespace Application.Interfaces
{
    public interface IQuyTrinhSanXuatService
    {
        Task<QuyTrinhSanXuat?> GetByIdAsync(int id);
        Task<IEnumerable<QuyTrinhSanXuat>> GetAllAsync();
        Task<IEnumerable<QuyTrinhSanXuat>> GetBySanPhamIdAsync(int sanPhamId);
        Task<QuyTrinhSanXuat?> GetBySanPhamAndTenQuyTrinhAsync(int sanPhamId, string tenQuyTrinh);
        Task<QuyTrinhSanXuat> CreateAsync(QuyTrinhSanXuat quyTrinhSanXuat);
        Task UpdateAsync(QuyTrinhSanXuat quyTrinhSanXuat);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTongChiPhiNhanCongAsync(int sanPhamId);
    }
} 