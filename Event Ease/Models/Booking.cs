namespace Event_Ease.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // Foreign Key linking to the Venue table
        public int VenueId { get; set; }
        public Venue? Venue { get; set; } // Navigation property

        // Foreign Key linking to the Event table
        public int EventId { get; set; }
        public Event? Event { get; set; } // Navigation property

        // The specific date this event is booked at this venue
        public DateTime BookingDate { get; set; }
    }
}