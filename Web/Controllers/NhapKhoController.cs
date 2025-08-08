using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System.Text.Json;

namespace Web.Controllers
{
    public class PhieuNhapKhoRequest
    {
        public int nhaCungCapId { get; set; }
        public string ngayNhap { get; set; } = string.Empty;
        public string? ghiChu { get; set; }
    }

    public class ChiTietNhapKhoRequest
    {
        public string loaiHang { get; set; } = string.Empty;
        public int hangHoaId { get; set; }
        public int soLuong { get; set; }
        public decimal donGia { get; set; }
        public int? nguyenLieuId { get; set; }
        public int? sanPhamId { get; set; }
    }

    public class CreateNhapKhoRequest
    {
        public PhieuNhapKhoRequest phieuNhapKho { get; set; } = new();
        public List<ChiTietNhapKhoRequest> chiTietNhapKhos { get; set; } = new();
    }

    public class EditNhapKhoRequest
    {
        public int id { get; set; }
        public string soPhieu { get; set; } = string.Empty;
        public string trangThai { get; set; } = string.Empty;
        public int nhaCungCapId { get; set; }
        public string ngayNhap { get; set; } = string.Empty;
        public string? ghiChu { get; set; }
    }

    public class EditNhapKhoFullRequest
    {
        public EditNhapKhoRequest phieuNhapKho { get; set; } = new();
        public List<ChiTietNhapKhoRequest> chiTietNhapKhos { get; set; } = new();
    }

    public class NhapKhoController : Controller
    {
        private readonly IPhieuNhapKhoService _phieuNhapKhoService;
        private readonly INguyenLieuService _nguyenLieuService;
        private readonly ISanPhamService _sanPhamService;
        private readonly INhaCungCapService _nhaCungCapService;
        private readonly IUnitOfWork _unitOfWork;

        public NhapKhoController(
            IPhieuNhapKhoService phieuNhapKhoService,
            INguyenLieuService nguyenLieuService,
            ISanPhamService sanPhamService,
            INhaCungCapService nhaCungCapService,
            IUnitOfWork unitOfWork)
        {
            _phieuNhapKhoService = phieuNhapKhoService;
            _nguyenLieuService = nguyenLieuService;
            _sanPhamService = sanPhamService;
            _nhaCungCapService = nhaCungCapService;
            _unitOfWork = unitOfWork;
        }

        // GET: NhapKho
        public async Task<IActionResult> Index()
        {
            var phieuNhapKhos = await _phieuNhapKhoService.GetAllWithDetailsAsync();
            return View(phieuNhapKhos);
        }

        // GET: NhapKho/CreateContent
        public async Task<IActionResult> CreateContent()
        {
            ViewBag.NhaCungCaps = await _nhaCungCapService.GetAllAsync();
            return PartialView("_CreateContent", new PhieuNhapKho());
        }

        // GET: NhapKho/EditContent/5
        public async Task<IActionResult> EditContent(int id)
        {
            var phieuNhapKho = await _phieuNhapKhoService.GetByIdWithDetailsAsync(id);
            if (phieuNhapKho == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho!" });
            }

            ViewBag.NhaCungCaps = await _nhaCungCapService.GetAllAsync();
            return PartialView("_EditContent", phieuNhapKho);
        }

        // GET: NhapKho/DetailsContent/5
        public async Task<IActionResult> DetailsContent(int id)
        {
            var phieuNhapKho = await _phieuNhapKhoService.GetByIdWithDetailsAsync(id);
            if (phieuNhapKho == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho!" });
            }

            return PartialView("_DetailsContent", phieuNhapKho);
        }

        // GET: NhapKho/DeleteContent/5
        public async Task<IActionResult> DeleteContent(int id)
        {
            var phieuNhapKho = await _phieuNhapKhoService.GetByIdWithDetailsAsync(id);
            if (phieuNhapKho == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho!" });
            }

            return PartialView("_DeleteContent", phieuNhapKho);
        }

        // GET: NhapKho/GetAllForSelect
        [HttpGet]
        public async Task<IActionResult> GetAllForSelect()
        {
            var nguyenLieus = await _nguyenLieuService.GetAllAsync();
            var sanPhams = await _sanPhamService.GetAllAsync();

            var result = new
            {
                nguyenLieus = nguyenLieus.Select(nl => new { id = nl.Id, tenNguyenLieu = nl.TenNguyenLieu, giaNhap = nl.GiaNhap }),
                sanPhams = sanPhams.Select(sp => new { id = sp.Id, tenSanPham = sp.TenSanPham, giaNhap = sp.GiaNhap })
            };

            return Json(result);
        }

        // POST: NhapKho/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNhapKhoRequest data)
        {
            try
            {
                if (data == null)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
                }

                // Validation
                if (data.phieuNhapKho.nhaCungCapId <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn nhà cung cấp!" });
                }

                if (string.IsNullOrEmpty(data.phieuNhapKho.ngayNhap))
                {
                    return Json(new { success = false, message = "Vui lòng chọn ngày nhập!" });
                }

                if (data.chiTietNhapKhos.Count == 0)
                {
                    return Json(new { success = false, message = "Vui lòng thêm ít nhất một chi tiết nhập kho!" });
                }

                var phieuNhapKho = new PhieuNhapKho
                {
                    NhaCungCapId = data.phieuNhapKho.nhaCungCapId,
                    NgayNhap = DateTime.Parse(data.phieuNhapKho.ngayNhap),
                    GhiChu = data.phieuNhapKho.ghiChu ?? "",
                    SoPhieu = await GenerateSoPhieuAsync(),
                    TrangThai = "Chờ xử lý",
                    TongTien = 0
                };

                var createdPhieu = await _phieuNhapKhoService.CreateAsync(phieuNhapKho);

                // Thêm chi tiết nhập kho
                foreach (var chiTiet in data.chiTietNhapKhos)
                {
                    var chiTietNhapKho = new ChiTietNhapKho
                    {
                        PhieuNhapKhoId = createdPhieu.Id,
                        SoLuong = chiTiet.soLuong,
                        DonGia = chiTiet.donGia,
                        ThanhTien = chiTiet.soLuong * chiTiet.donGia
                    };

                    if (chiTiet.loaiHang == "nguyenlieu")
                    {
                        chiTietNhapKho.NguyenLieuId = chiTiet.nguyenLieuId;
                    }
                    else
                    {
                        chiTietNhapKho.SanPhamId = chiTiet.sanPhamId;
                    }

                    await _phieuNhapKhoService.AddChiTietNhapKhoAsync(chiTietNhapKho);
                    createdPhieu.TongTien += chiTietNhapKho.ThanhTien;
                }

                // Cập nhật tổng tiền phiếu
                createdPhieu.TongTien = createdPhieu.TongTien;
                await _phieuNhapKhoService.UpdateAsync(createdPhieu);

                return Json(new { success = true, message = "Tạo phiếu nhập kho thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: NhapKho/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] EditNhapKhoFullRequest data)
        {
            try
            {
                if (data == null)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
                }

                var phieuNhapKho = await _phieuNhapKhoService.GetByIdAsync(data.phieuNhapKho.id);
                if (phieuNhapKho == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho!" });
                }

                phieuNhapKho.NhaCungCapId = data.phieuNhapKho.nhaCungCapId;
                phieuNhapKho.NgayNhap = DateTime.Parse(data.phieuNhapKho.ngayNhap);
                phieuNhapKho.GhiChu = data.phieuNhapKho.ghiChu ?? "";

                await _phieuNhapKhoService.UpdateAsync(phieuNhapKho);

                // Xóa chi tiết cũ và thêm chi tiết mới
                var existingChiTiet = await _unitOfWork.Repository<ChiTietNhapKho>().GetAllAsync();
                var chiTietToDelete = existingChiTiet.Where(ct => ct.PhieuNhapKhoId == phieuNhapKho.Id);
                
                foreach (var chiTiet in chiTietToDelete)
                {
                    await _unitOfWork.Repository<ChiTietNhapKho>().DeleteAsync(chiTiet);
                }

                // Thêm chi tiết mới
                foreach (var chiTiet in data.chiTietNhapKhos)
                {
                    var chiTietNhapKho = new ChiTietNhapKho
                    {
                        PhieuNhapKhoId = phieuNhapKho.Id,
                        SoLuong = chiTiet.soLuong,
                        DonGia = chiTiet.donGia,
                        ThanhTien = chiTiet.soLuong * chiTiet.donGia
                    };

                    if (chiTiet.loaiHang == "nguyenlieu")
                    {
                        chiTietNhapKho.NguyenLieuId = chiTiet.nguyenLieuId;
                    }
                    else
                    {
                        chiTietNhapKho.SanPhamId = chiTiet.sanPhamId;
                    }

                    await _phieuNhapKhoService.AddChiTietNhapKhoAsync(chiTietNhapKho);
                }

                return Json(new { success = true, message = "Cập nhật phiếu nhập kho thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: NhapKho/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var phieuNhapKho = await _phieuNhapKhoService.GetByIdAsync(id);
                if (phieuNhapKho == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho!" });
                }

                if (phieuNhapKho.TrangThai == "Đã xử lý")
                {
                    return Json(new { success = false, message = "Không thể xóa phiếu nhập kho đã xử lý!" });
                }

                await _phieuNhapKhoService.DeleteAsync(id);
                return Json(new { success = true, message = "Xóa phiếu nhập kho thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // POST: NhapKho/XacNhan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int id)
        {
            try
            {
                var phieuNhapKho = await _phieuNhapKhoService.GetByIdWithDetailsAsync(id);
                if (phieuNhapKho == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy phiếu nhập kho!" });
                }

                if (phieuNhapKho.TrangThai == "Đã xử lý")
                {
                    return Json(new { success = false, message = "Phiếu nhập kho đã được xử lý!" });
                }

                phieuNhapKho.TrangThai = "Đã xử lý";
                await _phieuNhapKhoService.UpdateAsync(phieuNhapKho);

                // Cập nhật tồn kho
                if (phieuNhapKho.ChiTietNhapKhos != null)
                {
                    foreach (var chiTiet in phieuNhapKho.ChiTietNhapKhos)
                    {
                        if (chiTiet.NguyenLieuId.HasValue)
                        {
                            await UpdateNguyenLieuTonKhoAsync(chiTiet.NguyenLieuId.Value, chiTiet.SoLuong);
                        }
                        else if (chiTiet.SanPhamId.HasValue)
                        {
                            await UpdateSanPhamTonKhoAsync(chiTiet.SanPhamId.Value, chiTiet.SoLuong);
                        }
                    }
                }

                return Json(new { success = true, message = "Xác nhận phiếu nhập kho thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        private async Task<string> GenerateSoPhieuAsync()
        {
            var today = DateTime.Today;
            var count = await _phieuNhapKhoService.GetByDateAsync(today);
            var countToday = count.Count() + 1;
            return $"NK{today:yyyyMMdd}{countToday:D3}";
        }

        private async Task UpdateNguyenLieuTonKhoAsync(int nguyenLieuId, int soLuong)
        {
            var nguyenLieu = await _nguyenLieuService.GetByIdAsync(nguyenLieuId);
            if (nguyenLieu != null)
            {
                nguyenLieu.SoLuongTon += soLuong;
                await _nguyenLieuService.UpdateAsync(nguyenLieu);
            }
        }

        private async Task UpdateSanPhamTonKhoAsync(int sanPhamId, int soLuong)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(sanPhamId);
            if (sanPham != null)
            {
                sanPham.SoLuongTon += soLuong;
                await _sanPhamService.UpdateAsync(sanPham);
            }
        }
    }
}
