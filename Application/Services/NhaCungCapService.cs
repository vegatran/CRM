using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NhaCungCapService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NhaCungCap?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<NhaCungCap>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<NhaCungCap>> GetAllAsync()
        {
            return await _unitOfWork.Repository<NhaCungCap>().GetAllAsync();
        }

        public async Task<NhaCungCap> CreateAsync(NhaCungCap nhaCungCap)
        {
            var result = await _unitOfWork.Repository<NhaCungCap>().AddAsync(nhaCungCap);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(NhaCungCap nhaCungCap)
        {
            await _unitOfWork.Repository<NhaCungCap>().UpdateAsync(nhaCungCap);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nhaCungCap = await GetByIdAsync(id);
            if (nhaCungCap != null)
            {
                await _unitOfWork.Repository<NhaCungCap>().DeleteAsync(nhaCungCap);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<NhaCungCap>().ExistsAsync(id);
        }

        public async Task<IEnumerable<NhaCungCap>> SearchAsync(string keyword)
        {
            var allNhaCungCaps = await GetAllAsync();
            return allNhaCungCaps.Where(ncc => 
                ncc.TenNhaCungCap.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (ncc.DiaChi != null && ncc.DiaChi.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (ncc.SoDienThoai != null && ncc.SoDienThoai.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (ncc.Email != null && ncc.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            );
        }
    }
} 