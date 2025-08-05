using Domain.Entities;

namespace Application.Interfaces
{
    public interface INhaCungCapService
    {
        Task<NhaCungCap?> GetByIdAsync(int id);
        Task<IEnumerable<NhaCungCap>> GetAllAsync();
        Task<NhaCungCap> CreateAsync(NhaCungCap nhaCungCap);
        Task UpdateAsync(NhaCungCap nhaCungCap);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<NhaCungCap>> SearchAsync(string keyword);
    }
} 