using FluentValidation;
using Mawid.Booking.Api.Dtos.WorkingHour;

namespace Mawid.Booking.Api.Validators.WorkingHour
{
    public class UpdateWorkingHoursRequestValidator :AbstractValidator<UpdateWorkingHoursRequest>
    {
        public UpdateWorkingHoursRequestValidator()
        {
            RuleForEach(x => x.Slots).SetValidator(new WorkingHoursSlotDtoValidator());
            RuleFor(x => x.Slots)
                .Must(NotOverlap)
                .WithMessage("Working hours on the same day must not overlap");
        }

        private static bool NotOverlap(List<WorkingHoursSlotDto> slots)
        {
            foreach (var day in slots.GroupBy(s => s.Day))
            {
                var ordered = day.OrderBy(s => s.StartTime).ToList();

                for (int i = 1; i < ordered.Count; i++)
                {
                    if (ordered[i].StartTime < ordered[i - 1].EndTime)
                        return false;
                }
            }
            return true;
        }
    }
}
