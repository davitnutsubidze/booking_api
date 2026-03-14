using FluentValidation;

namespace BookingSystem.Application.Features.Tenants.Commands.DeleteTenant;

public sealed class DeleteTenantCommandValidator : AbstractValidator<DeleteTenantCommand>
{
    public DeleteTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}