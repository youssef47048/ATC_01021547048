namespace Event_Management_System.Models
{
    public class EventTag
    {
        // Composite key is configured in DbContext
        public int EventId { get; set; }
        public int TagId { get; set; }
        
        // Navigation properties
        public virtual Event Event { get; set; }
        public virtual Tag Tag { get; set; }
    }
} 