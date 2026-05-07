using Microsoft.AspNetCore.Http.HttpResults;

namespace Event_Ease.Models
{
    // The Booking class represents an associative entity that connects Venues and Events, 
    // effectively acting as a join table in a many-to-many relationship (Microsoft, 2026a).
    public class Booking
    {
        // The Id property serves as the Primary Key for the table, allowing for 
        // unique identification of every reservation record (Microsoft, 2026b).
        public int Id { get; set; }

        // VenueId serves as the Foreign Key. Entity Framework Core uses this naming 
        // convention to automatically map the relationship to the Venue table (Microsoft, 2026c).
        public int VenueId { get; set; }

        // The Venue navigation property allows the application to access the properties 
        // of the linked venue directly from a booking object (Microsoft, 2026c).
        public Venue? Venue { get; set; }

        // EventId serves as the Foreign Key linking to the Event table (Microsoft, 2026c).
        public int EventId { get; set; }

        // The Event navigation property is used for Eager Loading, allowing the UI 
        // to display the event name associated with this booking.
        public Event? Event { get; set; }

        // BookingDate represents a payload property. Unlike a simple join table, 
        // this model stores specific metadata about the relationship (Microsoft, 2026a).
        public DateTime BookingDate { get; set; }
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. Many-to-many relationships - EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
//[Accessed 7 May 2026].

//Microsoft, 2026b.Modeling Keys - EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/keys
//[Accessed 7 May 2026].

//Microsoft, 2026c.Relationship mapping in EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
//[Accessed 7 May 2026].