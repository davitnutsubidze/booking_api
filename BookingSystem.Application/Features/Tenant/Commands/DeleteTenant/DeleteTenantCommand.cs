using MediatR;

namespace BookingSystem.Application.Features.Tenants.Commands.DeleteTenant;

public sealed record DeleteTenantCommand(Guid Id) : IRequest<bool>;