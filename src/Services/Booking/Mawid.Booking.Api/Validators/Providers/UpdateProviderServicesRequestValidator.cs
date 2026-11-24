using FluentValidation;
using Mawid.Booking.Api.Dtos.Providers;

namespace Mawid.Booking.Api.Validators.Providers
{
    public class UpdateProviderServicesRequestValidator : AbstractValidator<UpdateProviderServicesRequest>
    {
        public UpdateProviderServicesRequestValidator()
        {
            RuleFor(x => x.ServiceIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Service IDs must not contain duplicates");
        }
    }
}
