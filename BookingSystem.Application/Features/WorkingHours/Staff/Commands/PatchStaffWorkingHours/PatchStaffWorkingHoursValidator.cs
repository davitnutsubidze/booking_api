using FluentValidation;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;

public sealed class PatchStaffWorkingHoursValidator : AbstractValidator<PatchStaffWorkingHoursCommand>
{
    public PatchStaffWorkingHoursValidator()
    {
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.Body).NotNull();

        RuleFor(x => x.Body.StartTime).Matches(@"^\d{2}:\d{2}$")
            .When(x => x.Body.StartTime is not null);

        RuleFor(x => x.Body.EndTime).Matches(@"^\d{2}:\d{2}$")
            .When(x => x.Body.EndTime is not null);

        RuleFor(x => x.Body)
            .Must(b => b.StartTime is not null || b.EndTime is not null || b.IsDayOff is not null)
            .WithMessage("At least one field must be provided.");
    }
}
