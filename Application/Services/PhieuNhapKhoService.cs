using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PhieuNhapKhoService : IPhieuNhapKhoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhieuNhapKhoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PhieuNhapKho?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<PhieuNhapKho>().GetByIdAsync(id);
        }

        public async Task<PhieuNhapKho?> GetByIdWithDetailsAsync(int id)
        {
            var phieuNhapKho = await _unitOfWork.Repository<PhieuNhapKho>().GetByIdAsync(id);
            if (phieuNhapKho == null)
                return null;

            // Load ChiTietNhapKho với SanPham và NguyenLieu
            var chiTietNhapKhos = await _unitOfWork.Repository<ChiTietNhapKho>().GetAllAsync();
            phieuNhapKho.ChiTietNhapKhos = chiTietNhapKhos.Where(ct => ct.PhieuNhapKhoId == id).ToList();

            // Load navigation properties
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allNguyenLieus = await _unitOfWork.NguyenLieuRepository.GetAllAsync();
            var allNhaCungCaps = await _unitOfWork.NhaCungCapRepository.GetAllAsync();

            foreach (var chiTiet in phieuNhapKho.ChiTietNhapKhos)
            {
                if (chiTiet.SanPhamId.HasValue)
                {
                    chiTiet.SanPham = allSanPhams.FirstOrDefault(sp => sp.Id == chiTiet.SanPhamId);
                }
                if (chiTiet.NguyenLieuId.HasValue)
                {
                    chiTiet.NguyenLieu = allNguyenLieus.FirstOrDefault(nl => nl.Id == chiTiet.NguyenLieuId);
                }
            }

            // Load NhaCungCap
            phieuNhapKho.NhaCungCap = allNhaCungCaps.FirstOrDefault(ncc => ncc.Id == phieuNhapKho.NhaCungCapId) ?? new NhaCungCap();

            return phieuNhapKho;
        }

        public async Task<IEnumerable<PhieuNhapKho>> GetAllAsync()
        {
            return await _unitOfWork.Repository<PhieuNhapKho>().GetAllAsync();
        }

        public async Task<IEnumerable<PhieuNhapKho>> GetAllWithDetailsAsync()
        {
            var allPhieuNhapKhos = await GetAllAsync();
            var allChiTietNhapKhos = await _unitOfWork.Repository<ChiTietNhapKho>().GetAllAsync();
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allNguyenLieus = await _unitOfWork.NguyenLieuRepository.GetAllAsync();
            var allNhaCungCaps = await _unitOfWork.NhaCungCapRepository.GetAllAsync();

            foreach (var phieu in allPhieuNhapKhos)
            {
                // Load ChiTietNhapKho
                phieu.ChiTietNhapKhos = allChiTietNhapKhos.Where(ct => ct.PhieuNhapKhoId == phieu.Id).ToList();

                // Load navigation properties
                foreach (var chiTiet in phieu.ChiTietNhapKhos)
                {
                    if (chiTiet.SanPhamId.HasValue)
                    {
                        chiTiet.SanPham = allSanPhams.FirstOrDefault(sp => sp.Id == chiTiet.SanPhamId);
                    }
                    if (chiTiet.NguyenLieuId.HasValue)
                    {
                        chiTiet.NguyenLieu = allNguyenLieus.FirstOrDefault(nl => nl.Id == chiTiet.NguyenLieuId);
                    }
                }

                // Load NhaCungCap
                phieu.NhaCungCap = allNhaCungCaps.FirstOrDefault(ncc => ncc.Id == phieu.NhaCungCapId) ?? new NhaCungCap();
            }

            return allPhieuNhapKhos;
        }

        public async Task<IEnumerable<PhieuNhapKho>> GetByDateAsync(DateTime date)
        {
            var allPhieuNhapKhos = await GetAllAsync();
            return allPhieuNhapKhos.Where(p => p.NgayNhap.Date == date.Date);
        }

        public async Task<PhieuNhapKho> CreateAsync(PhieuNhapKho phieuNhapKho)
        {
            var result = await _unitOfWork.Repository<PhieuNhapKho>().AddAsync(phieuNhapKho);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(PhieuNhapKho phieuNhapKho)
        {
            await _unitOfWork.Repository<PhieuNhapKho>().UpdateAsync(phieuNhapKho);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phieuNhapKho = await GetByIdAsync(id);
            if (phieuNhapKho != null)
            {
                await _unitOfWork.Repository<PhieuNhapKho>().DeleteAsync(phieuNhapKho);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<PhieuNhapKho>().ExistsAsync(id);
        }

        public async Task<ChiTietNhapKho> AddChiTietNhapKhoAsync(ChiTietNhapKho chiTietNhapKho)
        {
            var result = await _unitOfWork.Repository<ChiTietNhapKho>().AddAsync(chiTietNhapKho);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
