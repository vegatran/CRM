using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;

namespace Web.Controllers
{
    public class XuatKhoController : Controller
    {
        private readonly IPhieuXuatKhoService _phieuXuatKhoService;

        public XuatKhoController(IPhieuXuatKhoService phieuXuatKhoService)
        {
            _phieuXuatKhoService = phieuXuatKhoService;
        }

        public async Task<IActionResult> Index()
        {
            var phieuXuatKhos = await _phieuXuatKhoService.GetAllAsync();
            return View(phieuXuatKhos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhieuXuatKho phieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                await _phieuXuatKhoService.CreateAsync(phieuXuatKho);
                return RedirectToAction(nameof(Index));
            }
            return View(phieuXuatKho);
        }

        public async Task<IActionResult> Details(int id)
        {
            var phieuXuatKho = await _phieuXuatKhoService.GetByIdAsync(id);
            if (phieuXuatKho == null)
            {
                return NotFound();
            }
            return View(phieuXuatKho);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var phieuXuatKho = await _phieuXuatKhoService.GetByIdAsync(id);
            if (phieuXuatKho == null)
            {
                return NotFound();
            }
            return View(phieuXuatKho);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PhieuXuatKho phieuXuatKho)
        {
            if (id != phieuXuatKho.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _phieuXuatKhoService.UpdateAsync(phieuXuatKho);
                return RedirectToAction(nameof(Index));
            }
            return View(phieuXuatKho);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var phieuXuatKho = await _phieuXuatKhoService.GetByIdAsync(id);
            if (phieuXuatKho == null)
            {
                return NotFound();
            }
            return View(phieuXuatKho);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _phieuXuatKhoService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
