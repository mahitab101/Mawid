using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();
    }
}
