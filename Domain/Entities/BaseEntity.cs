using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedDate { get; set; }
        
        public string CreatedBy { get; set; } = string.Empty;
        
        public string? UpdatedBy { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
} 