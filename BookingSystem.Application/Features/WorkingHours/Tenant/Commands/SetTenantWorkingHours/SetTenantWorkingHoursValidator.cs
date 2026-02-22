using FluentValidation;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.SetTenantWorkingHours;

public sealed class SetTenantWorkingHoursValidator : AbstractValidator<SetTenantWorkingHoursCommand>
{
    public SetTenantWorkingHoursValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Days).NotNull().Must(d => d.Count == 7)
            .WithMessage("Days must contain exactly 7 items (0..6).");

        RuleForEach(x => x.Days).ChildRules(day =>
        {
            day.RuleFor(d => d.DayOfWeek).InclusiveBetween(0, 6);
            day.RuleFor(d => d.StartTime).NotEmpty();
            day.RuleFor(d => d.EndTime).NotEmpty();
        });

        RuleFor(x => x.Days)
            .Must(days => days.Select(d => d.DayOfWeek).Distinct().Count() == 7)
            .WithMessage("Days must contain unique DayOfWeek values.");
    }
}
