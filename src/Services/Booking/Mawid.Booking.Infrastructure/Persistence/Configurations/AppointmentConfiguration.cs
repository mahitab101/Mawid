using Mawid.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(a=>a.Provider).WithMany().HasForeignKey(a=>a.ProviderId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Service).WithMany().HasForeignKey(a=>a.ServiceId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
