namespace Event_Ease.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; } // For your placeholder links

        // Navigation property
        public ICollection<Booking>? Bookings { get; set; }
    }
}