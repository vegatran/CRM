using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace Web.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        public async Task<IActionResult> Index()
        {
            var khachHangs = await _khachHangService.GetAllAsync();
            return View(khachHangs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                await _khachHangService.CreateAsync(khachHang);
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        public async Task<IActionResult> Details(int id)
        {
            var khachHang = await _khachHangService.GetByIdAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var khachHang = await _khachHangService.GetByIdAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, KhachHang khachHang)
        {
            if (id != khachHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _khachHangService.UpdateAsync(khachHang);
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var khachHang = await _khachHangService.GetByIdAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _khachHangService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
