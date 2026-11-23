using Mawid.Booking.Domain.Enums;
using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mawid.Booking.Api.Services
{
    public class AvailabilityService(BookingDbContext dbContext)
    {
        public async Task<List<DateTimeOffset>?> GetAvailableSlotsAsync(
            Guid providerId, Guid serviceId, DateOnly date, CancellationToken cancellationToken)
        {
            var service = await dbContext.Services
                         .AsNoTracking()
                         .FirstOrDefaultAsync(s => s.Id == serviceId
                                              && s.ProviderId == providerId
                                              && s.Provider.IsActive, cancellationToken);

            if (service is null)
                return null;

            var workingHours = await dbContext.WorkingHours
                .AsNoTracking()
                .Where(w => w.ProviderId == providerId && w.Day == date.DayOfWeek)
                .OrderBy(w => w.StartTime)
                .ToListAsync(cancellationToken);

            if (workingHours.Count == 0)
                return [];

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dubai");
            var duration = TimeSpan.FromMinutes(service.DurationMinutes);
            var slots = new List<(DateTimeOffset Start, DateTimeOffset End)>();

            foreach (var period in workingHours)
            {
                var current = date.ToDateTime(period.StartTime);  
                var periodEnd = date.ToDateTime(period.EndTime);   

                while (current + duration <= periodEnd)
                {
                    var startUtc = new DateTimeOffset(current, timeZone.GetUtcOffset(current)).ToUniversalTime();
                    slots.Add((startUtc, startUtc + duration));
                    current += duration;
                }
            }

            if (slots.Count == 0)
                return [];

            var dayStartUtc = slots.Min(s => s.Start);
            var dayEndUtc = slots.Max(s => s.End);

            var booked = await dbContext.Appointments
                .AsNoTracking()
                .Where(a => a.ProviderId == providerId
                         && a.Status != AppointmentStatus.Cancelled
                         && a.StartTime < dayEndUtc && a.EndTime > dayStartUtc)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToListAsync(cancellationToken);

            return slots
                .Where(slot => !booked.Any(b => slot.Start < b.EndTime && slot.End > b.StartTime))
                .Select(slot => slot.Start)
                .ToList();
        }
    }
}
