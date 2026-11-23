namespace Mawid.Booking.Api.Dtos.WorkingHour
{
    public class WorkingHoursSlotDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
