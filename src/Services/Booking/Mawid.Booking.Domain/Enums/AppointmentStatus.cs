using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Enums
{
    public enum AppointmentStatus
    {
        Pending = 0, 
        Confirmed = 1,
        Cancelled = 2, 
        NoShow = 3
    }
}
