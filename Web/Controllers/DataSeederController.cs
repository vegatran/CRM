using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace Web.Controllers
{
    public class DataSeederController : Controller
    {
        private readonly IDataSeederService _dataSeederService;

        public DataSeederController(IDataSeederService dataSeederService)
        {
            _dataSeederService = dataSeederService;
        }

        public async Task<IActionResult> SeedData()
        {
            try
            {
                await _dataSeederService.SeedDataAsync();
                TempData["Success"] = "Đã xóa dữ liệu cũ và tạo dữ liệu mẫu mới thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tạo dữ liệu mẫu: {ex.Message}";
            }

            return RedirectToAction("Index", "Home");
        }
    }
} 