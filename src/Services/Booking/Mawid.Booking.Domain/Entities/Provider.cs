using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public class Provider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Title { get; set; }   
        public bool IsActive { get; set; }

        public List<Service> Services { get; set; } = [];
        public List<WorkingHours> WorkingHours { get; set; } = [];
    }
}
