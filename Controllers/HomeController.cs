using Event_Management_System.Models;
using Event_Management_System.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Event_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IEventService eventService, ILogger<HomeController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 9; // Show 9 events per page
            var events = await _eventService.GetAllEventsAsync(page, pageSize);
            var totalEvents = await _eventService.GetTotalEventsCountAsync();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalEvents / pageSize);
            
            return View(events);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 