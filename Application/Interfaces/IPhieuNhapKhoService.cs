using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPhieuNhapKhoService
    {
        Task<PhieuNhapKho?> GetByIdAsync(int id);
        Task<PhieuNhapKho?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<PhieuNhapKho>> GetAllAsync();
        Task<IEnumerable<PhieuNhapKho>> GetAllWithDetailsAsync();
        Task<IEnumerable<PhieuNhapKho>> GetByDateAsync(DateTime date);
        Task<PhieuNhapKho> CreateAsync(PhieuNhapKho phieuNhapKho);
        Task UpdateAsync(PhieuNhapKho phieuNhapKho);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<ChiTietNhapKho> AddChiTietNhapKhoAsync(ChiTietNhapKho chiTietNhapKho);
    }
}
