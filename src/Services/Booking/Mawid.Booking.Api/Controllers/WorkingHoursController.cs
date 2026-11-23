using Mawid.Booking.Api.Dtos.Services;
using Mawid.Booking.Api.Dtos.UnifiedResponse;
using Mawid.Booking.Api.Dtos.WorkingHour;
using Mawid.Booking.Domain.Entities;
using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/providers/{providerId:guid}/working-hours")]
    [ApiController]
    public class WorkingHoursController : ControllerBase
    {
        private readonly BookingDbContext _dbContext;

        public WorkingHoursController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<List<WorkingHoursResponse>>>> Replace(
            Guid providerId, UpdateWorkingHoursRequest request, CancellationToken cancellationToken)
        {
            var providedExsist = await _dbContext.Providers.AnyAsync(p => p.Id == providerId && p.IsActive, cancellationToken);
            if (!providedExsist)
                return NotFound(ApiResponse<List<WorkingHoursResponse>>.Fail("Provider not found"));


            var oldHours = await _dbContext.WorkingHours
                .Where(w => w.ProviderId == providerId)
                .ToListAsync(cancellationToken);

            _dbContext.WorkingHours.RemoveRange(oldHours);


            var newHours = request.Slots
                .Select(slot => slot.ToEntity(providerId))
                .ToList();

            _dbContext.WorkingHours.AddRange(newHours);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(ApiResponse<List<WorkingHoursResponse>>.Ok(newHours.ToResponse(), "Working hours updated"));
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<WorkingHoursResponse>>>> GetAll(Guid providerId, CancellationToken cancellationToken)
        {
            var providerExists = await _dbContext.Providers
               .AnyAsync(p => p.Id == providerId && p.IsActive, cancellationToken);

            if (!providerExists)
                return NotFound(ApiResponse<List<WorkingHoursResponse>>.Fail("Provider not found"));

            var workingHours = await _dbContext.WorkingHours
                .AsNoTracking()
                .Where(s => s.ProviderId == providerId)
                .OrderBy(s => s.Day)
                .ThenBy(s => s.StartTime)
                .ToListAsync(cancellationToken);

            return Ok(
                   ApiResponse<List<WorkingHoursResponse>>.Ok(
                   workingHours.ToResponse(),
                   "Working hours retrieved successfully"));
        }
    }
}
