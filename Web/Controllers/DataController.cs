using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;

namespace Web.Controllers
{
    public class DataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SeedData()
        {
            try
            {
                // 1. Insert Nhà cung cấp
                var nhaCungCaps = new List<NhaCungCap>
                {
                    new NhaCungCap
                    {
                        TenNhaCungCap = "Công ty TNHH Vải Việt Nam",
                        DiaChi = "123 Đường Nguyễn Văn Linh, Quận 7, TP.HCM",
                        SoDienThoai = "+84 28 1234 5678",
                        Email = "info@vaivietnam.com",
                        MaSoThue = "0123456789",
                        NguoiDaiDien = "Nguyễn Văn An",
                        TrangThai = true
                    },
                    new NhaCungCap
                    {
                        TenNhaCungCap = "Công ty CP Chỉ May Thành Công",
                        DiaChi = "456 Đường Lê Văn Việt, Quận 9, TP.HCM",
                        SoDienThoai = "+84 28 2345 6789",
                        Email = "contact@chithanhcong.com",
                        MaSoThue = "0234567890",
                        NguoiDaiDien = "Trần Thị Bình",
                        TrangThai = true
                    },
                    new NhaCungCap
                    {
                        TenNhaCungCap = "Công ty TNHH Phụ Liệu May Mặc",
                        DiaChi = "789 Đường Mai Chí Thọ, Quận 2, TP.HCM",
                        SoDienThoai = "+84 28 3456 7890",
                        Email = "sales@phulieu.com",
                        MaSoThue = "0345678901",
                        NguoiDaiDien = "Lê Văn Cường",
                        TrangThai = true
                    },
                    new NhaCungCap
                    {
                        TenNhaCungCap = "Công ty CP Khuy Cúc Việt Nam",
                        DiaChi = "321 Đường Võ Văn Ngân, Thủ Đức, TP.HCM",
                        SoDienThoai = "+84 28 4567 8901",
                        Email = "info@khuycuc.com",
                        MaSoThue = "0456789012",
                        NguoiDaiDien = "Phạm Thị Dung",
                        TrangThai = true
                    }
                };

                foreach (var ncc in nhaCungCaps)
                {
                    if (!await _context.NhaCungCaps.AnyAsync(x => x.TenNhaCungCap == ncc.TenNhaCungCap))
                    {
                        _context.NhaCungCaps.Add(ncc);
                    }
                }

                await _context.SaveChangesAsync();

                // 2. Insert LoaiNguyenLieu
                var loaiNguyenLieus = new List<LoaiNguyenLieu>
                {
                    new LoaiNguyenLieu { TenLoai = "Vải", MoTa = "Các loại vải dùng để may quần áo", TrangThai = true },
                    new LoaiNguyenLieu { TenLoai = "Chỉ may", MoTa = "Các loại chỉ dùng để may", TrangThai = true },
                    new LoaiNguyenLieu { TenLoai = "Khuy", MoTa = "Các loại khuy, cúc, nút", TrangThai = true },
                    new LoaiNguyenLieu { TenLoai = "Phụ liệu khác", MoTa = "Các phụ liệu khác dùng trong may mặc", TrangThai = true }
                };

                foreach (var loai in loaiNguyenLieus)
                {
                    if (!await _context.LoaiNguyenLieus.AnyAsync(x => x.TenLoai == loai.TenLoai))
                    {
                        _context.LoaiNguyenLieus.Add(loai);
                    }
                }

                await _context.SaveChangesAsync();

                // 3. Insert Nguyên liệu
                var nhaCungCap1 = await _context.NhaCungCaps.FirstAsync(x => x.TenNhaCungCap == "Công ty TNHH Vải Việt Nam");
                var nhaCungCap2 = await _context.NhaCungCaps.FirstAsync(x => x.TenNhaCungCap == "Công ty CP Chỉ May Thành Công");
                var nhaCungCap3 = await _context.NhaCungCaps.FirstAsync(x => x.TenNhaCungCap == "Công ty TNHH Phụ Liệu May Mặc");
                var nhaCungCap4 = await _context.NhaCungCaps.FirstAsync(x => x.TenNhaCungCap == "Công ty CP Khuy Cúc Việt Nam");

                var loaiVai = await _context.LoaiNguyenLieus.FirstAsync(x => x.TenLoai == "Vải");
                var loaiChi = await _context.LoaiNguyenLieus.FirstAsync(x => x.TenLoai == "Chỉ may");
                var loaiKhuy = await _context.LoaiNguyenLieus.FirstAsync(x => x.TenLoai == "Khuy");
                var loaiPhuLieu = await _context.LoaiNguyenLieus.FirstAsync(x => x.TenLoai == "Phụ liệu khác");

                var nguyenLieus = new List<NguyenLieu>
                {
                    // Vải chính cho áo sơ mi
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Vải Cotton 100% Trắng",
                        MoTa = "Vải cotton 100% màu trắng, độ dày vừa phải, phù hợp may áo sơ mi",
                        MaNguyenLieu = "VAI001",
                        GiaNhap = 85000,
                        SoLuongTon = 500,
                        DonViTinh = "Mét",
                        ChatLieu = "Cotton 100%",
                        NhaCungCapId = nhaCungCap1.Id,
                        LoaiNguyenLieuId = loaiVai.Id,
                        TrangThai = true
                    },
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Vải lót Polyester Trắng",
                        MoTa = "Vải lót polyester màu trắng, mềm mại, dùng lót cổ áo và viền",
                        MaNguyenLieu = "VAI002",
                        GiaNhap = 45000,
                        SoLuongTon = 300,
                        DonViTinh = "Mét",
                        ChatLieu = "Polyester",
                        NhaCungCapId = nhaCungCap1.Id,
                        LoaiNguyenLieuId = loaiVai.Id,
                        TrangThai = true
                    },
                    // Chỉ may
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Chỉ may Polyester Trắng",
                        MoTa = "Chỉ may polyester màu trắng, độ bền cao, phù hợp may áo sơ mi",
                        MaNguyenLieu = "CHI001",
                        GiaNhap = 25000,
                        SoLuongTon = 100,
                        DonViTinh = "Cuộn",
                        ChatLieu = "Polyester",
                        NhaCungCapId = nhaCungCap2.Id,
                        LoaiNguyenLieuId = loaiChi.Id,
                        TrangThai = true
                    },
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Chỉ may Cotton Trắng",
                        MoTa = "Chỉ may cotton màu trắng, mềm mại, dùng may đường viền",
                        MaNguyenLieu = "CHI002",
                        GiaNhap = 30000,
                        SoLuongTon = 80,
                        DonViTinh = "Cuộn",
                        ChatLieu = "Cotton",
                        NhaCungCapId = nhaCungCap2.Id,
                        LoaiNguyenLieuId = loaiChi.Id,
                        TrangThai = true
                    },
                    // Khuy cúc
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Khuy nhựa Trắng 15mm",
                        MoTa = "Khuy nhựa màu trắng đường kính 15mm, dùng cho áo sơ mi",
                        MaNguyenLieu = "KHUY001",
                        GiaNhap = 5000,
                        SoLuongTon = 200,
                        DonViTinh = "Cái",
                        ChatLieu = "Nhựa",
                        NhaCungCapId = nhaCungCap4.Id,
                        LoaiNguyenLieuId = loaiKhuy.Id,
                        TrangThai = true
                    },
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Khuy nhựa Trắng 12mm",
                        MoTa = "Khuy nhựa màu trắng đường kính 12mm, dùng cho túi áo",
                        MaNguyenLieu = "KHUY002",
                        GiaNhap = 4000,
                        SoLuongTon = 150,
                        DonViTinh = "Cái",
                        ChatLieu = "Nhựa",
                        NhaCungCapId = nhaCungCap4.Id,
                        LoaiNguyenLieuId = loaiKhuy.Id,
                        TrangThai = true
                    },
                    // Phụ liệu khác
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Dây kéo Polyester Trắng 20cm",
                        MoTa = "Dây kéo polyester màu trắng dài 20cm, dùng cho túi áo",
                        MaNguyenLieu = "DAY001",
                        GiaNhap = 15000,
                        SoLuongTon = 50,
                        DonViTinh = "Cái",
                        ChatLieu = "Polyester",
                        NhaCungCapId = nhaCungCap3.Id,
                        LoaiNguyenLieuId = loaiPhuLieu.Id,
                        TrangThai = true
                    },
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Keo dán vải",
                        MoTa = "Keo dán vải chuyên dụng, dùng để dán viền cổ áo",
                        MaNguyenLieu = "KEO001",
                        GiaNhap = 35000,
                        SoLuongTon = 30,
                        DonViTinh = "Chai",
                        ChatLieu = "Acrylic",
                        NhaCungCapId = nhaCungCap3.Id,
                        LoaiNguyenLieuId = loaiPhuLieu.Id,
                        TrangThai = true
                    },
                    new NguyenLieu
                    {
                        TenNguyenLieu = "Vải viền cổ áo",
                        MoTa = "Vải viền cổ áo sơ mi, màu trắng, độ cứng vừa phải",
                        MaNguyenLieu = "VIEN001",
                        GiaNhap = 25000,
                        SoLuongTon = 100,
                        DonViTinh = "Mét",
                        ChatLieu = "Cotton + Polyester",
                        NhaCungCapId = nhaCungCap3.Id,
                        LoaiNguyenLieuId = loaiPhuLieu.Id,
                        TrangThai = true
                    }
                };

                foreach (var nl in nguyenLieus)
                {
                    if (!await _context.NguyenLieus.AnyAsync(x => x.MaNguyenLieu == nl.MaNguyenLieu))
                    {
                        _context.NguyenLieus.Add(nl);
                    }
                }

                await _context.SaveChangesAsync();

                // 4. Insert Sản phẩm mẫu
                var sanPhams = new List<SanPham>
                {
                    new SanPham
                    {
                        TenSanPham = "Áo sơ mi nam trắng",
                        MaSanPham = "SP001",
                        MoTa = "Áo sơ mi nam màu trắng, chất liệu cotton 100%, form regular fit",
                        GiaBan = 250000,
                        GiaNhap = 180000,
                        ChiPhiNhanCong = 50000,
                        SoLuongTon = 50,
                        KichThuoc = "M, L, XL",
                        MauSac = "#FFFFFF",
                        ChatLieu = "Cotton 100%",
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSanPham = "Áo sơ mi nữ trắng",
                        MaSanPham = "SP002",
                        MoTa = "Áo sơ mi nữ màu trắng, chất liệu cotton 100%, form slim fit",
                        GiaBan = 220000,
                        GiaNhap = 160000,
                        ChiPhiNhanCong = 45000,
                        SoLuongTon = 45,
                        KichThuoc = "S, M, L",
                        MauSac = "#FFFFFF",
                        ChatLieu = "Cotton 100%",
                        TrangThai = true
                    }
                };

                foreach (var sp in sanPhams)
                {
                    if (!await _context.SanPhams.AnyAsync(x => x.MaSanPham == sp.MaSanPham))
                    {
                        _context.SanPhams.Add(sp);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "Đã insert thành công dữ liệu mẫu bao gồm chi phí nhân công!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
} 