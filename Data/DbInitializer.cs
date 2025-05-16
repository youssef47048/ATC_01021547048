using Event_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Event_Management_System.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            
            // Get admin user details from configuration
            var adminEmail = configuration["AdminUser:Email"] ?? "admin@events.com";
            var adminPassword = configuration["AdminUser:Password"] ?? "Admin123!";
            var adminFirstName = configuration["AdminUser:FirstName"] ?? "Admin";
            var adminLastName = configuration["AdminUser:LastName"] ?? "User";
            
            Console.WriteLine($"Attempting to create admin user with email: {adminEmail}");
            
            // Check if admin exists
            var adminExists = await userManager.FindByEmailAsync(adminEmail);
            if (adminExists == null)
            {
                // Create admin user
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                    EmailConfirmed = true
                };
                
                // Use a password validator to bypass normal password rules for admin
                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var passwordResult = await passwordValidator.ValidateAsync(userManager, adminUser, adminPassword);
                if (!passwordResult.Succeeded)
                {
                    Console.WriteLine("Admin password does not meet requirements. Using a secure default.");
                    adminPassword = "Admin123!";
                }
                
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    Console.WriteLine("Admin user created successfully.");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    Console.WriteLine("Failed to create admin user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists, ensuring they are in the Admin role.");
                // Ensure the user is in the Admin role
                if (!await userManager.IsInRoleAsync(adminExists, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminExists, "Admin");
                }
            }
            
            // Seed categories if none exist
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Conference", Description = "Professional conferences and talks" },
                    new Category { Name = "Workshop", Description = "Interactive learning sessions" },
                    new Category { Name = "Concert", Description = "Music performances and shows" },
                    new Category { Name = "Exhibition", Description = "Art, cultural and product exhibitions" },
                    new Category { Name = "Networking", Description = "Business and social networking events" },
                    new Category { Name = "Sports", Description = "Sporting events and tournaments" }
                );
                
                await context.SaveChangesAsync();
            }
            
            // Seed tags if none exist
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Tag { Name = "Technology" },
                    new Tag { Name = "Business" },
                    new Tag { Name = "Education" },
                    new Tag { Name = "Entertainment" },
                    new Tag { Name = "Health" },
                    new Tag { Name = "Art" },
                    new Tag { Name = "Science" },
                    new Tag { Name = "Fashion" },
                    new Tag { Name = "Food" }
                );
                
                await context.SaveChangesAsync();
            }
            
            // Seed events if none exist
            if (!context.Events.Any())
            {
                // Get the first category for default
                var defaultCategory = await context.Categories.FirstAsync();
                
                var techEvent = new Event
                {
                    Name = "Tech Summit 2025",
                    Description = "Annual technology conference featuring the latest advancements in tech",
                    Date = DateTime.Now.AddDays(30),
                    Venue = "Convention Center",
                    Price = 99.99m,
                    ImagePath = "/images/events/tech-summit.jpg",
                    CategoryId = defaultCategory.Id,
                    IsActive = true
                };
                
                context.Events.Add(techEvent);
                await context.SaveChangesAsync();
                
                // Add tags to the event
                var techTag = await context.Tags.FirstOrDefaultAsync(t => t.Name == "Technology");
                var businessTag = await context.Tags.FirstOrDefaultAsync(t => t.Name == "Business");
                
                if (techTag != null && businessTag != null)
                {
                    context.EventTags.AddRange(
                        new EventTag { EventId = techEvent.Id, TagId = techTag.Id },
                        new EventTag { EventId = techEvent.Id, TagId = businessTag.Id }
                    );
                    
                    await context.SaveChangesAsync();
                }
            }
        }
    }
} 