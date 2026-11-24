using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public class Service : Entity
    {
        public string Title { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public List<ProviderService> ProviderServices { get; set; } = [];
    }
}
