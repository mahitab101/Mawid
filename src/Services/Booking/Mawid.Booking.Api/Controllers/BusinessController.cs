using Mawid.Booking.Api.Dtos.Business;
using Mawid.Booking.Api.Dtos.UnifiedResponse;
using Mawid.Booking.Api.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mawid.Booking.Api.Controllers
{
    [Route("api/business")]
    [ApiController]
    public class BusinessController(IOptions<BusinessOptions> businessOptions) : ControllerBase
    {
        [HttpGet]
        public ActionResult<ApiResponse<BusinessResponse>> Get()
        {
            var b = businessOptions.Value;
            return Ok(ApiResponse<BusinessResponse>.Ok(new BusinessResponse(b.Name, b.Address, b.TimeZone)));
        }
    }
}
