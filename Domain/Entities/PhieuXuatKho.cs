using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PhieuXuatKho : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string SoPhieu { get; set; } = string.Empty;

        [Required]
        public DateTime NgayXuat { get; set; }

        public int? KhachHangId { get; set; }
        public virtual KhachHang? KhachHang { get; set; }

        [StringLength(500)]
        public string? GhiChu { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } = "Chờ xử lý";

        public decimal TongTien { get; set; }

        public virtual ICollection<ChiTietXuatKho> ChiTietXuatKhos { get; set; } = new List<ChiTietXuatKho>();
    }
}
