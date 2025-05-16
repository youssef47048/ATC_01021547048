using Event_Management_System.Models;
using Event_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IEventService _eventService;

        public BookingsController(IBookingService bookingService, IEventService eventService)
        {
            _bookingService = bookingService;
            _eventService = eventService;
        }

        // GET: Admin/Bookings
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var bookings = await _bookingService.GetAllBookingsAsync(page, pageSize);
            var totalBookings = await _bookingService.GetTotalBookingsCountAsync();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBookings / pageSize);
            
            return View(bookings);
        }

        // GET: Admin/Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Admin/Bookings/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            await _bookingService.CancelBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Bookings/ByEvent/5
        public async Task<IActionResult> ByEvent(int id)
        {
            var @event = await _eventService.GetEventByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var bookings = await _bookingService.GetBookingsByEventIdAsync(id);
            
            ViewBag.Event = @event;
            return View(bookings);
        }
        
        // GET: Admin/Bookings/ByUser/userId
        public async Task<IActionResult> ByUser(string id)
        {
            var bookings = await _bookingService.GetUserBookingsAsync(id);
            
            if (bookings == null || !bookings.Any())
            {
                ViewBag.UserEmail = "Unknown User";
            }
            else
            {
                ViewBag.UserEmail = bookings.First().User?.Email ?? "Unknown User";
            }
            
            return View(bookings);
        }
    }
} 