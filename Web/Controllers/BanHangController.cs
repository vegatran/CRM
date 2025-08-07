using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System.Text;

namespace Web.Controllers
{
    public class BanHangController : Controller
    {
        private readonly IPhieuBanHangService _phieuBanHangService;
        private readonly ISanPhamService _sanPhamService;
        private readonly IKhachHangService _khachHangService;
        private readonly IUnitOfWork _unitOfWork;

        public BanHangController(
            IPhieuBanHangService phieuBanHangService,
            ISanPhamService sanPhamService,
            IKhachHangService khachHangService,
            IUnitOfWork unitOfWork)
        {
            _phieuBanHangService = phieuBanHangService;
            _sanPhamService = sanPhamService;
            _khachHangService = khachHangService;
            _unitOfWork = unitOfWork;
        }

        // GET: BanHang
        public async Task<IActionResult> Index()
        {
            var phieuBanHangs = await _phieuBanHangService.GetAllWithDetailsAsync();
            return View(phieuBanHangs);
        }

        // GET: BanHang/CreateContent
        public async Task<IActionResult> CreateContent()
        {
            ViewBag.KhachHangs = await _khachHangService.GetAllAsync();
            ViewBag.SanPhams = await _sanPhamService.GetAllAsync();
            return PartialView("_CreateContent", new PhieuBanHang());
        }

        // GET: BanHang/EditContent/5
        public async Task<IActionResult> EditContent(int id)
        {
            var phieuBanHang = await _phieuBanHangService.GetByIdWithDetailsAsync(id);
            if (phieuBanHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu bán hàng!" });
            }

            ViewBag.KhachHangs = await _khachHangService.GetAllAsync();
            ViewBag.SanPhams = await _sanPhamService.GetAllAsync();
            return PartialView("_EditContent", phieuBanHang);
        }

        // GET: BanHang/DetailsContent/5
        public async Task<IActionResult> DetailsContent(int id)
        {
            var phieuBanHang = await _phieuBanHangService.GetByIdWithDetailsAsync(id);
            if (phieuBanHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu bán hàng!" });
            }

            return PartialView("_DetailsContent", phieuBanHang);
        }

        // GET: BanHang/DeleteContent/5
        public async Task<IActionResult> DeleteContent(int id)
        {
            var phieuBanHang = await _phieuBanHangService.GetByIdWithDetailsAsync(id);
            if (phieuBanHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu bán hàng!" });
            }

            return PartialView("_DeleteContent", phieuBanHang);
        }

        // POST: BanHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] dynamic data)
        {
            try
            {
                var phieuBanHangData = data.phieuBanHang;
                var chiTietData = data.chiTietBanHangs;

                var phieuBanHang = new PhieuBanHang
                {
                    KhachHangId = phieuBanHangData.khachHangId,
                    NgayBan = DateTime.Parse(phieuBanHangData.ngayBan.ToString()),
                    GhiChu = phieuBanHangData.ghiChu?.ToString() ?? "",
                    SoPhieu = await GenerateSoPhieuAsync(),
                    TrangThai = "Chờ xử lý",
                    TongTien = 0,
                    GiamGia = phieuBanHangData.giamGia ?? 0,
                    ThanhToan = 0
                };

                var createdPhieu = await _phieuBanHangService.CreateAsync(phieuBanHang);

                // Thêm chi tiết bán hàng
                foreach (var chiTiet in chiTietData)
                {
                    var chiTietBanHang = new ChiTietBanHang
                    {
                        PhieuBanHangId = createdPhieu.Id,
                        SanPhamId = (int)chiTiet.sanPhamId,
                        SoLuong = (int)chiTiet.soLuong,
                        DonGia = (decimal)chiTiet.donGia,
                        ThanhTien = (int)chiTiet.soLuong * (decimal)chiTiet.donGia
                    };

                    await _phieuBanHangService.AddChiTietBanHangAsync(chiTietBanHang);
                    createdPhieu.TongTien += chiTietBanHang.ThanhTien;
                }

                // Tính thanh toán
                createdPhieu.ThanhToan = createdPhieu.TongTien - createdPhieu.GiamGia;
                await _phieuBanHangService.UpdateAsync(createdPhieu);

                return Json(new { success = true, message = "Tạo phiếu bán hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: BanHang/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] dynamic data)
        {
            try
            {
                var phieuBanHangData = data.phieuBanHang;
                var chiTietData = data.chiTietBanHangs;

                var phieuBanHang = await _phieuBanHangService.GetByIdAsync((int)phieuBanHangData.id);
                if (phieuBanHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu bán hàng!" });
                }

                phieuBanHang.KhachHangId = phieuBanHangData.khachHangId;
                phieuBanHang.NgayBan = DateTime.Parse(phieuBanHangData.ngayBan.ToString());
                phieuBanHang.GhiChu = phieuBanHangData.ghiChu?.ToString() ?? "";
                phieuBanHang.GiamGia = phieuBanHangData.giamGia ?? 0;

                await _phieuBanHangService.UpdateAsync(phieuBanHang);

                // Xóa chi tiết cũ và thêm chi tiết mới
                var existingChiTiet = await _unitOfWork.Repository<ChiTietBanHang>().GetAllAsync();
                var chiTietToDelete = existingChiTiet.Where(ct => ct.PhieuBanHangId == phieuBanHang.Id);
                
                foreach (var chiTiet in chiTietToDelete)
                {
                    await _unitOfWork.Repository<ChiTietBanHang>().DeleteAsync(chiTiet);
                }

                // Thêm chi tiết mới
                foreach (var chiTiet in chiTietData)
                {
                    var chiTietBanHang = new ChiTietBanHang
                    {
                        PhieuBanHangId = phieuBanHang.Id,
                        SanPhamId = (int)chiTiet.sanPhamId,
                        SoLuong = (int)chiTiet.soLuong,
                        DonGia = (decimal)chiTiet.donGia,
                        ThanhTien = (int)chiTiet.soLuong * (decimal)chiTiet.donGia
                    };

                    await _phieuBanHangService.AddChiTietBanHangAsync(chiTietBanHang);
                }

                return Json(new { success = true, message = "Cập nhật phiếu bán hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: BanHang/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var phieuBanHang = await _phieuBanHangService.GetByIdAsync(id);
                if (phieuBanHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu bán hàng!" });
                }

                if (phieuBanHang.TrangThai == "Đã xử lý")
                {
                    return Json(new { success = false, message = "Không thể xóa phiếu bán hàng đã xử lý!" });
                }

                await _phieuBanHangService.DeleteAsync(id);
                return Json(new { success = true, message = "Xóa phiếu bán hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: BanHang/XacNhan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int id)
        {
            try
            {
                var phieuBanHang = await _phieuBanHangService.GetByIdWithDetailsAsync(id);
                if (phieuBanHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu bán hàng!" });
                }

                if (phieuBanHang.TrangThai == "Đã xử lý")
                {
                    return Json(new { success = false, message = "Phiếu bán hàng đã được xử lý!" });
                }

                phieuBanHang.TrangThai = "Đã xử lý";
                await _phieuBanHangService.UpdateAsync(phieuBanHang);

                // Cập nhật tồn kho (giảm tồn kho)
                if (phieuBanHang.ChiTietBanHangs != null)
                {
                    foreach (var chiTiet in phieuBanHang.ChiTietBanHangs)
                    {
                        await UpdateSanPhamTonKhoAsync(chiTiet.SanPhamId, -chiTiet.SoLuong);
                    }
                }

                return Json(new { success = true, message = "Xác nhận phiếu bán hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        private async Task<string> GenerateSoPhieuAsync()
        {
            var today = DateTime.Today;
            var count = await _phieuBanHangService.GetByDateAsync(today);
            var countToday = count.Count() + 1;
            return $"BH{today:yyyyMMdd}{countToday:D3}";
        }

        private async Task UpdateSanPhamTonKhoAsync(int sanPhamId, int soLuong)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(sanPhamId);
            if (sanPham != null)
            {
                sanPham.SoLuongTon += soLuong;
                if (sanPham.SoLuongTon < 0) sanPham.SoLuongTon = 0;
                await _sanPhamService.UpdateAsync(sanPham);
            }
        }
    }
}
