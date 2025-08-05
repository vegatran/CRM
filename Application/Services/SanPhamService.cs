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

        public async Task<SanPham?> GetByIdWithDetailsAsync(int id)
        {
            var sanPham = await _unitOfWork.Repository<SanPham>().GetByIdAsync(id);
            if (sanPham != null)
            {
                // Load QuyTrinhSanXuat
                var quyTrinhs = await _unitOfWork.QuyTrinhSanXuatRepository.GetAllAsync();
                sanPham.QuyTrinhSanXuats = quyTrinhs.Where(q => q.SanPhamId == id && q.TrangThai).ToList();

                // Load DinhMucNguyenLieu với NguyenLieu
                var dinhMucs = await _unitOfWork.DinhMucNguyenLieuRepository.GetAllAsync();
                var dinhMucsForProduct = dinhMucs.Where(d => d.SanPhamId == id && d.TrangThai).ToList();
                
                // Load NguyenLieu cho từng DinhMuc
                foreach (var dinhMuc in dinhMucsForProduct)
                {
                    var nguyenLieu = await _unitOfWork.NguyenLieuRepository.GetByIdAsync(dinhMuc.NguyenLieuId);
                    if (nguyenLieu != null)
                    {
                        dinhMuc.NguyenLieu = nguyenLieu;
                    }
                }
                sanPham.DinhMucNguyenLieus = dinhMucsForProduct;
            }
            return sanPham;
        }

        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _unitOfWork.Repository<SanPham>().GetAllAsync();
        }

        public async Task<IEnumerable<SanPham>> GetAllWithDetailsAsync()
        {
            var allSanPhams = await GetAllAsync();
            var allQuyTrinhs = await _unitOfWork.QuyTrinhSanXuatRepository.GetAllAsync();
            var allDinhMucs = await _unitOfWork.DinhMucNguyenLieuRepository.GetAllAsync();
            var allNguyenLieus = await _unitOfWork.NguyenLieuRepository.GetAllAsync();

            foreach (var sanPham in allSanPhams)
            {
                // Load QuyTrinhSanXuat cho sản phẩm
                sanPham.QuyTrinhSanXuats = allQuyTrinhs
                    .Where(q => q.SanPhamId == sanPham.Id && q.TrangThai)
                    .ToList();

                // Load DinhMucNguyenLieu với NguyenLieu cho sản phẩm
                var dinhMucsForProduct = allDinhMucs
                    .Where(d => d.SanPhamId == sanPham.Id && d.TrangThai)
                    .ToList();

                foreach (var dinhMuc in dinhMucsForProduct)
                {
                    var nguyenLieu = allNguyenLieus.FirstOrDefault(nl => nl.Id == dinhMuc.NguyenLieuId);
                    if (nguyenLieu != null)
                    {
                        dinhMuc.NguyenLieu = nguyenLieu;
                    }
                }
                sanPham.DinhMucNguyenLieus = dinhMucsForProduct;
            }

            return allSanPhams;
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