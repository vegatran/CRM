using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class NguyenLieu : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenNguyenLieu { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        [Required]
        [StringLength(50)]
        public string MaNguyenLieu { get; set; } = string.Empty;
        
        public decimal GiaNhap { get; set; }
        
        public int SoLuongTon { get; set; } = 0;
        
        [StringLength(20)]
        public string? DonViTinh { get; set; }
        
        [StringLength(100)]
        public string? ChatLieu { get; set; }
        
        [StringLength(50)]
        public string? MauSac { get; set; }
        
        [StringLength(500)]
        public string? HinhAnh { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Foreign keys
        public int NhaCungCapId { get; set; }
        public int LoaiNguyenLieuId { get; set; }
        
        // Navigation properties
        public virtual NhaCungCap NhaCungCap { get; set; } = null!;
        public virtual LoaiNguyenLieu LoaiNguyenLieu { get; set; } = null!;
        public virtual ICollection<ThanhPhanCauHinh> ThanhPhanCauHinhs { get; set; } = new List<ThanhPhanCauHinh>();
        public virtual ICollection<ChiTietNhapKho> ChiTietNhapKhos { get; set; } = new List<ChiTietNhapKho>();
    }
} 