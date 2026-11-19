using Mawid.Booking.Domain.Entities;

namespace Mawid.Booking.Api.Dtos.Providers
{
    public class ProviderResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Title { get; set; }
        public bool IsActive { get; set; }
    }
}
