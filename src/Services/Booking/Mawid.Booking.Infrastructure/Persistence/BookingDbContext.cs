using Mawid.Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mawid.Booking.Infrastructure.Persistence
{
    public class BookingDbContext(DbContextOptions<BookingDbContext> options)
      : DbContext(options)
    {
        public DbSet<Provider> Providers => Set<Provider>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<WorkingHours> WorkingHours => Set<WorkingHours>();
        public DbSet<Appointment> Appointments => Set<Appointment>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
    => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
    }


}
