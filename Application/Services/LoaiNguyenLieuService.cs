using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class LoaiNguyenLieuService : ILoaiNguyenLieuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoaiNguyenLieuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LoaiNguyenLieu?> GetByIdAsync(int id)
        {
            return await _unitOfWork.LoaiNguyenLieuRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LoaiNguyenLieu>> GetAllAsync()
        {
            return await _unitOfWork.LoaiNguyenLieuRepository.GetAllAsync();
        }

        public async Task<LoaiNguyenLieu> CreateAsync(LoaiNguyenLieu loaiNguyenLieu)
        {
            await _unitOfWork.LoaiNguyenLieuRepository.AddAsync(loaiNguyenLieu);
            await _unitOfWork.SaveChangesAsync();
            return loaiNguyenLieu;
        }

        public async Task UpdateAsync(LoaiNguyenLieu loaiNguyenLieu)
        {
            await _unitOfWork.LoaiNguyenLieuRepository.UpdateAsync(loaiNguyenLieu);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loaiNguyenLieu = await GetByIdAsync(id);
            if (loaiNguyenLieu != null)
            {
                await _unitOfWork.LoaiNguyenLieuRepository.DeleteAsync(loaiNguyenLieu);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.LoaiNguyenLieuRepository.GetByIdAsync(id) != null;
        }
    }
} 