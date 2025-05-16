using Event_Management_System.Data;
using Event_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace Event_Management_System.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync(int? page = null, int pageSize = 10)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .ThenInclude(et => et.Tag)
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.Date);
                
            if (page.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize).Take(pageSize);
            }
            
            return await query.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .ThenInclude(et => et.Tag)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .ThenInclude(et => et.Tag)
                .Where(e => e.CategoryId == categoryId && e.IsActive)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByTagAsync(int tagId)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .ThenInclude(et => et.Tag)
                .Where(e => e.EventTags.Any(et => et.TagId == tagId) && e.IsActive)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .ThenInclude(et => et.Tag)
                .Where(e => (e.Name.Contains(searchTerm) ||
                           e.Description.Contains(searchTerm) ||
                           e.Category.Name.Contains(searchTerm) ||
                           e.EventTags.Any(et => et.Tag.Name.Contains(searchTerm))) &&
                           e.IsActive)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<int> CreateEventAsync(Event @event, List<int> tagIds)
        {
            try
            {
                Console.WriteLine($"Creating event: {JsonSerializer.Serialize(@event)}");
                
                // Ensure required fields are set
                if (string.IsNullOrEmpty(@event.Name))
                {
                    throw new ArgumentException("Event name cannot be null or empty");
                }
                
                // Debug database connection
                Console.WriteLine("Database connection state: " + (_context.Database.GetConnectionString() != null ? "Connected" : "Not connected"));
                
                // Set default values for required fields if not provided
                @event.CreatedAt = DateTime.Now;
                @event.IsActive = true;
                
                // Debug entity state before adding
                Console.WriteLine($"Entity state before adding: {_context.Entry(@event).State}");
                
                _context.Events.Add(@event);
                
                // Debug entity state after adding
                Console.WriteLine($"Entity state after adding: {_context.Entry(@event).State}");
                
                // Save with explicit transaction
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await _context.SaveChangesAsync();
                        Console.WriteLine($"SaveChanges result: {result} records affected");
                        Console.WriteLine($"Event saved with ID: {@event.Id}");
                        
                        // Add tags to the event
                        if (tagIds != null && tagIds.Any())
                        {
                            Console.WriteLine($"Adding {tagIds.Count} tags to event");
                            foreach (var tagId in tagIds)
                            {
                                _context.EventTags.Add(new EventTag
                                {
                                    EventId = @event.Id,
                                    TagId = tagId
                                });
                            }
                            
                            var tagResult = await _context.SaveChangesAsync();
                            Console.WriteLine($"Tags SaveChanges result: {tagResult} records affected");
                        }
                        
                        await transaction.CommitAsync();
                        Console.WriteLine("Transaction committed successfully");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"Transaction rolled back due to error: {ex.Message}");
                        throw;
                    }
                }
                
                return @event.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateEventAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw to be handled by the controller
            }
        }

        public async Task UpdateEventAsync(Event @event, List<int> tagIds)
        {
            var existingEvent = await _context.Events
                .Include(e => e.EventTags)
                .FirstOrDefaultAsync(e => e.Id == @event.Id);
                
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {@event.Id} not found.");
            }
            
            // Update event properties
            existingEvent.Name = @event.Name;
            existingEvent.Description = @event.Description;
            existingEvent.Date = @event.Date;
            existingEvent.Venue = @event.Venue;
            existingEvent.Price = @event.Price;
            existingEvent.CategoryId = @event.CategoryId;
            existingEvent.IsActive = @event.IsActive;
            existingEvent.UpdatedAt = DateTime.Now;
            
            // Only update image path if a new one is provided
            if (!string.IsNullOrEmpty(@event.ImagePath))
            {
                existingEvent.ImagePath = @event.ImagePath;
            }
            
            // Remove existing tags
            _context.EventTags.RemoveRange(existingEvent.EventTags);
            
            // Add updated tags
            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    _context.EventTags.Add(new EventTag
                    {
                        EventId = @event.Id,
                        TagId = tagId
                    });
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }
            
            // Soft delete by marking it as inactive
            @event.IsActive = false;
            @event.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEventBookedByUserAsync(int eventId, string userId)
        {
            return await _context.Bookings
                .AnyAsync(b => b.EventId == eventId && 
                           b.UserId == userId && 
                           b.Status == "Confirmed");
        }

        public async Task<int> GetTotalEventsCountAsync()
        {
            return await _context.Events.CountAsync(e => e.IsActive);
        }
    }
} 