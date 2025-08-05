using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ThanhPhanCauHinh : BaseEntity
    {
        public int SoLuong { get; set; }
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        // Foreign keys
        public int SanPhamId { get; set; }
        public int NguyenLieuId { get; set; }
        
        // Navigation properties
        public virtual SanPham SanPham { get; set; } = null!;
        public virtual NguyenLieu NguyenLieu { get; set; } = null!;
    }
} 