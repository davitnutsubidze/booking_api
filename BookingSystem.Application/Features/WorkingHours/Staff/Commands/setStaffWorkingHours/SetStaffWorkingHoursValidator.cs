using FluentValidation;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;

public sealed class SetStaffWorkingHoursValidator : AbstractValidator<SetStaffWorkingHoursCommand>
{
    public SetStaffWorkingHoursValidator()
    {
        RuleFor(x => x.StaffId).NotEmpty();
        RuleFor(x => x.Days).NotNull().Must(d => d.Count == 7)
            .WithMessage("Days must contain exactly 7 items (0..6).");

        RuleFor(x => x.Days)
            .Must(days => days.Select(d => d.DayOfWeek).Distinct().Count() == 7)
            .WithMessage("Days must contain unique DayOfWeek values.");
    }
}
