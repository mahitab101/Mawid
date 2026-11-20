using Mawid.Booking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/providers/{providerId:guid}[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly BookingDbContext _dbContext;

        public ServicesController(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
