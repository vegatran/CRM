using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PhieuNhapKho : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string SoPhieu { get; set; } = string.Empty;
        
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        
        [StringLength(500)]
        public string? GhiChu { get; set; }
        
        public decimal TongTien { get; set; }
        
        public string TrangThai { get; set; } = "Chờ xử lý"; // Chờ xử lý, Đã xử lý, Đã hủy
        
        // Foreign keys
        public int NhaCungCapId { get; set; }
        
        // Navigation properties
        public virtual NhaCungCap NhaCungCap { get; set; } = null!;
        public virtual ICollection<ChiTietNhapKho> ChiTietNhapKhos { get; set; } = new List<ChiTietNhapKho>();
    }
} 