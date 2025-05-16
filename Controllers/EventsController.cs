using Event_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Event_Management_System.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var @event = await _eventService.GetEventByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            // Check if user has already booked this event
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                ViewBag.IsBooked = await _eventService.IsEventBookedByUserAsync(id, userId);
            }
            else
            {
                ViewBag.IsBooked = false;
            }

            return View(@event);
        }

        // GET: Events/Category/5
        public async Task<IActionResult> Category(int id, int page = 1)
        {
            const int pageSize = 9;
            var events = await _eventService.GetEventsByCategoryAsync(id);
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)events.Count() / pageSize);
            ViewBag.CategoryId = id;
            
            return View(events.Skip((page - 1) * pageSize).Take(pageSize));
        }

        // GET: Events/Tag/5
        public async Task<IActionResult> Tag(int id, int page = 1)
        {
            const int pageSize = 9;
            var events = await _eventService.GetEventsByTagAsync(id);
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)events.Count() / pageSize);
            ViewBag.TagId = id;
            
            return View(events.Skip((page - 1) * pageSize).Take(pageSize));
        }

        // POST: Events/Search
        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm, int page = 1)
        {
            const int pageSize = 9;
            var events = await _eventService.SearchEventsAsync(searchTerm);
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)events.Count() / pageSize);
            ViewBag.SearchTerm = searchTerm;
            
            return View(events.Skip((page - 1) * pageSize).Take(pageSize));
        }
    }
} 