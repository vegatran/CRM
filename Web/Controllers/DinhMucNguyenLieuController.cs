using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace Web.Controllers
{
    public class DinhMucNguyenLieuController : Controller
    {
        private readonly IDinhMucNguyenLieuService _dinhMucNguyenLieuService;
        private readonly ISanPhamService _sanPhamService;
        private readonly INguyenLieuService _nguyenLieuService;

        public DinhMucNguyenLieuController(
            IDinhMucNguyenLieuService dinhMucNguyenLieuService,
            ISanPhamService sanPhamService,
            INguyenLieuService nguyenLieuService)
        {
            _dinhMucNguyenLieuService = dinhMucNguyenLieuService;
            _sanPhamService = sanPhamService;
            _nguyenLieuService = nguyenLieuService;
        }

        // GET: DinhMucNguyenLieu
        public async Task<IActionResult> Index()
        {
            var dinhMucs = await _dinhMucNguyenLieuService.GetAllWithDetailsAsync();
            return View(dinhMucs);
        }

        // GET: DinhMucNguyenLieu/Create
        public async Task<IActionResult> Create(int? sanPhamId)
        {
            ViewBag.SanPhams = await _sanPhamService.GetAllAsync();
            ViewBag.NguyenLieus = await _nguyenLieuService.GetAllAsync();
            
            var dinhMuc = new DinhMucNguyenLieu();
            if (sanPhamId.HasValue)
            {
                dinhMuc.SanPhamId = sanPhamId.Value;
            }
            
            return PartialView("_CreateContent", dinhMuc);
        }

        // POST: DinhMucNguyenLieu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SanPhamId,NguyenLieuId,SoLuongCan,DonViTinh,GhiChu,TrangThai")] DinhMucNguyenLieu dinhMucNguyenLieu)
        {
            // Bỏ qua validation cho các trường navigation property
            ModelState.Remove("SanPham");
            ModelState.Remove("NguyenLieu");

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem đã tồn tại định mức cho sản phẩm và nguyên liệu này chưa
                    var existingDinhMuc = await _dinhMucNguyenLieuService.GetBySanPhamAndNguyenLieuAsync(dinhMucNguyenLieu.SanPhamId, dinhMucNguyenLieu.NguyenLieuId);
                    
                    if (existingDinhMuc != null)
                    {
                        // Nếu đã tồn tại, cộng dồn số lượng
                        existingDinhMuc.SoLuongCan += dinhMucNguyenLieu.SoLuongCan;
                        
                        // Cập nhật ghi chú nếu có
                        if (!string.IsNullOrEmpty(dinhMucNguyenLieu.GhiChu))
                        {
                            if (!string.IsNullOrEmpty(existingDinhMuc.GhiChu))
                            {
                                existingDinhMuc.GhiChu += " | " + dinhMucNguyenLieu.GhiChu;
                            }
                            else
                            {
                                existingDinhMuc.GhiChu = dinhMucNguyenLieu.GhiChu;
                            }
                        }
                        
                        await _dinhMucNguyenLieuService.UpdateAsync(existingDinhMuc);
                        return Json(new { success = true, message = "Định mức nguyên liệu đã tồn tại, đã cộng dồn số lượng!" });
                    }
                    else
                    {
                        // Nếu chưa tồn tại, tạo mới
                        await _dinhMucNguyenLieuService.CreateAsync(dinhMucNguyenLieu);
                        return Json(new { success = true, message = "Thêm định mức nguyên liệu thành công!" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi thêm: " + ex.Message });
                }
            }
            
            // Nếu ModelState không hợp lệ, trả về lỗi Json
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join(", ", errors) });
        }

        // GET: DinhMucNguyenLieu/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dinhMuc = await _dinhMucNguyenLieuService.GetByIdAsync(id);
            if (dinhMuc == null)
            {
                return NotFound();
            }
            
            ViewBag.SanPhams = await _sanPhamService.GetAllAsync();
            ViewBag.NguyenLieus = await _nguyenLieuService.GetAllAsync();
            return PartialView("_EditContent", dinhMuc);
        }

        // POST: DinhMucNguyenLieu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SanPhamId,NguyenLieuId,SoLuongCan,DonViTinh,GhiChu,TrangThai")] DinhMucNguyenLieu dinhMucNguyenLieu)
        {
            if (id != dinhMucNguyenLieu.Id)
            {
                return Json(new { success = false, message = "ID không hợp lệ!" });
            }

            // Bỏ qua validation cho các trường navigation property
            ModelState.Remove("SanPham");
            ModelState.Remove("NguyenLieu");

            if (ModelState.IsValid)
            {
                try
                {
                    await _dinhMucNguyenLieuService.UpdateAsync(dinhMucNguyenLieu);
                    return Json(new { success = true, message = "Cập nhật định mức nguyên liệu thành công!" });
                }
                catch (Exception ex)
                {
                    if (!await _dinhMucNguyenLieuService.ExistsAsync(dinhMucNguyenLieu.Id))
                    {
                        return Json(new { success = false, message = "Không tìm thấy định mức nguyên liệu!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật: " + ex.Message });
                    }
                }
            }
            
            // Nếu ModelState không hợp lệ, trả về lỗi Json
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join(", ", errors) });
        }

        // GET: DinhMucNguyenLieu/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var dinhMuc = await _dinhMucNguyenLieuService.GetByIdWithDetailsAsync(id);
            if (dinhMuc == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteContent", dinhMuc);
        }

        // POST: DinhMucNguyenLieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dinhMucNguyenLieuService.DeleteAsync(id);
            return Json(new { success = true, message = "Xóa định mức nguyên liệu thành công!" });
        }

        // GET: DinhMucNguyenLieu/BySanPham/5
        [HttpGet]
        [Route("DinhMucNguyenLieu/BySanPham/{sanPhamId}")]
        public async Task<IActionResult> BySanPham(int sanPhamId)
        {
            Console.WriteLine($"DinhMucNguyenLieuController.BySanPham called with sanPhamId: {sanPhamId}");
            var dinhMucs = await _dinhMucNguyenLieuService.GetBySanPhamIdAsync(sanPhamId);
            Console.WriteLine($"Found {dinhMucs.Count()} dinhMucs for sanPhamId: {sanPhamId}");
            return PartialView("_DinhMucList", dinhMucs);
        }
    }
} 