using FluentValidation;

namespace BookingSystem.Application.Features.Tenants.Queries.GetTenantById;

public sealed class GetTenantByIdQueryValidator : AbstractValidator<GetTenantByIdQuery>
{
    public GetTenantByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}