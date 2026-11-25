namespace Mawid.Booking.Api.Dtos.Availability
{
    public record SlotResponse(DateTimeOffset StartTime, bool IsAvailable);
}
