namespace Mawid.Booking.Api.Dtos.Services
{
    public class CreateServiceRequest
    {
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
