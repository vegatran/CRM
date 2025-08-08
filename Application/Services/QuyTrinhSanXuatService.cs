using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class QuyTrinhSanXuatService : IQuyTrinhSanXuatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuyTrinhSanXuatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<QuyTrinhSanXuat?> GetByIdAsync(int id)
        {
            return await _unitOfWork.QuyTrinhSanXuatRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<QuyTrinhSanXuat>> GetAllAsync()
        {
            return await _unitOfWork.QuyTrinhSanXuatRepository.GetAllAsync();
        }

        public async Task<IEnumerable<QuyTrinhSanXuat>> GetBySanPhamIdAsync(int sanPhamId)
        {
            var allQuyTrinhs = await _unitOfWork.QuyTrinhSanXuatRepository.GetAllAsync();
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();

            var result = allQuyTrinhs.Where(q => q.SanPhamId == sanPhamId && q.TrangThai)
                              .OrderBy(q => q.ThuTu)
                              .ToList();

            // Manually populate navigation properties
            foreach (var quyTrinh in result)
            {
                var sanPham = allSanPhams.FirstOrDefault(sp => sp.Id == quyTrinh.SanPhamId);
                if (sanPham != null)
                    quyTrinh.SanPham = sanPham;
            }

            return result;
        }

        public async Task<QuyTrinhSanXuat?> GetBySanPhamAndTenQuyTrinhAsync(int sanPhamId, string tenCongDoan)
        {
            var allQuyTrinhs = await _unitOfWork.QuyTrinhSanXuatRepository.GetAllAsync();
            return allQuyTrinhs.FirstOrDefault(q => q.SanPhamId == sanPhamId && 
                                                   q.TenCongDoan.Equals(tenCongDoan, StringComparison.OrdinalIgnoreCase) && 
                                                   q.TrangThai);
        }

        public async Task<QuyTrinhSanXuat> CreateAsync(QuyTrinhSanXuat quyTrinhSanXuat)
        {
            await _unitOfWork.QuyTrinhSanXuatRepository.AddAsync(quyTrinhSanXuat);
            await _unitOfWork.SaveChangesAsync();
            return quyTrinhSanXuat;
        }

        public async Task UpdateAsync(QuyTrinhSanXuat quyTrinhSanXuat)
        {
            await _unitOfWork.QuyTrinhSanXuatRepository.UpdateAsync(quyTrinhSanXuat);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var quyTrinhSanXuat = await GetByIdAsync(id);
            if (quyTrinhSanXuat != null)
            {
                await _unitOfWork.QuyTrinhSanXuatRepository.DeleteAsync(quyTrinhSanXuat);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.QuyTrinhSanXuatRepository.GetByIdAsync(id) != null;
        }

        public async Task<decimal> GetTongChiPhiNhanCongAsync(int sanPhamId)
        {
            var quyTrinhs = await GetBySanPhamIdAsync(sanPhamId);
            return quyTrinhs.Sum(q => q.ChiPhiNhanCong);
        }
    }
} 