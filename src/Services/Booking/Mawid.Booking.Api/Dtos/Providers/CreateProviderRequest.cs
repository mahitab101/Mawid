namespace Mawid.Booking.Api.Dtos.Providers
{
    public class CreateProviderRequest
    {
        public required string Name { get; set; }
        public string? Title { get; set; }
    }
}
