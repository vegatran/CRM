using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

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
            var sanPhams = await _sanPhamService.GetAllAsync();
            return View(sanPhams);
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

        // GET: SanPham/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenSanPham,MoTa,MaSanPham,GiaBan,GiaNhap,KichThuoc,MauSac,ChatLieu,TrangThai")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                await _sanPhamService.CreateAsync(sanPham);
                return RedirectToAction(nameof(Index));
            }
            return View(sanPham);
        }

        // GET: SanPham/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            return View(sanPham);
        }

        // POST: SanPham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSanPham,MoTa,MaSanPham,GiaBan,GiaNhap,SoLuongTon,KichThuoc,MauSac,ChatLieu,TrangThai")] SanPham sanPham)
        {
            if (id != sanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _sanPhamService.UpdateAsync(sanPham);
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
                return RedirectToAction(nameof(Index));
            }
            return View(sanPham);
        }

        // GET: SanPham/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sanPhamService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 