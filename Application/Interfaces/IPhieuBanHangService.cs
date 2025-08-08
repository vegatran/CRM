using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPhieuBanHangService
    {
        Task<PhieuBanHang?> GetByIdAsync(int id);
        Task<PhieuBanHang?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<PhieuBanHang>> GetAllAsync();
        Task<IEnumerable<PhieuBanHang>> GetAllWithDetailsAsync();
        Task<IEnumerable<PhieuBanHang>> GetByDateAsync(DateTime date);
        Task<PhieuBanHang> CreateAsync(PhieuBanHang phieuBanHang);
        Task UpdateAsync(PhieuBanHang phieuBanHang);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<ChiTietBanHang> AddChiTietBanHangAsync(ChiTietBanHang chiTietBanHang);
    }
}
