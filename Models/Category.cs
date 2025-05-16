using System.ComponentModel.DataAnnotations;

namespace Event_Management_System.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        // Navigation property
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
} 