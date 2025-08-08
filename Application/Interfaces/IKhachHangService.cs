using Domain.Entities;

namespace Application.Interfaces
{
    public interface IKhachHangService
    {
        Task<KhachHang?> GetByIdAsync(int id);
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang> CreateAsync(KhachHang khachHang);
        Task UpdateAsync(KhachHang khachHang);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
