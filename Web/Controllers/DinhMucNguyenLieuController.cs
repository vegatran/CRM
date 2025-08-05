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
            
            return PartialView("_CreateModal", dinhMuc);
        }

        // POST: DinhMucNguyenLieu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SanPhamId,NguyenLieuId,SoLuongCan,DonViTinh,GhiChu,TrangThai")] DinhMucNguyenLieu dinhMucNguyenLieu)
        {
            if (ModelState.IsValid)
            {
                await _dinhMucNguyenLieuService.CreateAsync(dinhMucNguyenLieu);
                return Json(new { success = true, message = "Thêm định mức nguyên liệu thành công!" });
            }
            
            ViewBag.SanPhams = await _sanPhamService.GetAllAsync();
            ViewBag.NguyenLieus = await _nguyenLieuService.GetAllAsync();
            return PartialView("_CreateModal", dinhMucNguyenLieu);
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
            return PartialView("_EditModal", dinhMuc);
        }

        // POST: DinhMucNguyenLieu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SanPhamId,NguyenLieuId,SoLuongCan,DonViTinh,GhiChu,TrangThai")] DinhMucNguyenLieu dinhMucNguyenLieu)
        {
            if (id != dinhMucNguyenLieu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dinhMucNguyenLieuService.UpdateAsync(dinhMucNguyenLieu);
                    return Json(new { success = true, message = "Cập nhật định mức nguyên liệu thành công!" });
                }
                catch (Exception)
                {
                    if (!await _dinhMucNguyenLieuService.ExistsAsync(dinhMucNguyenLieu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewBag.SanPhams = await _sanPhamService.GetAllAsync();
            ViewBag.NguyenLieus = await _nguyenLieuService.GetAllAsync();
            return PartialView("_EditModal", dinhMucNguyenLieu);
        }

        // GET: DinhMucNguyenLieu/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var dinhMuc = await _dinhMucNguyenLieuService.GetByIdAsync(id);
            if (dinhMuc == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteModal", dinhMuc);
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