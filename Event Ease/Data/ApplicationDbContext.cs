using Event_Ease.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Event_Ease.Data
{
    // The ApplicationDbContext inherits from DbContext, which is the primary class 
    // responsible for interacting with the database in EF Core (Microsoft, 2026a).
    public class ApplicationDbContext : DbContext
    {
        // The constructor accepts DbContextOptions, allowing for the configuration 
        // of database providers (e.g., SQL Server) and connection strings (Microsoft, 2026a).
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Each DbSet property represents a table in the database. EF Core uses 
        // these sets to perform CRUD operations on your entities (Microsoft, 2026b).

        // Represents the Venues table in the database.
        public DbSet<Venue> Venues { get; set; }

        // Represents the Events table in the database.
        public DbSet<Event> Events { get; set; }

        // Represents the Bookings table in the database, facilitating relationships 
        // between Venues and Events (Microsoft, 2026c).
        public DbSet<Booking> Bookings { get; set; }
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. DbContext Class (Microsoft.EntityFrameworkCore). [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext
//[Accessed 7 May 2026].

//Microsoft, 2026b.DbSet Class(Microsoft.EntityFrameworkCore). [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1
//[Accessed 7 May 2026].

//Microsoft, 2026c.Relationships in Entity Framework Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
//[Accessed 7 May 2026].