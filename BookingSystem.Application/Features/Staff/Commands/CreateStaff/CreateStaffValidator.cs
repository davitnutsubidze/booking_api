using FluentValidation;

namespace BookingSystem.Application.Features.Staff.Commands.CreateStaff;

public sealed class CreateStaffValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Body.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Body.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Body.Phone).MaximumLength(30);
        RuleFor(x => x.Body.Bio).MaximumLength(1000);
    }
}
