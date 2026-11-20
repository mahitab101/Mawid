using FluentValidation;
using Mawid.Booking.Api.Dtos.Providers;

namespace Mawid.Booking.Api.Validators.Providers
{
    public class CreateProviderRequestValidator : AbstractValidator<CreateProviderRequest>
    {
        public CreateProviderRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required").MaximumLength(200);
            RuleFor(x => x.Title).MaximumLength(200);
        }
    }
}
