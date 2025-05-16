using Event_Management_System.Models;

namespace Event_Management_System.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(int? page = null, int pageSize = 10);
        Task<Event> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId);
        Task<IEnumerable<Event>> GetEventsByTagAsync(int tagId);
        Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm);
        Task<int> CreateEventAsync(Event @event, List<int> tagIds);
        Task UpdateEventAsync(Event @event, List<int> tagIds);
        Task DeleteEventAsync(int id);
        Task<bool> IsEventBookedByUserAsync(int eventId, string userId);
        Task<int> GetTotalEventsCountAsync();
    }
} 