namespace Mawid.Booking.Api.Dtos.WorkingHour
{
    public class UpdateWorkingHoursRequest
    {
        public List<WorkingHoursSlotDto> Slots { get; set; } = [];
    }
}
