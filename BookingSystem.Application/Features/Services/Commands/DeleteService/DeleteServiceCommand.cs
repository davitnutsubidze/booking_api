using MediatR;

namespace BookingSystem.Application.Features.Services.Commands.DeleteService;

public sealed record DeleteServiceCommand(Guid TenantId, Guid Id) : IRequest;
