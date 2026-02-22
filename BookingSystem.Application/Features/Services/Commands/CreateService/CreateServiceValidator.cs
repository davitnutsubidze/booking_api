using FluentValidation;

namespace BookingSystem.Application.Features.Services.Commands.CreateService;

public sealed class CreateServiceValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Body.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Body.Description).MaximumLength(2000);
        RuleFor(x => x.Body.DurationMinutes).InclusiveBetween(5, 24 * 60);
        RuleFor(x => x.Body.Price).GreaterThanOrEqualTo(0).When(x => x.Body.Price is not null);
        RuleFor(x => x.Body.Currency).MaximumLength(10);
    }
}
