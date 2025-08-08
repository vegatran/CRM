-- Insert sample data for CRM May Mặc
-- Nhà cung cấp và Nguyên liệu để sản xuất áo sơ mi trắng

-- 1. Insert Nhà cung cấp
INSERT INTO NhaCungCaps (TenNhaCungCap, DiaChi, SoDienThoai, Email, MaSoThue, NguoiDaiDien, TrangThai, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy, IsDeleted) VALUES
('Công ty TNHH Vải Việt Nam', '123 Đường Nguyễn Văn Linh, Quận 7, TP.HCM', '+84 28 1234 5678', 'info@vaivietnam.com', '0123456789', 'Nguyễn Văn An', 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Công ty CP Chỉ May Thành Công', '456 Đường Lê Văn Việt, Quận 9, TP.HCM', '+84 28 2345 6789', 'contact@chithanhcong.com', '0234567890', 'Trần Thị Bình', 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Công ty TNHH Phụ Liệu May Mặc', '789 Đường Mai Chí Thọ, Quận 2, TP.HCM', '+84 28 3456 7890', 'sales@phulieu.com', '0345678901', 'Lê Văn Cường', 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Công ty CP Khuy Cúc Việt Nam', '321 Đường Võ Văn Ngân, Thủ Đức, TP.HCM', '+84 28 4567 8901', 'info@khuycuc.com', '0456789012', 'Phạm Thị Dung', 1, GETDATE(), GETDATE(), 'System', 'System', 0);

-- 2. Insert LoaiNguyenLieu (nếu chưa có)
IF NOT EXISTS (SELECT * FROM LoaiNguyenLieus WHERE Id = 1)
BEGIN
    INSERT INTO LoaiNguyenLieus (TenLoaiNguyenLieu, MoTa, TrangThai, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy, IsDeleted) VALUES
    ('Vải', 'Các loại vải dùng để may quần áo', 1, GETDATE(), GETDATE(), 'System', 'System', 0),
    ('Chỉ may', 'Các loại chỉ dùng để may', 1, GETDATE(), GETDATE(), 'System', 'System', 0),
    ('Khuy', 'Các loại khuy, cúc, nút', 1, GETDATE(), GETDATE(), 'System', 'System', 0),
    ('Phụ liệu khác', 'Các phụ liệu khác dùng trong may mặc', 1, GETDATE(), GETDATE(), 'System', 'System', 0);
END

-- 3. Insert Nguyên liệu để sản xuất áo sơ mi trắng
INSERT INTO NguyenLieus (TenNguyenLieu, MoTa, MaNguyenLieu, GiaNhap, SoLuongTon, DonViTinh, ChatLieu, NhaCungCapId, LoaiNguyenLieuId, TrangThai, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy, IsDeleted) VALUES
-- Vải chính cho áo sơ mi
('Vải Cotton 100% Trắng', 'Vải cotton 100% màu trắng, độ dày vừa phải, phù hợp may áo sơ mi', 'VAI001', 85000, 500, 'Mét', 'Cotton 100%', 1, 1, 1, GETDATE(), GETDATE(), 'System', 'System', 0),

-- Vải lót
('Vải lót Polyester Trắng', 'Vải lót polyester màu trắng, mềm mại, dùng lót cổ áo và viền', 'VAI002', 45000, 300, 'Mét', 'Polyester', 1, 1, 1, GETDATE(), GETDATE(), 'System', 'System', 0),

-- Chỉ may
('Chỉ may Polyester Trắng', 'Chỉ may polyester màu trắng, độ bền cao, phù hợp may áo sơ mi', 'CHI001', 25000, 100, 'Cuộn', 'Polyester', 2, 2, 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Chỉ may Cotton Trắng', 'Chỉ may cotton màu trắng, mềm mại, dùng may đường viền', 'CHI002', 30000, 80, 'Cuộn', 'Cotton', 2, 2, 1, GETDATE(), GETDATE(), 'System', 'System', 0),

-- Khuy cúc
('Khuy nhựa Trắng 15mm', 'Khuy nhựa màu trắng đường kính 15mm, dùng cho áo sơ mi', 'KHUY001', 5000, 200, 'Cái', 'Nhựa', 4, 3, 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Khuy nhựa Trắng 12mm', 'Khuy nhựa màu trắng đường kính 12mm, dùng cho túi áo', 'KHUY002', 4000, 150, 'Cái', 'Nhựa', 4, 3, 1, GETDATE(), GETDATE(), 'System', 'System', 0),

-- Phụ liệu khác
('Dây kéo Polyester Trắng 20cm', 'Dây kéo polyester màu trắng dài 20cm, dùng cho túi áo', 'DAY001', 15000, 50, 'Cái', 'Polyester', 3, 4, 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Keo dán vải', 'Keo dán vải chuyên dụng, dùng để dán viền cổ áo', 'KEO001', 35000, 30, 'Chai', 'Acrylic', 3, 4, 1, GETDATE(), GETDATE(), 'System', 'System', 0),
('Vải viền cổ áo', 'Vải viền cổ áo sơ mi, màu trắng, độ cứng vừa phải', 'VIEN001', 25000, 100, 'Mét', 'Cotton + Polyester', 3, 4, 1, GETDATE(), GETDATE(), 'System', 'System', 0);

-- 4. Insert Sản phẩm mẫu (Áo sơ mi trắng)
INSERT INTO SanPhams (TenSanPham, MaSanPham, MoTa, GiaBan, GiaNhap, ChiPhiNhanCong, SoLuongTon, KichThuoc, MauSac, ChatLieu, TrangThai, HinhAnh, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy, IsDeleted) VALUES
('Áo sơ mi nam trắng', 'SP001', 'Áo sơ mi nam màu trắng, chất liệu cotton 100%, form regular fit', 250000, 180000, 50000, 50, 'M, L, XL', '#FFFFFF', 'Cotton 100%', 1, NULL, GETDATE(), GETDATE(), 'System', 'System', 0),
('Áo sơ mi nữ trắng', 'SP002', 'Áo sơ mi nữ màu trắng, chất liệu cotton 100%, form slim fit', 220000, 160000, 45000, 45, 'S, M, L', '#FFFFFF', 'Cotton 100%', 1, NULL, GETDATE(), GETDATE(), 'System', 'System', 0);

PRINT 'Đã insert thành công dữ liệu mẫu cho CRM May Mặc!';
PRINT 'Bao gồm:';
PRINT '- 4 nhà cung cấp';
PRINT '- 9 loại nguyên liệu để sản xuất áo sơ mi trắng';
PRINT '- 2 sản phẩm mẫu (áo sơ mi nam và nữ) với chi phí nhân công'; 