using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class TestController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly ApplicationDbContext _context;

        public TestController(ISanPhamService sanPhamService, ApplicationDbContext context)
        {
            _sanPhamService = sanPhamService;
            _context = context;
        }

        public async Task<IActionResult> TestBaseEntity()
        {
            // Test tạo mới sản phẩm
            var sanPham = new SanPham
            {
                TenSanPham = "Test Product " + DateTime.Now.ToString("HHmmss"),
                MaSanPham = "TEST" + DateTime.Now.ToString("HHmmss"),
                GiaBan = 100000,
                GiaNhap = 80000,
                TrangThai = true
            };

            // Kiểm tra trước khi save
            var beforeSave = new
            {
                CreatedDate = sanPham.CreatedDate,
                CreatedBy = sanPham.CreatedBy,
                UpdatedDate = sanPham.UpdatedDate,
                UpdatedBy = sanPham.UpdatedBy,
                IsDeleted = sanPham.IsDeleted
            };

            // Lưu vào database
            var result = await _sanPhamService.CreateAsync(sanPham);

            // Kiểm tra sau khi save
            var afterSave = new
            {
                Id = result.Id,
                CreatedDate = result.CreatedDate,
                CreatedBy = result.CreatedBy,
                UpdatedDate = result.UpdatedDate,
                UpdatedBy = result.UpdatedBy,
                IsDeleted = result.IsDeleted
            };

            // Kiểm tra từ database
            var fromDb = await _context.SanPhams.FirstOrDefaultAsync(s => s.Id == result.Id);

            var fromDatabase = new
            {
                Id = fromDb?.Id,
                CreatedDate = fromDb?.CreatedDate,
                CreatedBy = fromDb?.CreatedBy,
                UpdatedDate = fromDb?.UpdatedDate,
                UpdatedBy = fromDb?.UpdatedBy,
                IsDeleted = fromDb?.IsDeleted
            };

            return Json(new
            {
                BeforeSave = beforeSave,
                AfterSave = afterSave,
                FromDatabase = fromDatabase
            });
        }

        public async Task<IActionResult> SimpleTest()
        {
            try
            {
                // Tạo sản phẩm mới
                var sanPham = new SanPham
                {
                    TenSanPham = "Test Simple",
                    MaSanPham = "SIMPLE" + DateTime.Now.ToString("HHmmss"),
                    GiaBan = 50000,
                    GiaNhap = 40000
                };

                // Lưu vào database
                var result = await _sanPhamService.CreateAsync(sanPham);

                return Json(new
                {
                    Success = true,
                    Message = "Test thành công!",
                    Data = new
                    {
                        Id = result.Id,
                        TenSanPham = result.TenSanPham,
                        CreatedDate = result.CreatedDate,
                        CreatedBy = result.CreatedBy,
                        IsDeleted = result.IsDeleted
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Test thất bại: " + ex.Message
                });
            }
        }

        public async Task<IActionResult> ViewAllSanPhams()
        {
            var sanPhams = await _context.SanPhams
                .OrderByDescending(s => s.CreatedDate)
                .Take(5)
                .Select(s => new
                {
                    s.Id,
                    s.TenSanPham,
                    s.MaSanPham,
                    s.CreatedDate,
                    s.CreatedBy,
                    s.UpdatedDate,
                    s.UpdatedBy,
                    s.IsDeleted
                })
                .ToListAsync();

            return Json(sanPhams);
        }

        public IActionResult TestModal()
        {
            return View();
        }
    }
} 