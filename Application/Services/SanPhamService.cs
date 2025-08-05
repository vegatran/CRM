using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;

namespace Application.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SanPhamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SanPham?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<SanPham>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _unitOfWork.Repository<SanPham>().GetAllAsync();
        }

        public async Task<SanPham> CreateAsync(SanPham sanPham)
        {
            var result = await _unitOfWork.Repository<SanPham>().AddAsync(sanPham);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(SanPham sanPham)
        {
            await _unitOfWork.Repository<SanPham>().UpdateAsync(sanPham);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sanPham = await GetByIdAsync(id);
            if (sanPham != null)
            {
                await _unitOfWork.Repository<SanPham>().DeleteAsync(sanPham);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<SanPham>().ExistsAsync(id);
        }

        public async Task<IEnumerable<SanPham>> SearchAsync(string keyword)
        {
            var allSanPhams = await GetAllAsync();
            return allSanPhams.Where(sp => 
                sp.TenSanPham.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                sp.MaSanPham.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (sp.MoTa != null && sp.MoTa.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            );
        }
    }
} 