using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class QuyTrinhSanXuat : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenCongDoan { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        public decimal ChiPhiNhanCong { get; set; } = 0;
        
        public int ThuTu { get; set; } = 1; // Thứ tự công đoạn
        
        public bool TrangThai { get; set; } = true;
        
        // Foreign key
        public int SanPhamId { get; set; }
        
        // Navigation property
        public virtual SanPham SanPham { get; set; } = null!;
    }
} 