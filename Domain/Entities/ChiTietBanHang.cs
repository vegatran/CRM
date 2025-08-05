using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ChiTietBanHang : BaseEntity
    {
        public int SoLuong { get; set; }
        
        public decimal DonGia { get; set; }
        
        public decimal ThanhTien { get; set; }
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        // Foreign keys
        public int PhieuBanHangId { get; set; }
        public int SanPhamId { get; set; }
        
        // Navigation properties
        public virtual PhieuBanHang PhieuBanHang { get; set; } = null!;
        public virtual SanPham SanPham { get; set; } = null!;
    }
} 