using Mawid.Booking.Api.Dtos.Providers;
using Mawid.Booking.Domain.Entities;

public static class ProviderMappings
{
    public static ProviderResponse ToResponse(this Provider provider) => new()
    {
        Id = provider.Id,
        Name = provider.Name,
        Title = provider.Title,
        IsActive = provider.IsActive
    };

    public static List<ProviderResponse> ToResponse(
        this IEnumerable<Provider> providers)
    {
        return providers
            .Select(provider => provider.ToResponse())
            .ToList();
    }
}