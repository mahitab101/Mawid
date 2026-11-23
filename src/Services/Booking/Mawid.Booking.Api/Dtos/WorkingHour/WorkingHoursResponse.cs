namespace Mawid.Booking.Api.Dtos.WorkingHour
{
    public class WorkingHoursResponse
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
