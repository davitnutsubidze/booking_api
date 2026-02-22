using FluentValidation;

namespace BookingSystem.Application.Features.Appointments.Commands.CreateAppointment;

public sealed class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();

        RuleFor(x => x.CustomerFirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CustomerLastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CustomerPhone).NotEmpty().MaximumLength(30);

        RuleFor(x => x.StartDateTimeUtc)
            .Must(d => d.Kind == DateTimeKind.Utc)
            .WithMessage("StartDateTimeUtc must be UTC (DateTimeKind.Utc).");

        RuleFor(x => x.StartDateTimeUtc)
            .Must(d => d > DateTime.UtcNow.AddMinutes(1))
            .WithMessage("StartDateTimeUtc must be in the future.");
    }
}
