using FluentValidation;
using Mawid.Booking.Api.Dtos.Appointments;

namespace Mawid.Booking.Api.Validators.Appointments
{
    public class CreateAppointmentRequestValidator:AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator()
        {
            RuleFor(a => a.ProviderId).NotEmpty();
            RuleFor(a => a.ServiceId).NotEmpty();                          
            RuleFor(a => a.CustomerName).NotEmpty().MaximumLength(200);    
            RuleFor(a => a.CustomerPhone).NotEmpty().MaximumLength(20);
            RuleFor(a => a.CustomerEmail).NotEmpty().EmailAddress();       

            RuleFor(a => a.StartTime)
                .GreaterThan(_ => DateTimeOffset.UtcNow)
                .WithMessage("Appointment must be in the future");
        }
    }
}
