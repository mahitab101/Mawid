using Mawid.Booking.Api.Dtos.Services;
using Mawid.Booking.Api.Dtos.UnifiedResponse;
using Mawid.Booking.Domain.Entities;
using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/providers/{providerId:guid}/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly BookingDbContext _dbContext;

        public ServicesController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Create(Guid providerId, CreateServiceRequest request, CancellationToken cancellationToken)
        {
            var providerExists = await _dbContext.Providers
                .AnyAsync(p => p.Id == providerId && p.IsActive, cancellationToken);

            if (!providerExists)
                return NotFound(ApiResponse<ServiceResponse>.Fail("Provider not found"));

            Service newService = new Service
            {
                ProviderId = providerId,
                Title = request.Title,
                Price = request.Price,
                DurationMinutes = request.DurationMinutes
            };

            _dbContext.Services.Add(newService);
            await _dbContext.SaveChangesAsync();

            var response = ApiResponse<ServiceResponse>.Ok(newService.ToResponse(), "Service Added Successfully");

            return Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult<List<ServiceResponse>>> GetAll(Guid providerId, CancellationToken cancellationToken)
        {
            var providerExists = await _dbContext.Providers
                .AnyAsync(p => p.Id == providerId && p.IsActive, cancellationToken);

            if (!providerExists)
                return NotFound(ApiResponse<ServiceResponse>.Fail("Provider not found"));

            var providerServices = await _dbContext.Services
                .AsNoTracking()
                .Where(s => s.ProviderId == providerId)
                .OrderBy(s => s.Title)
                .ToListAsync(cancellationToken);

            return Ok(
                ApiResponse<List<ServiceResponse>>.Ok(
                    providerServices.ToResponse(),
                    "Services retrieved successfully"));
        }
    }
}
