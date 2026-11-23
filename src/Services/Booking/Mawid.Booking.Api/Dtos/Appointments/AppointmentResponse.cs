using Mawid.Booking.Domain.Enums;

namespace Mawid.Booking.Api.Dtos.Appointments
{
    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
