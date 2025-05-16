using System.Collections.Generic;

namespace Event_Management_System.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        
        public string FullName => $"{FirstName} {LastName}";
    }
} 