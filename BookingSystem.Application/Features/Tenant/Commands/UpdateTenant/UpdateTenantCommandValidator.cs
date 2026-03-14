using FluentValidation;

namespace BookingSystem.Application.Features.Tenants.Commands.UpdateTenant;

public sealed class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.body.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.body.Slug)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens.");

        RuleFor(x => x.body.Description)
            .MaximumLength(1000);

        RuleFor(x => x.body.Phone)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.body.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.body.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.body.TimeZone)
            .NotEmpty()
            .MaximumLength(100);
    }
}