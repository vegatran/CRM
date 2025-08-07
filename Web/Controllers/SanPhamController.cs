using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using System.IO;

namespace Web.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        // GET: SanPham
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _sanPhamService.GetAllWithDetailsAsync();
            return View(sanPhams);
        }

        // GET: SanPham/GetAllForSelect
        [HttpGet]
        public async Task<IActionResult> GetAllForSelect()
        {
            var sanPhams = await _sanPhamService.GetAllAsync();
            var result = sanPhams.Select(sp => new { id = sp.Id, tenSanPham = sp.TenSanPham, giaNhap = sp.GiaNhap });
            return Json(result);
        }

        // GET: SanPham/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPham/ChiTietSanXuat/5
        public async Task<IActionResult> ChiTietSanXuat(int id)
        {
            var sanPham = await _sanPhamService.GetByIdWithDetailsAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPham/Create
        public IActionResult Create()
        {
            return PartialView("_CreateModal");
        }

        // POST: SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenSanPham,MoTa,MaSanPham,GiaBan,GiaNhap,ChiPhiNhanCong,KichThuoc,MauSac,ChatLieu,TrangThai")] SanPham sanPham)
        {
            // Parse currency values from form
            var giaBanStr = Request.Form["GiaBan"].ToString();
            var giaNhapStr = Request.Form["GiaNhap"].ToString();
            var chiPhiNhanCongStr = Request.Form["ChiPhiNhanCong"].ToString();
            
            // Remove all non-digit characters except decimal point
            var giaBanClean = giaBanStr.Replace(",", "").Replace(".", "");
            var giaNhapClean = giaNhapStr.Replace(",", "").Replace(".", "");
            var chiPhiNhanCongClean = chiPhiNhanCongStr.Replace(",", "").Replace(".", "");
            
            if (decimal.TryParse(giaBanClean, out decimal giaBan))
            {
                sanPham.GiaBan = giaBan;
            }
            
            if (decimal.TryParse(giaNhapClean, out decimal giaNhap))
            {
                sanPham.GiaNhap = giaNhap;
            }
            
            if (decimal.TryParse(chiPhiNhanCongClean, out decimal chiPhiNhanCong))
            {
                sanPham.ChiPhiNhanCong = chiPhiNhanCong;
            }

            // Handle image upload
            var imageFile = Request.Form.Files.FirstOrDefault(f => f.Name == "HinhAnh");
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                sanPham.HinhAnh = "/uploads/products/" + fileName;
            }
            
            if (ModelState.IsValid)
            {
                await _sanPhamService.CreateAsync(sanPham);
                return Json(new { success = true, message = "Thêm sản phẩm thành công!" });
            }
            return PartialView("_CreateModal", sanPham);
        }

        // GET: SanPham/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            return PartialView("_EditModal", sanPham);
        }

        // POST: SanPham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSanPham,MoTa,MaSanPham,GiaBan,GiaNhap,ChiPhiNhanCong,SoLuongTon,KichThuoc,MauSac,ChatLieu,TrangThai")] SanPham sanPham)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            // Parse currency values from form
            var giaBanStr = Request.Form["GiaBan"].ToString();
            var giaNhapStr = Request.Form["GiaNhap"].ToString();
            var chiPhiNhanCongStr = Request.Form["ChiPhiNhanCong"].ToString();
            
            // Remove all non-digit characters except decimal point
            var giaBanClean = giaBanStr.Replace(",", "").Replace(".", "");
            var giaNhapClean = giaNhapStr.Replace(",", "").Replace(".", "");
            var chiPhiNhanCongClean = chiPhiNhanCongStr.Replace(",", "").Replace(".", "");
            
            if (decimal.TryParse(giaBanClean, out decimal giaBan))
            {
                sanPham.GiaBan = giaBan;
            }
            
            if (decimal.TryParse(giaNhapClean, out decimal giaNhap))
            {
                sanPham.GiaNhap = giaNhap;
            }
            
            if (decimal.TryParse(chiPhiNhanCongClean, out decimal chiPhiNhanCong))
            {
                sanPham.ChiPhiNhanCong = chiPhiNhanCong;
            }

            // Handle image upload
            var imageFile = Request.Form.Files.FirstOrDefault(f => f.Name == "HinhAnh");
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                sanPham.HinhAnh = "/uploads/products/" + fileName;
            }
            else
            {
                // Nếu không có hình ảnh mới được upload, giữ lại hình ảnh hiện tại
                var currentHinhAnh = Request.Form["CurrentHinhAnh"].ToString();
                if (!string.IsNullOrEmpty(currentHinhAnh))
                {
                    sanPham.HinhAnh = currentHinhAnh;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _sanPhamService.UpdateAsync(sanPham);
                    return Json(new { success = true, message = "Cập nhật sản phẩm thành công!" });
                }
                catch (Exception)
                {
                    if (!await _sanPhamService.ExistsAsync(sanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("_EditModal", sanPham);
        }

        // GET: SanPham/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteModal", sanPham);
        }

        // POST: SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sanPhamService.DeleteAsync(id);
            return Json(new { success = true, message = "Xóa sản phẩm thành công!" });
        }
    }
} 