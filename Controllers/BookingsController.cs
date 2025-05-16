using Event_Management_System.Models;
using Event_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_System.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IEventService _eventService;

        public BookingsController(IBookingService bookingService, IEventService eventService)
        {
            _bookingService = bookingService;
            _eventService = eventService;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Ensure the user can only see their own bookings
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            if (booking.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(booking);
        }

        // GET: Bookings/BookEvent/5
        public async Task<IActionResult> BookEvent(int id)
        {
            var @event = await _eventService.GetEventByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            // Check if user has already booked this event
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var isBooked = await _eventService.IsEventBookedByUserAsync(id, userId);
            if (isBooked)
            {
                // User has already booked this event
                return RedirectToAction("Details", "Events", new { id = id });
            }

            // Create booking
            var booking = new Booking
            {
                UserId = userId,
                EventId = id,
                TicketCount = 1
            };

            await _bookingService.CreateBookingAsync(booking);

            return RedirectToAction("Confirmation", new { id = booking.Id });
        }

        // GET: Bookings/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Ensure the user can only cancel their own bookings
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            if (booking.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            await _bookingService.CancelBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 