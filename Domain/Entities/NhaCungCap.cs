using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class NhaCungCap : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenNhaCungCap { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? DiaChi { get; set; }
        
        [StringLength(20)]
        public string? SoDienThoai { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(50)]
        public string? MaSoThue { get; set; }
        
        [StringLength(100)]
        public string? NguoiDaiDien { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<NguyenLieu> NguyenLieus { get; set; } = new List<NguyenLieu>();
        public virtual ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();
    }
} 