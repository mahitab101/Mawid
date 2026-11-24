using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public class Provider : Entity
    {
        public string Name { get; set; }
        public string? Title { get; set; }   
        public bool IsActive { get; set; }

        public List<ProviderService> ProviderServices { get; set; } = [];
        public List<WorkingHours> WorkingHours { get; set; } = [];
    }
}
