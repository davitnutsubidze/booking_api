using FluentValidation;

namespace BookingSystem.Application.Features.StaffServices.Commands.SetStaffServices;

public sealed class SetStaffServicesValidator : AbstractValidator<SetStaffServicesCommand>
{
    public SetStaffServicesValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.StaffId).NotEmpty();

        RuleFor(x => x.ServiceIds)
            .NotNull()
            .Must(ids => ids.Count > 0)
            .WithMessage("ServiceIds cannot be empty.");

        RuleFor(x => x.ServiceIds)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("ServiceIds must be unique.");
    }
}
