using FluentValidation;
using Mawid.Booking.Api.Dtos.Services;

namespace Mawid.Booking.Api.Validators.Services
{
    public class CreateServiceRequestValidator:AbstractValidator<CreateServiceRequest>
    {
        public CreateServiceRequestValidator()
        {
            RuleFor(s=>s.Title).NotEmpty().WithMessage("{PropertyName} is required").MaximumLength(200);
            RuleFor(s => s.Price).GreaterThanOrEqualTo(0);
            RuleFor(s => s.DurationMinutes).InclusiveBetween(5, 480)
                .WithMessage("{PropertyName} must be between {From} and {To} minutes"); ;
        }
    }
}
