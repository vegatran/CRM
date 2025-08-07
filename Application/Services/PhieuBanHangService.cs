using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PhieuBanHangService : IPhieuBanHangService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhieuBanHangService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PhieuBanHang?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<PhieuBanHang>().GetByIdAsync(id);
        }

        public async Task<PhieuBanHang?> GetByIdWithDetailsAsync(int id)
        {
            var phieuBanHang = await _unitOfWork.Repository<PhieuBanHang>().GetByIdAsync(id);
            if (phieuBanHang == null)
                return null;

            // Load ChiTietBanHang với SanPham
            var chiTietBanHangs = await _unitOfWork.Repository<ChiTietBanHang>().GetAllAsync();
            phieuBanHang.ChiTietBanHangs = chiTietBanHangs.Where(ct => ct.PhieuBanHangId == id).ToList();

            // Load navigation properties
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allKhachHangs = await _unitOfWork.KhachHangRepository.GetAllAsync();

            foreach (var chiTiet in phieuBanHang.ChiTietBanHangs)
            {
                chiTiet.SanPham = allSanPhams.FirstOrDefault(sp => sp.Id == chiTiet.SanPhamId) ?? new SanPham();
            }

            // Load KhachHang
            phieuBanHang.KhachHang = allKhachHangs.FirstOrDefault(kh => kh.Id == phieuBanHang.KhachHangId) ?? new KhachHang();

            return phieuBanHang;
        }

        public async Task<IEnumerable<PhieuBanHang>> GetAllAsync()
        {
            return await _unitOfWork.Repository<PhieuBanHang>().GetAllAsync();
        }

        public async Task<IEnumerable<PhieuBanHang>> GetAllWithDetailsAsync()
        {
            var allPhieuBanHangs = await GetAllAsync();
            var allChiTietBanHangs = await _unitOfWork.Repository<ChiTietBanHang>().GetAllAsync();
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allKhachHangs = await _unitOfWork.KhachHangRepository.GetAllAsync();

            foreach (var phieu in allPhieuBanHangs)
            {
                // Load ChiTietBanHang
                phieu.ChiTietBanHangs = allChiTietBanHangs.Where(ct => ct.PhieuBanHangId == phieu.Id).ToList();

                // Load navigation properties
                foreach (var chiTiet in phieu.ChiTietBanHangs)
                {
                    chiTiet.SanPham = allSanPhams.FirstOrDefault(sp => sp.Id == chiTiet.SanPhamId) ?? new SanPham();
                }

                // Load KhachHang
                phieu.KhachHang = allKhachHangs.FirstOrDefault(kh => kh.Id == phieu.KhachHangId) ?? new KhachHang();
            }

            return allPhieuBanHangs;
        }

        public async Task<IEnumerable<PhieuBanHang>> GetByDateAsync(DateTime date)
        {
            var allPhieuBanHangs = await GetAllAsync();
            return allPhieuBanHangs.Where(p => p.NgayBan.Date == date.Date);
        }

        public async Task<PhieuBanHang> CreateAsync(PhieuBanHang phieuBanHang)
        {
            var result = await _unitOfWork.Repository<PhieuBanHang>().AddAsync(phieuBanHang);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(PhieuBanHang phieuBanHang)
        {
            await _unitOfWork.Repository<PhieuBanHang>().UpdateAsync(phieuBanHang);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phieuBanHang = await GetByIdAsync(id);
            if (phieuBanHang != null)
            {
                await _unitOfWork.Repository<PhieuBanHang>().DeleteAsync(phieuBanHang);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<PhieuBanHang>().ExistsAsync(id);
        }

        public async Task<ChiTietBanHang> AddChiTietBanHangAsync(ChiTietBanHang chiTietBanHang)
        {
            var result = await _unitOfWork.Repository<ChiTietBanHang>().AddAsync(chiTietBanHang);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
