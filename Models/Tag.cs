using System.ComponentModel.DataAnnotations;

namespace Event_Management_System.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        // Navigation property
        public virtual ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
    }
} 