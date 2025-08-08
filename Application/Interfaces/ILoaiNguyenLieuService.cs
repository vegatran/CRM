using Domain.Entities;

namespace Application.Interfaces
{
    public interface ILoaiNguyenLieuService
    {
        Task<LoaiNguyenLieu?> GetByIdAsync(int id);
        Task<IEnumerable<LoaiNguyenLieu>> GetAllAsync();
        Task<LoaiNguyenLieu> CreateAsync(LoaiNguyenLieu loaiNguyenLieu);
        Task UpdateAsync(LoaiNguyenLieu loaiNguyenLieu);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 