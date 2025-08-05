using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class NhaCungCapDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên nhà cung cấp không được vượt quá 100 ký tự")]
        public string TenNhaCungCap { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string? DiaChi { get; set; }
        
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string? SoDienThoai { get; set; }
        
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
        
        [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
        public string? MaSoThue { get; set; }
        
        [StringLength(100, ErrorMessage = "Người đại diện không được vượt quá 100 ký tự")]
        public string? NguoiDaiDien { get; set; }
        
        public bool TrangThai { get; set; } = true;
    }
} 