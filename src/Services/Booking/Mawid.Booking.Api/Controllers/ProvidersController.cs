using Mawid.Booking.Api.Dtos.Providers;
using Mawid.Booking.Api.Dtos.Services;
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


        [HttpPut("{id:guid}/services")]
        public async Task<ActionResult<ApiResponse<List<ServiceResponse>>>> UpdateServices(
    Guid id, UpdateProviderServicesRequest request, CancellationToken cancellationToken)
        {
            var providerExists = await _dbContext.Providers
                .AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
            if (!providerExists)
                return NotFound(ApiResponse<List<ServiceResponse>>.Fail("Provider not found"));

            var services = await _dbContext.Services
                .Where(s => request.ServiceIds.Contains(s.Id))
                .ToListAsync(cancellationToken);
            if (services.Count != request.ServiceIds.Count)
                return BadRequest(ApiResponse<List<ServiceResponse>>.Fail("One or more services were not found"));

            var oldLinks = await _dbContext.ProviderServices
                .Where(ps => ps.ProviderId == id)
                .ToListAsync(cancellationToken);
            _dbContext.ProviderServices.RemoveRange(oldLinks);

            _dbContext.ProviderServices.AddRange(
                request.ServiceIds.Select(serviceId => new ProviderService { ProviderId = id, ServiceId = serviceId }));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(ApiResponse<List<ServiceResponse>>.Ok(
                services.OrderBy(s => s.Title).ToResponse(), "Provider services updated"));
        }
    }
}
