using Mawid.Booking.Domain.Entities;

namespace Mawid.Booking.Api.Dtos.WorkingHour
{
    public static class WorkingHoursMappings
    {
        public static WorkingHours ToEntity(this WorkingHoursSlotDto slot, Guid providerId) => new()
        {
            ProviderId = providerId,
            Day = slot.Day,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime
        };
        public static WorkingHoursResponse ToResponse(this WorkingHours workingHour) => new()
        {
            Id = workingHour.Id,
            Day = workingHour.Day,
            StartTime = workingHour.StartTime,
            EndTime = workingHour.EndTime,
            ProviderId = workingHour.ProviderId,
        };

        public static List<WorkingHoursResponse> ToResponse(this IEnumerable<WorkingHours> workingHours)
        {
            return workingHours.Select(workingHour => workingHour.ToResponse()).ToList();
        }
    }
}
