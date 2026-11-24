using Mawid.Booking.Api.Dtos.Providers;
using Mawid.Booking.Api.Dtos.Services;
using Mawid.Booking.Api.Dtos.UnifiedResponse;
using Mawid.Booking.Domain.Entities;
using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly BookingDbContext _dbContext;

        public ServicesController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Create(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            Service newService = new Service
            {
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
        public async Task<ActionResult<ApiResponse<List<ServiceResponse>>>> GetAll(CancellationToken cancellationToken)
        {
            var services = await _dbContext.Services
                .AsNoTracking()
                .OrderBy(s => s.Title)
                .ToListAsync(cancellationToken);

            return Ok(ApiResponse<List<ServiceResponse>>.Ok(services.ToResponse(), "Services retrieved successfully"));
        }

        [HttpGet("{id:guid}/providers")]
        public async Task<ActionResult<ApiResponse<List<ProviderResponse>>>> GetProviders(
    Guid id, CancellationToken cancellationToken)
        {
            var serviceExists = await _dbContext.Services.AnyAsync(s => s.Id == id, cancellationToken);
            if (!serviceExists)
                return NotFound(ApiResponse<List<ProviderResponse>>.Fail("Service not found"));

            var providers = await _dbContext.ProviderServices
                .AsNoTracking()
                .Where(ps => ps.ServiceId == id && ps.Provider.IsActive)
                .Select(ps => ps.Provider)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            return Ok(ApiResponse<List<ProviderResponse>>.Ok(providers.ToResponse(), "Providers retrieved successfully"));
        }
    }
}
