using Event_Management_System.Models;

namespace Event_Management_System.Services
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
        Task<Booking> GetBookingByIdAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByEventIdAsync(int eventId);
        Task CancelBookingAsync(int id);
        Task<IEnumerable<Booking>> GetAllBookingsAsync(int page, int pageSize);
        Task<int> GetTotalBookingsCountAsync();
    }
} 