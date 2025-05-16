using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Event_Management_System.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Venue { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public string? ImagePath { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // For file upload (not stored in database)
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        
        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
} 