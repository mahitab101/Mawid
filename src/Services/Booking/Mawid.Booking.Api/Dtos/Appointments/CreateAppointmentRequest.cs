using Mawid.Booking.Domain.Enums;

namespace Mawid.Booking.Api.Dtos.Appointments
{
    public class CreateAppointmentRequest
    {
        public Guid ProviderId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
