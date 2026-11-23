using Mawid.Booking.Api.Dtos.Providers;
using Mawid.Booking.Api.Dtos.UnifiedResponse;
using Mawid.Booking.Api.Services;
using Mawid.Booking.Domain.Entities;
using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly BookingDbContext _dbContext;

        public ProvidersController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProviderResponse>>> Create(
           CreateProviderRequest request, CancellationToken cancellationToken)
        {
            var provider = new Provider
            {
                Name = request.Name,
                Title = request.Title,
                IsActive = true
            };

            _dbContext.Providers.Add(provider);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = ApiResponse<ProviderResponse>.Ok(provider.ToResponse(), "Provider created successfully");
            return CreatedAtAction(nameof(GetById), new { id = provider.Id }, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProviderResponse>>> GetById(
            Guid id, CancellationToken cancellationToken)
        {
            var provider = await _dbContext.Providers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (provider is null)
                return NotFound(ApiResponse<ProviderResponse>.Fail("Provider not found"));

            return Ok(ApiResponse<ProviderResponse>.Ok(provider.ToResponse(), "Provider Retrived Successfully"));
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProviderResponse>>>> GetAll(
            CancellationToken cancellationToken)
        {
            var providers = await _dbContext.Providers
                .Where(p=>p.IsActive)
                .OrderBy(p=>p.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = providers.ToResponse();

            return Ok(
                ApiResponse<List<ProviderResponse>>.Ok(
                    response,
                    "Providers retrieved successfully"));
        }


        [HttpPatch("{id:guid}/deactivate")]
        public async Task<ActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
        {
            var provider = await _dbContext.Providers
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (provider is null) return NotFound(ApiResponse<object>.Fail("Provider not found"));

            provider.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(ApiResponse<object>.Ok("Provider Deactivated Successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id,UpdateProviderRequest request ,CancellationToken cancellationToken)
        {
            var provider = await _dbContext.Providers
                          .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (provider is null) return NotFound(ApiResponse<object>.Fail("Provider not found"));

            provider.Name = request.Name;
            provider.Title = request.Title;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(
                ApiResponse<ProviderResponse>.Ok(
                    provider.ToResponse(),
                    "Provider updated successfully"));
        }

        [HttpGet("{id:guid}/availability")]
        public async Task<ActionResult<ApiResponse<List<DateTimeOffset>>>> GetAvailability(
        Guid id,
        [FromQuery] DateOnly date,
        [FromQuery] Guid serviceId,
        [FromServices] AvailabilityService availabilityService,
        CancellationToken cancellationToken)
        {
            var slots = await availabilityService.GetAvailableSlotsAsync(id, serviceId, date, cancellationToken);

            if (slots is null)
                return NotFound(ApiResponse<List<DateTimeOffset>>.Fail("Provider or service not found"));

            return Ok(ApiResponse<List<DateTimeOffset>>.Ok(slots, "Available slots retrieved successfully"));
        }
    }
}
