using Event_Management_System.Data;
using Event_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateBookingAsync(Booking booking)
        {
            // Calculate total amount
            var @event = await _context.Events.FindAsync(booking.EventId);
            if (@event == null)
            {
                throw new KeyNotFoundException($"Event with ID {booking.EventId} not found.");
            }
            
            booking.TotalAmount = @event.Price * booking.TicketCount;
            booking.Status = "Confirmed";
            booking.BookingDate = DateTime.Now;
            
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            
            return booking.Id;
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .ThenInclude(e => e.Category)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .ThenInclude(e => e.Category)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Where(b => b.EventId == eventId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.IsCancelled = true;
                booking.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync(int page, int pageSize)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.User)
                .OrderByDescending(b => b.BookingDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalBookingsCountAsync()
        {
            return await _context.Bookings.CountAsync();
        }
    }
} 