using FluentValidation;
using Mawid.Booking.Api.Dtos.WorkingHour;

namespace Mawid.Booking.Api.Validators.WorkingHour
{
    public class WorkingHoursSlotDtoValidator : AbstractValidator<WorkingHoursSlotDto>
    {
        public WorkingHoursSlotDtoValidator()
        {
            RuleFor(x => x.Day).IsInEnum();
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime)
                .WithMessage("{PropertyName} must be after StartTime");
        }
    }
}
