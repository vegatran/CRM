using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace Web.Controllers
{
    public class NhaCungCapController : Controller
    {
        private readonly INhaCungCapService _nhaCungCapService;

        public NhaCungCapController(INhaCungCapService nhaCungCapService)
        {
            _nhaCungCapService = nhaCungCapService;
        }

        // GET: NhaCungCap
        public async Task<IActionResult> Index()
        {
            var nhaCungCaps = await _nhaCungCapService.GetAllAsync();
            return View(nhaCungCaps);
        }

        // GET: NhaCungCap/Create
        public IActionResult Create()
        {
            return PartialView("_CreateModal");
        }

        // POST: NhaCungCap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenNhaCungCap,DiaChi,SoDienThoai,Email,MaSoThue,NguoiDaiDien,TrangThai")] NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                await _nhaCungCapService.CreateAsync(nhaCungCap);
                return Json(new { success = true, message = "Thêm nhà cung cấp thành công!" });
            }
            return PartialView("_CreateModal", nhaCungCap);
        }

        // GET: NhaCungCap/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var nhaCungCap = await _nhaCungCapService.GetByIdAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }
            return PartialView("_EditModal", nhaCungCap);
        }

        // POST: NhaCungCap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenNhaCungCap,DiaChi,SoDienThoai,Email,MaSoThue,NguoiDaiDien,TrangThai")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _nhaCungCapService.UpdateAsync(nhaCungCap);
                    return Json(new { success = true, message = "Cập nhật nhà cung cấp thành công!" });
                }
                catch (Exception)
                {
                    if (!await _nhaCungCapService.ExistsAsync(nhaCungCap.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("_EditModal", nhaCungCap);
        }

        // GET: NhaCungCap/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var nhaCungCap = await _nhaCungCapService.GetByIdAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteModal", nhaCungCap);
        }

        // POST: NhaCungCap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nhaCungCapService.DeleteAsync(id);
            return Json(new { success = true, message = "Xóa nhà cung cấp thành công!" });
        }
    }
} 