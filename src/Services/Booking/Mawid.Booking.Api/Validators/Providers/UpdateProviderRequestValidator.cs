using FluentValidation;
using Mawid.Booking.Api.Dtos.Providers;

namespace Mawid.Booking.Api.Validators.Providers
{
    public class UpdateProviderRequestValidator:AbstractValidator<UpdateProviderRequest>
    {
        public UpdateProviderRequestValidator()
        {
            RuleFor(u=>u.Name).NotEmpty().WithMessage("{PropertyName} is required").MaximumLength(200);
        }
    }
}
