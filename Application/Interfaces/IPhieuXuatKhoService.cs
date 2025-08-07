using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPhieuXuatKhoService
    {
        Task<IEnumerable<PhieuXuatKho>> GetAllAsync();
        Task<PhieuXuatKho> GetByIdAsync(int id);
        Task<PhieuXuatKho> CreateAsync(PhieuXuatKho phieuXuatKho);
        Task<PhieuXuatKho> UpdateAsync(PhieuXuatKho phieuXuatKho);
        Task DeleteAsync(int id);
    }
}
