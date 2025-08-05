using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace Web.Controllers
{
    public class NguyenLieuController : Controller
    {
        private readonly INguyenLieuService _nguyenLieuService;
        private readonly INhaCungCapService _nhaCungCapService;
        private readonly ILoaiNguyenLieuService _loaiNguyenLieuService;

        public NguyenLieuController(INguyenLieuService nguyenLieuService, INhaCungCapService nhaCungCapService, ILoaiNguyenLieuService loaiNguyenLieuService)
        {
            _nguyenLieuService = nguyenLieuService;
            _nhaCungCapService = nhaCungCapService;
            _loaiNguyenLieuService = loaiNguyenLieuService;
        }

        // GET: NguyenLieu
        public async Task<IActionResult> Index()
        {
            var nguyenLieus = await _nguyenLieuService.GetAllWithDetailsAsync();
            return View(nguyenLieus);
        }

        // GET: NguyenLieu/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.NhaCungCaps = await _nhaCungCapService.GetAllAsync();
            ViewBag.LoaiNguyenLieus = await _loaiNguyenLieuService.GetAllAsync();
            return PartialView("_CreateModal");
        }

        // POST: NguyenLieu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenNguyenLieu,MoTa,MaNguyenLieu,GiaNhap,SoLuongTon,DonViTinh,ChatLieu,MauSac,HinhAnh,NhaCungCapId,LoaiNguyenLieuId,TrangThai")] NguyenLieu nguyenLieu)
        {
            // Parse currency values from form
            var giaNhapStr = Request.Form["GiaNhap"].ToString();
            var giaNhapClean = giaNhapStr.Replace(",", "").Replace(".", "");
            
            if (decimal.TryParse(giaNhapClean, out decimal giaNhap))
            {
                nguyenLieu.GiaNhap = giaNhap;
            }

            // Handle image upload
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "materials");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                nguyenLieu.HinhAnh = "/uploads/materials/" + fileName;
            }

            if (ModelState.IsValid)
            {
                await _nguyenLieuService.CreateAsync(nguyenLieu);
                return Json(new { success = true, message = "Thêm nguyên liệu thành công!" });
            }
            
            ViewBag.NhaCungCaps = await _nhaCungCapService.GetAllAsync();
            ViewBag.LoaiNguyenLieus = await _loaiNguyenLieuService.GetAllAsync();
            return PartialView("_CreateModal", nguyenLieu);
        }

        // GET: NguyenLieu/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var nguyenLieu = await _nguyenLieuService.GetByIdAsync(id);
            if (nguyenLieu == null)
            {
                return NotFound();
            }
            
            ViewBag.NhaCungCaps = await _nhaCungCapService.GetAllAsync();
            ViewBag.LoaiNguyenLieus = await _loaiNguyenLieuService.GetAllAsync();
            return PartialView("_EditModal", nguyenLieu);
        }

        // POST: NguyenLieu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNguyenLieu,MoTa,MaNguyenLieu,GiaNhap,SoLuongTon,DonViTinh,ChatLieu,MauSac,HinhAnh,NhaCungCapId,LoaiNguyenLieuId,TrangThai")] NguyenLieu nguyenLieu)
        {
            if (id != nguyenLieu.Id)
            {
                return NotFound();
            }

            // Parse currency values from form
            var giaNhapStr = Request.Form["GiaNhap"].ToString();
            var giaNhapClean = giaNhapStr.Replace(",", "").Replace(".", "");
            
            if (decimal.TryParse(giaNhapClean, out decimal giaNhap))
            {
                nguyenLieu.GiaNhap = giaNhap;
            }

            // Handle image upload
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "materials");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                nguyenLieu.HinhAnh = "/uploads/materials/" + fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _nguyenLieuService.UpdateAsync(nguyenLieu);
                    return Json(new { success = true, message = "Cập nhật nguyên liệu thành công!" });
                }
                catch (Exception)
                {
                    if (!await _nguyenLieuService.ExistsAsync(nguyenLieu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewBag.NhaCungCaps = await _nhaCungCapService.GetAllAsync();
            ViewBag.LoaiNguyenLieus = await _loaiNguyenLieuService.GetAllAsync();
            return PartialView("_EditModal", nguyenLieu);
        }

        // GET: NguyenLieu/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var nguyenLieu = await _nguyenLieuService.GetByIdAsync(id);
            if (nguyenLieu == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteModal", nguyenLieu);
        }

        // POST: NguyenLieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nguyenLieuService.DeleteAsync(id);
            return Json(new { success = true, message = "Xóa nguyên liệu thành công!" });
        }
    }
} 