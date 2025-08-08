using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ChiTietXuatKho : BaseEntity
    {
        public int SoLuong { get; set; }
        
        public decimal DonGia { get; set; }
        
        public decimal ThanhTien { get; set; }
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        // Foreign keys
        public int PhieuXuatKhoId { get; set; }
        public int? SanPhamId { get; set; }
        public int? NguyenLieuId { get; set; }
        
        // Navigation properties
        public virtual PhieuXuatKho PhieuXuatKho { get; set; } = null!;
        public virtual SanPham? SanPham { get; set; }
        public virtual NguyenLieu? NguyenLieu { get; set; }
    }
}
