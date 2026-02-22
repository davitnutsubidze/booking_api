using FluentValidation;

namespace BookingSystem.Application.Features.BlockedTimes.Commands.CreateBlockedTime;

public sealed class CreateBlockedTimeValidator : AbstractValidator<CreateBlockedTimeCommand>
{
    public CreateBlockedTimeValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();

        RuleFor(x => x.Body.StartUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("StartUtc must be UTC.");

        RuleFor(x => x.Body.EndUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("EndUtc must be UTC.");

        RuleFor(x => x.Body)
            .Must(b => b.EndUtc > b.StartUtc)
            .WithMessage("EndUtc must be after StartUtc.");
    }
}
