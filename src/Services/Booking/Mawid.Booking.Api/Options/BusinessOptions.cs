namespace Mawid.Booking.Api.Options
{
    public class BusinessOptions
    {
        public const string SectionName = "Business";

        public required string Name { get; init; }
        public required string TimeZone { get; init; }
        public string? Address { get; init; }

        public TimeZoneInfo GetTimeZone() => TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
    }
}
