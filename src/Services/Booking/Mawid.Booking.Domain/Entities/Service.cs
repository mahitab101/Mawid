using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public Provider Provider { get; set; } = null!;
    }
}
