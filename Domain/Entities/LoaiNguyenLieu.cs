using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class LoaiNguyenLieu : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        public bool TrangThai { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<NguyenLieu> NguyenLieus { get; set; } = new List<NguyenLieu>();
    }
} 