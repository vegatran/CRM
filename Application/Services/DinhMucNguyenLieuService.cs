using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DinhMucNguyenLieuService : IDinhMucNguyenLieuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DinhMucNguyenLieuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DinhMucNguyenLieu?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<DinhMucNguyenLieu>().GetByIdAsync(id);
        }

        public async Task<DinhMucNguyenLieu?> GetByIdWithDetailsAsync(int id)
        {
            var dinhMuc = await _unitOfWork.Repository<DinhMucNguyenLieu>().GetByIdAsync(id);
            if (dinhMuc == null)
                return null;

            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allNguyenLieus = await _unitOfWork.NguyenLieuRepository.GetAllAsync();

            // Manually populate navigation properties
            var sanPham = allSanPhams.FirstOrDefault(sp => sp.Id == dinhMuc.SanPhamId);
            if (sanPham != null)
                dinhMuc.SanPham = sanPham;

            var nguyenLieu = allNguyenLieus.FirstOrDefault(nl => nl.Id == dinhMuc.NguyenLieuId);
            if (nguyenLieu != null)
                dinhMuc.NguyenLieu = nguyenLieu;

            return dinhMuc;
        }

        public async Task<IEnumerable<DinhMucNguyenLieu>> GetAllAsync()
        {
            return await _unitOfWork.Repository<DinhMucNguyenLieu>().GetAllAsync();
        }

        public async Task<IEnumerable<DinhMucNguyenLieu>> GetAllWithDetailsAsync()
        {
            var allDinhMucs = await GetAllAsync();
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allNguyenLieus = await _unitOfWork.NguyenLieuRepository.GetAllAsync();

            // Manually populate navigation properties
            foreach (var dinhMuc in allDinhMucs)
            {
                var sanPham = allSanPhams.FirstOrDefault(sp => sp.Id == dinhMuc.SanPhamId);
                if (sanPham != null)
                    dinhMuc.SanPham = sanPham;

                var nguyenLieu = allNguyenLieus.FirstOrDefault(nl => nl.Id == dinhMuc.NguyenLieuId);
                if (nguyenLieu != null)
                    dinhMuc.NguyenLieu = nguyenLieu;
            }

            return allDinhMucs;
        }

        public async Task<IEnumerable<DinhMucNguyenLieu>> GetBySanPhamIdAsync(int sanPhamId)
        {
            var allDinhMucs = await GetAllAsync();
            var allSanPhams = await _unitOfWork.SanPhamRepository.GetAllAsync();
            var allNguyenLieus = await _unitOfWork.NguyenLieuRepository.GetAllAsync();

            var result = allDinhMucs.Where(d => d.SanPhamId == sanPhamId && d.TrangThai).ToList();

            // Manually populate navigation properties
            foreach (var dinhMuc in result)
            {
                var sanPham = allSanPhams.FirstOrDefault(sp => sp.Id == dinhMuc.SanPhamId);
                if (sanPham != null)
                    dinhMuc.SanPham = sanPham;

                var nguyenLieu = allNguyenLieus.FirstOrDefault(nl => nl.Id == dinhMuc.NguyenLieuId);
                if (nguyenLieu != null)
                    dinhMuc.NguyenLieu = nguyenLieu;
            }

            return result;
        }

        public async Task<DinhMucNguyenLieu?> GetBySanPhamAndNguyenLieuAsync(int sanPhamId, int nguyenLieuId)
        {
            var allDinhMucs = await GetAllAsync();
            return allDinhMucs.FirstOrDefault(d => d.SanPhamId == sanPhamId && d.NguyenLieuId == nguyenLieuId && d.TrangThai);
        }

        public async Task<DinhMucNguyenLieu> CreateAsync(DinhMucNguyenLieu dinhMucNguyenLieu)
        {
            var result = await _unitOfWork.Repository<DinhMucNguyenLieu>().AddAsync(dinhMucNguyenLieu);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateAsync(DinhMucNguyenLieu dinhMucNguyenLieu)
        {
            await _unitOfWork.Repository<DinhMucNguyenLieu>().UpdateAsync(dinhMucNguyenLieu);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dinhMuc = await GetByIdAsync(id);
            if (dinhMuc != null)
            {
                await _unitOfWork.Repository<DinhMucNguyenLieu>().DeleteAsync(dinhMuc);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<DinhMucNguyenLieu>().ExistsAsync(id);
        }

        public async Task<decimal> GetTongChiPhiNguyenLieuAsync(int sanPhamId)
        {
            var dinhMucs = await GetBySanPhamIdAsync(sanPhamId);
            return dinhMucs.Sum(d => d.SoLuongCan * (d.NguyenLieu?.GiaNhap ?? 0));
        }
    }
} 