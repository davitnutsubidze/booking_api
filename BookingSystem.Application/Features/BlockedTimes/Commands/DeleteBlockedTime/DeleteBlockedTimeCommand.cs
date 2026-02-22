using MediatR;

namespace BookingSystem.Application.Features.BlockedTimes.Commands.DeleteBlockedTime;

public sealed record DeleteBlockedTimeCommand(Guid TenantId, Guid Id) : IRequest;
