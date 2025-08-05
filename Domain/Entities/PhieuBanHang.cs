using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PhieuBanHang : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string SoPhieu { get; set; } = string.Empty;
        
        public DateTime NgayBan { get; set; } = DateTime.Now;
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        public decimal TongTien { get; set; }
        
        public decimal GiamGia { get; set; } = 0;
        
        public decimal ThanhToan { get; set; }
        
        public string TrangThai { get; set; } = "Chờ xử lý"; // Chờ xử lý, Đã xử lý, Đã hủy
        
        // Foreign keys
        public int KhachHangId { get; set; }
        
        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual ICollection<ChiTietBanHang> ChiTietBanHangs { get; set; } = new List<ChiTietBanHang>();
    }
} 