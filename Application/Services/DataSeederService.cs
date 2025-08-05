using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DataSeederService : IDataSeederService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INhaCungCapService _nhaCungCapService;
        private readonly ILoaiNguyenLieuService _loaiNguyenLieuService;
        private readonly INguyenLieuService _nguyenLieuService;
        private readonly ISanPhamService _sanPhamService;
        private readonly IQuyTrinhSanXuatService _quyTrinhSanXuatService;
        private readonly IDinhMucNguyenLieuService _dinhMucNguyenLieuService;

        public DataSeederService(
            IUnitOfWork unitOfWork,
            INhaCungCapService nhaCungCapService,
            ILoaiNguyenLieuService loaiNguyenLieuService,
            INguyenLieuService nguyenLieuService,
            ISanPhamService sanPhamService,
            IQuyTrinhSanXuatService quyTrinhSanXuatService,
            IDinhMucNguyenLieuService dinhMucNguyenLieuService)
        {
            _unitOfWork = unitOfWork;
            _nhaCungCapService = nhaCungCapService;
            _loaiNguyenLieuService = loaiNguyenLieuService;
            _nguyenLieuService = nguyenLieuService;
            _sanPhamService = sanPhamService;
            _quyTrinhSanXuatService = quyTrinhSanXuatService;
            _dinhMucNguyenLieuService = dinhMucNguyenLieuService;
        }

        public async Task SeedDataAsync()
        {
            // Xóa toàn bộ dữ liệu cũ trước khi insert mới
            await ClearAllDataAsync();

            // 1. Tạo Loại nguyên liệu

            // 1. Tạo Loại nguyên liệu
            var loaiVai = new LoaiNguyenLieu
            {
                TenLoai = "Vải",
                MoTa = "Các loại vải may mặc",
                TrangThai = true
            };
            await _loaiNguyenLieuService.CreateAsync(loaiVai);

            var loaiChi = new LoaiNguyenLieu
            {
                TenLoai = "Chỉ may",
                MoTa = "Các loại chỉ may",
                TrangThai = true
            };
            await _loaiNguyenLieuService.CreateAsync(loaiChi);

            var loaiCuc = new LoaiNguyenLieu
            {
                TenLoai = "Cúc áo",
                MoTa = "Các loại cúc áo",
                TrangThai = true
            };
            await _loaiNguyenLieuService.CreateAsync(loaiCuc);

            // 2. Tạo Nhà cung cấp
            var nccVai = new NhaCungCap
            {
                TenNhaCungCap = "Công ty TNHH Vải Việt Nam",
                DiaChi = "123 Đường ABC, Quận 1, TP.HCM",
                SoDienThoai = "+84 28 1234 5678",
                Email = "info@vaivietnam.com",
                MaSoThue = "0123456789",
                NguoiDaiDien = "Nguyễn Văn A",
                TrangThai = true
            };
            await _nhaCungCapService.CreateAsync(nccVai);

            var nccChi = new NhaCungCap
            {
                TenNhaCungCap = "Công ty CP Chỉ may Thành Công",
                DiaChi = "456 Đường XYZ, Quận 3, TP.HCM",
                SoDienThoai = "+84 28 9876 5432",
                Email = "contact@chithanhcong.com",
                MaSoThue = "9876543210",
                NguoiDaiDien = "Trần Thị B",
                TrangThai = true
            };
            await _nhaCungCapService.CreateAsync(nccChi);

            var nccCuc = new NhaCungCap
            {
                TenNhaCungCap = "Công ty TNHH Cúc áo Đẹp",
                DiaChi = "789 Đường DEF, Quận 7, TP.HCM",
                SoDienThoai = "+84 28 5555 6666",
                Email = "sales@cucdep.com",
                MaSoThue = "5555666677",
                NguoiDaiDien = "Lê Văn C",
                TrangThai = true
            };
            await _nhaCungCapService.CreateAsync(nccCuc);

            // 3. Tạo Nguyên liệu
            var vaiCotton = new NguyenLieu
            {
                TenNguyenLieu = "Vải cotton trắng",
                MaNguyenLieu = "VL001_DEMO",
                MoTa = "Vải cotton 100% màu trắng, chất lượng cao",
                GiaNhap = 45000,
                SoLuongTon = 100,
                DonViTinh = "mét",
                ChatLieu = "Cotton 100%",
                MauSac = "#FFFFFF",
                HinhAnh = "/uploads/materials/cotton-white.jpg",
                NhaCungCapId = nccVai.Id,
                LoaiNguyenLieuId = loaiVai.Id,
                TrangThai = true
            };
            await _nguyenLieuService.CreateAsync(vaiCotton);

            var chiTrang = new NguyenLieu
            {
                TenNguyenLieu = "Chỉ may trắng",
                MaNguyenLieu = "CM001_DEMO",
                MoTa = "Chỉ may cotton màu trắng, độ bền cao",
                GiaNhap = 15000,
                SoLuongTon = 50,
                DonViTinh = "cuộn",
                ChatLieu = "Cotton",
                MauSac = "#FFFFFF",
                HinhAnh = "/uploads/materials/thread-white.jpg",
                NhaCungCapId = nccChi.Id,
                LoaiNguyenLieuId = loaiChi.Id,
                TrangThai = true
            };
            await _nguyenLieuService.CreateAsync(chiTrang);

            var cucTrang = new NguyenLieu
            {
                TenNguyenLieu = "Cúc áo trắng",
                MaNguyenLieu = "CA001_DEMO",
                MoTa = "Cúc áo nhựa màu trắng, kích thước 1.5cm",
                GiaNhap = 5000,
                SoLuongTon = 200,
                DonViTinh = "cái",
                ChatLieu = "Nhựa",
                MauSac = "#FFFFFF",
                HinhAnh = "/uploads/materials/button-white.jpg",
                NhaCungCapId = nccCuc.Id,
                LoaiNguyenLieuId = loaiCuc.Id,
                TrangThai = true
            };
            await _nguyenLieuService.CreateAsync(cucTrang);

            // 4. Tạo Sản phẩm
            var aoSoMiTrang = new SanPham
            {
                TenSanPham = "Áo sơ mi trắng nam",
                MaSanPham = "SP001_DEMO",
                MoTa = "Áo sơ mi nam màu trắng, chất liệu cotton cao cấp",
                GiaBan = 250000,
                GiaNhap = 150000,
                SoLuongTon = 20,
                KichThuoc = "M, L, XL",
                ChatLieu = "Cotton 100%",
                MauSac = "#FFFFFF",
                HinhAnh = "/uploads/products/ao-so-mi-trang.jpg",
                TrangThai = true
            };
            await _sanPhamService.CreateAsync(aoSoMiTrang);

            // 5. Tạo Quy trình sản xuất cho áo sơ mi trắng
            var quyTrinh1 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Cắt vải",
                MoTa = "Cắt vải theo mẫu",
                ChiPhiNhanCong = 15000,
                ThuTu = 1,
                SanPhamId = aoSoMiTrang.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinh1);

            var quyTrinh2 = new QuyTrinhSanXuat
            {
                TenCongDoan = "May thân áo",
                MoTa = "May các phần thân áo",
                ChiPhiNhanCong = 25000,
                ThuTu = 2,
                SanPhamId = aoSoMiTrang.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinh2);

            var quyTrinh3 = new QuyTrinhSanXuat
            {
                TenCongDoan = "May tay áo",
                MoTa = "May và gắn tay áo",
                ChiPhiNhanCong = 20000,
                ThuTu = 3,
                SanPhamId = aoSoMiTrang.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinh3);

            var quyTrinh4 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Gắn cúc",
                MoTa = "Gắn cúc áo",
                ChiPhiNhanCong = 10000,
                ThuTu = 4,
                SanPhamId = aoSoMiTrang.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinh4);

            var quyTrinh5 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Hoàn thiện",
                MoTa = "Là ủi và kiểm tra chất lượng",
                ChiPhiNhanCong = 15000,
                ThuTu = 5,
                SanPhamId = aoSoMiTrang.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinh5);

            // 7. Tạo Định mức nguyên liệu cho áo sơ mi trắng
            var dinhMuc1 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiTrang.Id,
                NguyenLieuId = vaiCotton.Id,
                SoLuongCan = 2.5m, // 2.5 mét vải
                DonViTinh = "mét",
                GhiChu = "Vải cotton trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc1);

            var dinhMuc2 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiTrang.Id,
                NguyenLieuId = chiTrang.Id,
                SoLuongCan = 1m, // 1 cuộn chỉ
                DonViTinh = "cuộn",
                GhiChu = "Chỉ may trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc2);

            var dinhMuc3 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiTrang.Id,
                NguyenLieuId = cucTrang.Id,
                SoLuongCan = 6m, // 6 cúc áo
                DonViTinh = "cái",
                GhiChu = "Cúc áo trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc3);

            // 8. Tạo thêm một số sản phẩm khác
            var aoSoMiXanh = new SanPham
            {
                TenSanPham = "Áo sơ mi xanh nam",
                MaSanPham = "SP002_DEMO",
                MoTa = "Áo sơ mi nam màu xanh dương, chất liệu cotton",
                GiaBan = 280000,
                GiaNhap = 170000,
                SoLuongTon = 15,
                KichThuoc = "S, M, L, XL",
                ChatLieu = "Cotton 100%",
                MauSac = "#0066CC",
                HinhAnh = "/uploads/products/ao-so-mi-xanh.jpg",
                TrangThai = true
            };
            await _sanPhamService.CreateAsync(aoSoMiXanh);

            var aoSoMiDen = new SanPham
            {
                TenSanPham = "Áo sơ mi đen nam",
                MaSanPham = "SP003_DEMO",
                MoTa = "Áo sơ mi nam màu đen, phong cách lịch lãm",
                GiaBan = 300000,
                GiaNhap = 180000,
                SoLuongTon = 10,
                KichThuoc = "M, L, XL, XXL",
                ChatLieu = "Cotton 100%",
                MauSac = "#000000",
                HinhAnh = "/uploads/products/ao-so-mi-den.jpg",
                TrangThai = true
            };
            await _sanPhamService.CreateAsync(aoSoMiDen);

            // 5. Tạo Quy trình sản xuất cho áo sơ mi xanh
            var quyTrinhXanh1 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Cắt vải xanh",
                MoTa = "Cắt vải cotton xanh theo mẫu",
                ChiPhiNhanCong = 15000,
                ThuTu = 1,
                SanPhamId = aoSoMiXanh.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhXanh1);

            var quyTrinhXanh2 = new QuyTrinhSanXuat
            {
                TenCongDoan = "May thân áo xanh",
                MoTa = "May các phần thân áo xanh",
                ChiPhiNhanCong = 25000,
                ThuTu = 2,
                SanPhamId = aoSoMiXanh.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhXanh2);

            var quyTrinhXanh3 = new QuyTrinhSanXuat
            {
                TenCongDoan = "May tay áo xanh",
                MoTa = "May và gắn tay áo xanh",
                ChiPhiNhanCong = 20000,
                ThuTu = 3,
                SanPhamId = aoSoMiXanh.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhXanh3);

            var quyTrinhXanh4 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Gắn cúc áo xanh",
                MoTa = "Gắn cúc áo cho áo xanh",
                ChiPhiNhanCong = 10000,
                ThuTu = 4,
                SanPhamId = aoSoMiXanh.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhXanh4);

            var quyTrinhXanh5 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Hoàn thiện áo xanh",
                MoTa = "Là ủi và kiểm tra chất lượng áo xanh",
                ChiPhiNhanCong = 15000,
                ThuTu = 5,
                SanPhamId = aoSoMiXanh.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhXanh5);

            // 6. Tạo Quy trình sản xuất cho áo sơ mi đen
            var quyTrinhDen1 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Cắt vải đen",
                MoTa = "Cắt vải cotton đen theo mẫu",
                ChiPhiNhanCong = 15000,
                ThuTu = 1,
                SanPhamId = aoSoMiDen.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhDen1);

            var quyTrinhDen2 = new QuyTrinhSanXuat
            {
                TenCongDoan = "May thân áo đen",
                MoTa = "May các phần thân áo đen",
                ChiPhiNhanCong = 25000,
                ThuTu = 2,
                SanPhamId = aoSoMiDen.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhDen2);

            var quyTrinhDen3 = new QuyTrinhSanXuat
            {
                TenCongDoan = "May tay áo đen",
                MoTa = "May và gắn tay áo đen",
                ChiPhiNhanCong = 20000,
                ThuTu = 3,
                SanPhamId = aoSoMiDen.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhDen3);

            var quyTrinhDen4 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Gắn cúc áo đen",
                MoTa = "Gắn cúc áo cho áo đen",
                ChiPhiNhanCong = 10000,
                ThuTu = 4,
                SanPhamId = aoSoMiDen.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhDen4);

            var quyTrinhDen5 = new QuyTrinhSanXuat
            {
                TenCongDoan = "Hoàn thiện áo đen",
                MoTa = "Là ủi và kiểm tra chất lượng áo đen",
                ChiPhiNhanCong = 15000,
                ThuTu = 5,
                SanPhamId = aoSoMiDen.Id,
                TrangThai = true
            };
            await _quyTrinhSanXuatService.CreateAsync(quyTrinhDen5);

            // 9. Tạo thêm nguyên liệu
            var vaiXanh = new NguyenLieu
            {
                TenNguyenLieu = "Vải cotton xanh",
                MaNguyenLieu = "VL002_DEMO",
                MoTa = "Vải cotton 100% màu xanh dương",
                GiaNhap = 48000,
                SoLuongTon = 80,
                DonViTinh = "mét",
                ChatLieu = "Cotton 100%",
                MauSac = "#0066CC",
                HinhAnh = "/uploads/materials/cotton-blue.jpg",
                NhaCungCapId = nccVai.Id,
                LoaiNguyenLieuId = loaiVai.Id,
                TrangThai = true
            };
            await _nguyenLieuService.CreateAsync(vaiXanh);

            var vaiDen = new NguyenLieu
            {
                TenNguyenLieu = "Vải cotton đen",
                MaNguyenLieu = "VL003_DEMO",
                MoTa = "Vải cotton 100% màu đen",
                GiaNhap = 52000,
                SoLuongTon = 60,
                DonViTinh = "mét",
                ChatLieu = "Cotton 100%",
                MauSac = "#000000",
                HinhAnh = "/uploads/materials/cotton-black.jpg",
                NhaCungCapId = nccVai.Id,
                LoaiNguyenLieuId = loaiVai.Id,
                TrangThai = true
            };
            await _nguyenLieuService.CreateAsync(vaiDen);

            // 10. Tạo định mức nguyên liệu cho áo sơ mi xanh
            var dinhMuc4 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiXanh.Id,
                NguyenLieuId = vaiXanh.Id,
                SoLuongCan = 2.5m,
                DonViTinh = "mét",
                GhiChu = "Vải cotton xanh cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc4);

            var dinhMuc5 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiXanh.Id,
                NguyenLieuId = chiTrang.Id,
                SoLuongCan = 1m,
                DonViTinh = "cuộn",
                GhiChu = "Chỉ may trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc5);

            var dinhMuc6 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiXanh.Id,
                NguyenLieuId = cucTrang.Id,
                SoLuongCan = 6m,
                DonViTinh = "cái",
                GhiChu = "Cúc áo trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc6);

            // 11. Tạo định mức nguyên liệu cho áo sơ mi đen
            var dinhMuc7 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiDen.Id,
                NguyenLieuId = vaiDen.Id,
                SoLuongCan = 2.5m,
                DonViTinh = "mét",
                GhiChu = "Vải cotton đen cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc7);

            var dinhMuc8 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiDen.Id,
                NguyenLieuId = chiTrang.Id,
                SoLuongCan = 1m,
                DonViTinh = "cuộn",
                GhiChu = "Chỉ may trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc8);

            var dinhMuc9 = new DinhMucNguyenLieu
            {
                SanPhamId = aoSoMiDen.Id,
                NguyenLieuId = cucTrang.Id,
                SoLuongCan = 6m,
                DonViTinh = "cái",
                GhiChu = "Cúc áo trắng cho 1 chiếc áo sơ mi",
                TrangThai = true
            };
            await _dinhMucNguyenLieuService.CreateAsync(dinhMuc9);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ClearAllDataAsync()
        {
            // Xóa theo thứ tự để tránh lỗi foreign key constraint
            // 1. Xóa Định mức nguyên liệu
            var allDinhMucs = await _dinhMucNguyenLieuService.GetAllAsync();
            foreach (var dinhMuc in allDinhMucs)
            {
                await _dinhMucNguyenLieuService.DeleteAsync(dinhMuc.Id);
            }

            // 2. Xóa Quy trình sản xuất
            var allQuyTrinhs = await _quyTrinhSanXuatService.GetAllAsync();
            foreach (var quyTrinh in allQuyTrinhs)
            {
                await _quyTrinhSanXuatService.DeleteAsync(quyTrinh.Id);
            }

            // 3. Xóa Sản phẩm
            var allSanPhams = await _sanPhamService.GetAllAsync();
            foreach (var sanPham in allSanPhams)
            {
                await _sanPhamService.DeleteAsync(sanPham.Id);
            }

            // 4. Xóa Nguyên liệu
            var allNguyenLieus = await _nguyenLieuService.GetAllAsync();
            foreach (var nguyenLieu in allNguyenLieus)
            {
                await _nguyenLieuService.DeleteAsync(nguyenLieu.Id);
            }

            // 5. Xóa Nhà cung cấp
            var allNhaCungCaps = await _nhaCungCapService.GetAllAsync();
            foreach (var nhaCungCap in allNhaCungCaps)
            {
                await _nhaCungCapService.DeleteAsync(nhaCungCap.Id);
            }

            // 6. Xóa Loại nguyên liệu
            var allLoaiNguyenLieus = await _loaiNguyenLieuService.GetAllAsync();
            foreach (var loaiNguyenLieu in allLoaiNguyenLieus)
            {
                await _loaiNguyenLieuService.DeleteAsync(loaiNguyenLieu.Id);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
} 