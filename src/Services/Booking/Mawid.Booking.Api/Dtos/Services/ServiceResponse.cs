using Mawid.Booking.Domain.Entities;

namespace Mawid.Booking.Api.Dtos.Services
{
    public class ServiceResponse
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
