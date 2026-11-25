using Mawid.Booking.Api.Dtos.Appointments;
using Mawid.Booking.Api.Dtos.UnifiedResponse;
using Mawid.Booking.Api.Options;
using Mawid.Booking.Api.Services;
using Mawid.Booking.Domain.Entities;
using Mawid.Booking.Domain.Enums;
using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentsController(BookingDbContext dbContext, 
        AvailabilityService availabilityService,
        IOptions<BusinessOptions> businessOptions) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AppointmentResponse>>> Create(
            CreateAppointmentRequest request, CancellationToken cancellationToken)
        {
            var service = await dbContext.Services.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.ServiceId
  && s.ProviderServices.Any(ps => ps.ProviderId == request.ProviderId && ps.Provider.IsActive), cancellationToken);
            if (service is null)
                return NotFound(ApiResponse<AppointmentResponse>.Fail("Provider or service not found"));

            var startUtc = request.StartTime.ToUniversalTime();
            var timeZone = businessOptions.Value.GetTimeZone();
            var localDate = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(startUtc, timeZone).DateTime);

            // Prevent booking same service in the same day
            var dayStartLocal = localDate.ToDateTime(TimeOnly.MinValue);
            var dayStartUtc = new DateTimeOffset(dayStartLocal, timeZone.GetUtcOffset(dayStartLocal)).ToUniversalTime();
            var dayEndUtc = dayStartUtc.AddDays(1);

            var alreadyBooked = await dbContext.Appointments.AnyAsync(a =>
                a.CustomerPhone == request.CustomerPhone
                && a.ServiceId == request.ServiceId
                && a.Status != AppointmentStatus.Cancelled
                && a.StartTime >= dayStartUtc && a.StartTime < dayEndUtc,
                cancellationToken);

            if (alreadyBooked)
                return Conflict(ApiResponse<AppointmentResponse>.Fail("You already have a booking for this service on this day"));

            var available = await availabilityService.GetAvailableSlotsAsync(
                request.ProviderId, request.ServiceId, localDate, cancellationToken);

            if (available is null || !available.Any(s => s.StartTime == startUtc && s.IsAvailable))
                return Conflict(ApiResponse<AppointmentResponse>.Fail("This time slot is not available"));

            var appointment = new Appointment
            {
                ProviderId = request.ProviderId,
                ServiceId = request.ServiceId,
                StartTime = startUtc,
                EndTime = startUtc.AddMinutes(service.DurationMinutes),
                Status = AppointmentStatus.Pending,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                CustomerEmail = request.CustomerEmail,
                CreatedAt = DateTimeOffset.UtcNow
            };
            dbContext.Appointments.Add(appointment);

            
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                return Conflict(ApiResponse<AppointmentResponse>.Fail("This time slot was just booked by someone else"));
            }

         
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id },
                ApiResponse<AppointmentResponse>.Ok(appointment.ToResponse(), "Appointment booked successfully"));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<AppointmentResponse>>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await dbContext.Appointments.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            return appointment is null
                ? NotFound(ApiResponse<AppointmentResponse>.Fail("Appointment not found"))
                : Ok(ApiResponse<AppointmentResponse>.Ok(appointment.ToResponse()));
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<ActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await dbContext.Appointments
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (appointment is null)
                return NotFound(ApiResponse<object>.Fail("Appointment not found"));

            if (appointment.Status == AppointmentStatus.Cancelled)
                return NoContent();   

            if (appointment.Status == AppointmentStatus.NoShow || appointment.StartTime <= DateTimeOffset.UtcNow)
                return Conflict(ApiResponse<object>.Fail("Past appointments cannot be cancelled"));

            appointment.Status = AppointmentStatus.Cancelled;
            await dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
