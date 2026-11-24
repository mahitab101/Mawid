using Mawid.Booking.Domain.Entities;

namespace Mawid.Booking.Api.Dtos.Services
{
    public static class ServiceMappings
    {
        public static ServiceResponse ToResponse(this Service service) => new()
        {
            Id = service.Id,
            Title = service.Title,
            Price = service.Price,
            DurationMinutes = service.DurationMinutes
        };

        public static List<ServiceResponse> ToResponse(this IEnumerable<Service> services)
        {
            return services.Select(service  => service.ToResponse()).ToList();
        }

    }
}
