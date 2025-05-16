using Microsoft.AspNetCore.Identity;

namespace Event_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
} 