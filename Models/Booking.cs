using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management_System.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;
        
        [Required]
        public int TicketCount { get; set; } = 1;
        
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, etc.
        
        public bool IsCancelled { get; set; } = false;
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Event Event { get; set; }
    }
} 