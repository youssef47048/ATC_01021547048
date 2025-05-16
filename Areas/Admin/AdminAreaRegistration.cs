using Microsoft.AspNetCore.Mvc;

namespace Event_Management_System.Areas.Admin
{
    [Area("Admin")]
    public class AdminAreaRegistration : AreaAttribute
    {
        public AdminAreaRegistration() : base("Admin")
        {
        }
    }
} 