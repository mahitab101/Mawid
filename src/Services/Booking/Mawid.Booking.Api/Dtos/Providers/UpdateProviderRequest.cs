namespace Mawid.Booking.Api.Dtos.Providers
{
    public class UpdateProviderRequest
    {
        public required string Name { get; set; }
        public string? Title { get; set; }
    }
}
