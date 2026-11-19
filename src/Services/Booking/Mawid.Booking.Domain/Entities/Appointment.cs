using Mawid.Booking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public class Appointment : Entity
    {
        public Guid ProviderId { get; set; }
        public Guid ServiceId { get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public Provider Provider { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}
