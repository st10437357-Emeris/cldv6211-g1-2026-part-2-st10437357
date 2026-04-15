namespace Event_Ease.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // For your placeholder links

        // Navigation property to link back to Bookings
        public ICollection<Booking>? Bookings { get; set; }
    }
}