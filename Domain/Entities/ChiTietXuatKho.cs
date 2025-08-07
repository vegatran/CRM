using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ChiTietXuatKho : BaseEntity
    {
        public int PhieuXuatKhoId { get; set; }
        public virtual PhieuXuatKho PhieuXuatKho { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string LoaiHang { get; set; } = string.Empty; // "SanPham" hoặc "NguyenLieu"

        public int HangHoaId { get; set; }

        public int? SanPhamId { get; set; }
        public virtual SanPham? SanPham { get; set; }

        public int? NguyenLieuId { get; set; }
        public virtual NguyenLieu? NguyenLieu { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal DonGia { get; set; }

        public decimal ThanhTien => SoLuong * DonGia;
    }
}
