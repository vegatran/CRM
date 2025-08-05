using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class KhachHang : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenKhachHang { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? DiaChi { get; set; }
        
        [StringLength(20)]
        public string? SoDienThoai { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(50)]
        public string? MaKhachHang { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Foreign keys
        public int KhuVucId { get; set; }
        
        // Navigation properties
        public virtual KhuVuc KhuVuc { get; set; } = null!;
        public virtual ICollection<PhieuBanHang> PhieuBanHangs { get; set; } = new List<PhieuBanHang>();
    }
} 