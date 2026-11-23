using Mawid.Booking.Domain.Entities;

namespace Mawid.Booking.Api.Dtos.Appointments
{
    public static class AppointmentMappings
    {
        public static AppointmentResponse ToResponse(this Appointment a) => new()
        {
            Id = a.Id,
            ProviderId = a.ProviderId,
            ServiceId = a.ServiceId,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            CustomerName = a.CustomerName
        };
    }
}
