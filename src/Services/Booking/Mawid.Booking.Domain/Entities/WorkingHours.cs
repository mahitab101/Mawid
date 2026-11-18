using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public class WorkingHours
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public Provider Provider { get; set; } = null!;
    }
}
