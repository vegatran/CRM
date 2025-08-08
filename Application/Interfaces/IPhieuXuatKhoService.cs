using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPhieuXuatKhoService
    {
        Task<PhieuXuatKho?> GetByIdAsync(int id);
        Task<IEnumerable<PhieuXuatKho>> GetAllAsync();
        Task<PhieuXuatKho> CreateAsync(PhieuXuatKho phieuXuatKho);
        Task UpdateAsync(PhieuXuatKho phieuXuatKho);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
