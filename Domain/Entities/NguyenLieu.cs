using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class NguyenLieu : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        [Required]
        [StringLength(50)]
        public string MaNguyenLieu { get; set; } = string.Empty;
        
        public decimal GiaNhap { get; set; }
        
        public int SoLuongTon { get; set; } = 0;
        
        [StringLength(20)]
        public string? DonViTinh { get; set; }
        
        [StringLength(100)]
        public string? ChatLieu { get; set; }
        
        [StringLength(50)]
        public string? MauSac { get; set; }
        
        [StringLength(500)]
        public string? HinhAnh { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Foreign keys
        [Required(ErrorMessage = "Nhà cung cấp là bắt buộc")]
        public int NhaCungCapId { get; set; }
        
        [Required(ErrorMessage = "Loại nguyên liệu là bắt buộc")]
        public int LoaiNguyenLieuId { get; set; }
        
        // Navigation properties
        public virtual NhaCungCap? NhaCungCap { get; set; }
        public virtual LoaiNguyenLieu? LoaiNguyenLieu { get; set; }
        public virtual ICollection<ThanhPhanCauHinh> ThanhPhanCauHinhs { get; set; } = new List<ThanhPhanCauHinh>();
        public virtual ICollection<ChiTietNhapKho> ChiTietNhapKhos { get; set; } = new List<ChiTietNhapKho>();
        
        // Computed property - Số lượng tồn kho thực tế (tính từ lịch sử nhập/xuất)
        public int SoLuongTonThucTe
        {
            get
            {
                if (ChiTietNhapKhos == null) return SoLuongTon;
                
                var tongNhap = ChiTietNhapKhos
                    .Where(ct => ct.PhieuNhapKho != null && ct.PhieuNhapKho.TrangThai == "Đã xử lý")
                    .Sum(ct => ct.SoLuong);
                
                // TODO: Trừ đi số lượng đã xuất (cần thêm ChiTietXuatKho)
                // var tongXuat = ChiTietXuatKhos?.Sum(ct => ct.SoLuong) ?? 0;
                
                return tongNhap; // Tạm thời chỉ tính từ nhập kho
            }
        }
        
        // Computed property - Giá nhập trung bình
        public decimal GiaNhapTrungBinh
        {
            get
            {
                if (ChiTietNhapKhos == null || !ChiTietNhapKhos.Any())
                    return GiaNhap;
                
                var nhapKhos = ChiTietNhapKhos
                    .Where(ct => ct.PhieuNhapKho != null && ct.PhieuNhapKho.TrangThai == "Đã xử lý")
                    .ToList();
                
                if (!nhapKhos.Any()) return GiaNhap;
                
                var tongTien = nhapKhos.Sum(ct => ct.ThanhTien);
                var tongSoLuong = nhapKhos.Sum(ct => ct.SoLuong);
                
                return tongSoLuong > 0 ? tongTien / tongSoLuong : GiaNhap;
            }
        }
    }
} 