using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Event_Management_System.Models;

namespace Event_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventTag> EventTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure many-to-many relationship between Event and Tag
            builder.Entity<EventTag>()
                .HasKey(et => new { et.EventId, et.TagId });

            builder.Entity<EventTag>()
                .HasOne(et => et.Event)
                .WithMany(e => e.EventTags)
                .HasForeignKey(et => et.EventId);

            builder.Entity<EventTag>()
                .HasOne(et => et.Tag)
                .WithMany(t => t.EventTags)
                .HasForeignKey(et => et.TagId);
                
            // Configure one-to-many relationship between Category and Event
            builder.Entity<Category>()
                .HasMany(c => c.Events)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId);
                
            // Configure one-to-many relationship between ApplicationUser and Booking
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);
                
            // Configure one-to-many relationship between Event and Booking
            builder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId);
        }
    }
} 