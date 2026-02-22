using FluentValidation;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.PatchTenantWorkingHours;

public sealed class PatchTenantWorkingHoursValidator : AbstractValidator<PatchTenantWorkingHoursCommand>
{
    public PatchTenantWorkingHoursValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.Body).NotNull();

        RuleFor(x => x.Body.StartTime).Matches(@"^\d{2}:\d{2}$")
            .When(x => x.Body.StartTime is not null);

        RuleFor(x => x.Body.EndTime).Matches(@"^\d{2}:\d{2}$")
            .When(x => x.Body.EndTime is not null);

        RuleFor(x => x.Body)
            .Must(b => b.StartTime is not null || b.EndTime is not null || b.IsClosed is not null)
            .WithMessage("At least one field must be provided.");
    }
}