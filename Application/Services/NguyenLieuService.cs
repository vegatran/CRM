using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class NguyenLieuService : INguyenLieuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NguyenLieuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NguyenLieu?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<NguyenLieu>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<NguyenLieu>> GetAllAsync()
        {
            return await _unitOfWork.Repository<NguyenLieu>().GetAllAsync();
        }

        public async Task<IEnumerable<NguyenLieu>> GetAllWithDetailsAsync()
        {
            var allNguyenLieus = await GetAllAsync();
            var loaiNguyenLieus = await _unitOfWork.LoaiNguyenLieuRepository.GetAllAsync();
            var nhaCungCaps = await _unitOfWork.NhaCungCapRepository.GetAllAsync();

            // Manually populate navigation properties
            foreach (var nguyenLieu in allNguyenLieus)
            {
                var loaiNguyenLieu = loaiNguyenLieus.FirstOrDefault(l => l.Id == nguyenLieu.LoaiNguyenLieuId);
                var nhaCungCap = nhaCungCaps.FirstOrDefault(n => n.Id == nguyenLieu.NhaCungCapId);
                
                if (loaiNguyenLieu != null)
                    nguyenLieu.LoaiNguyenLieu = loaiNguyenLieu;
                if (nhaCungCap != null)
                    nguyenLieu.NhaCungCap = nhaCungCap;
            }

            return allNguyenLieus;
        }

        public async Task<NguyenLieu> CreateAsync(NguyenLieu nguyenLieu)
        {
            var result = await _unitOfWork.Repository<NguyenLieu>().AddAsync(nguyenLieu);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(NguyenLieu nguyenLieu)
        {
            await _unitOfWork.Repository<NguyenLieu>().UpdateAsync(nguyenLieu);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nguyenLieu = await GetByIdAsync(id);
            if (nguyenLieu != null)
            {
                await _unitOfWork.Repository<NguyenLieu>().DeleteAsync(nguyenLieu);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<NguyenLieu>().ExistsAsync(id);
        }

        public async Task<IEnumerable<NguyenLieu>> SearchAsync(string keyword)
        {
            var allNguyenLieus = await GetAllAsync();
            return allNguyenLieus.Where(nl => 
                nl.TenNguyenLieu.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                nl.MaNguyenLieu.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (nl.MoTa != null && nl.MoTa.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (nl.ChatLieu != null && nl.ChatLieu.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            );
        }

        public async Task<IEnumerable<NguyenLieu>> GetByLoaiNguyenLieuAsync(int loaiNguyenLieuId)
        {
            var allNguyenLieus = await GetAllAsync();
            return allNguyenLieus.Where(nl => nl.LoaiNguyenLieuId == loaiNguyenLieuId);
        }
    }
} 