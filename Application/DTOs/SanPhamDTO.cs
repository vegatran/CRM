using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class SanPhamDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string TenSanPham { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? MoTa { get; set; }
        
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string MaSanPham { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Giá bán là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0")]
        public decimal GiaBan { get; set; }
        
        [Required(ErrorMessage = "Giá nhập là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá nhập phải lớn hơn 0")]
        public decimal GiaNhap { get; set; }
        
        public int SoLuongTon { get; set; } = 0;
        
        [StringLength(20)]
        public string? KichThuoc { get; set; }
        
        [StringLength(50)]
        public string? MauSac { get; set; }
        
        [StringLength(100)]
        public string? ChatLieu { get; set; }
        
        public bool TrangThai { get; set; } = true;
    }
} 