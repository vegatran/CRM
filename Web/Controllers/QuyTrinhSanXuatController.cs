using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Application.DTOs;

namespace Web.Controllers
{
    public class QuyTrinhSanXuatController : Controller
    {
        private readonly IQuyTrinhSanXuatService _quyTrinhSanXuatService;
        private readonly ISanPhamService _sanPhamService;

        public QuyTrinhSanXuatController(
            IQuyTrinhSanXuatService quyTrinhSanXuatService,
            ISanPhamService sanPhamService)
        {
            _quyTrinhSanXuatService = quyTrinhSanXuatService;
            _sanPhamService = sanPhamService;
        }

        // GET: QuyTrinhSanXuat/BySanPham/5
        [HttpGet]
        [Route("QuyTrinhSanXuat/BySanPham/{sanPhamId}")]
        public async Task<IActionResult> BySanPham(int sanPhamId)
        {
            Console.WriteLine($"QuyTrinhSanXuatController.BySanPham called with sanPhamId: {sanPhamId}");
            var quyTrinhs = await _quyTrinhSanXuatService.GetBySanPhamIdAsync(sanPhamId);
            Console.WriteLine($"Found {quyTrinhs.Count()} quyTrinhs for sanPhamId: {sanPhamId}");
            return PartialView("_QuyTrinhList", quyTrinhs);
        }

        // GET: QuyTrinhSanXuat/Create
        public async Task<IActionResult> Create(int sanPhamId)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(sanPhamId);
            if (sanPham == null)
            {
                return NotFound();
            }

            ViewBag.SanPhamId = sanPhamId;
            ViewBag.SanPhamTen = sanPham.TenSanPham;
            return PartialView("_Create");
        }

        // POST: QuyTrinhSanXuat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuyTrinhSanXuat quyTrinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _quyTrinhSanXuatService.CreateAsync(quyTrinh);
                    return Json(new { success = true, message = "Thêm quy trình thành công!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }
            }

            ViewBag.SanPhamId = quyTrinh.SanPhamId;
            return PartialView("_Create", quyTrinh);
        }

        // GET: QuyTrinhSanXuat/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var quyTrinh = await _quyTrinhSanXuatService.GetByIdAsync(id);
            if (quyTrinh == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", quyTrinh);
        }

        // POST: QuyTrinhSanXuat/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuyTrinhSanXuat quyTrinh)
        {
            if (id != quyTrinh.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _quyTrinhSanXuatService.UpdateAsync(quyTrinh);
                    return Json(new { success = true, message = "Cập nhật quy trình thành công!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }
            }

            return PartialView("_Edit", quyTrinh);
        }

        // POST: QuyTrinhSanXuat/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _quyTrinhSanXuatService.DeleteAsync(id);
                return Json(new { success = true, message = "Xóa quy trình thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
} 