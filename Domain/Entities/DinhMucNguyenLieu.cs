using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DinhMucNguyenLieu : BaseEntity
    {
        [Required]
        public int SanPhamId { get; set; }
        
        [Required]
        public int NguyenLieuId { get; set; }
        
        [Required]
        public decimal SoLuongCan { get; set; }
        
        [StringLength(20)]
        public string? DonViTinh { get; set; }
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Navigation properties
        public virtual SanPham SanPham { get; set; } = null!;
        public virtual NguyenLieu NguyenLieu { get; set; } = null!;
    }
} 