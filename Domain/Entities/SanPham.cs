using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SanPham : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenSanPham { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        [Required]
        [StringLength(50)]
        public string MaSanPham { get; set; } = string.Empty;
        
        public decimal GiaBan { get; set; }
        
        public decimal GiaNhap { get; set; }
        
        public int SoLuongTon { get; set; } = 0;
        
        [StringLength(20)]
        public string? KichThuoc { get; set; }
        
        [StringLength(50)]
        public string? MauSac { get; set; }
        
        [StringLength(100)]
        public string? ChatLieu { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<ThanhPhanCauHinh> ThanhPhanCauHinhs { get; set; } = new List<ThanhPhanCauHinh>();
        public virtual ICollection<ChiTietBanHang> ChiTietBanHangs { get; set; } = new List<ChiTietBanHang>();
        public virtual ICollection<ChiTietNhapKho> ChiTietNhapKhos { get; set; } = new List<ChiTietNhapKho>();
    }
} 