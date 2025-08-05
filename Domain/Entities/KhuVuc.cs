using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class KhuVuc : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenKhuVuc { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
    }
} 