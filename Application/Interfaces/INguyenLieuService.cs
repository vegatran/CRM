using Domain.Entities;

namespace Application.Interfaces
{
    public interface INguyenLieuService
    {
        Task<NguyenLieu?> GetByIdAsync(int id);
        Task<IEnumerable<NguyenLieu>> GetAllAsync();
        Task<IEnumerable<NguyenLieu>> GetAllWithDetailsAsync();
        Task<NguyenLieu> CreateAsync(NguyenLieu nguyenLieu);
        Task UpdateAsync(NguyenLieu nguyenLieu);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<NguyenLieu>> SearchAsync(string keyword);
        Task<IEnumerable<NguyenLieu>> GetByLoaiNguyenLieuAsync(int loaiNguyenLieuId);
    }
} 