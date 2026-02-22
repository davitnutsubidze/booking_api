using BookingSystem.Application.DTOs.BlockedTimes;
using MediatR;

namespace BookingSystem.Application.Features.BlockedTimes.Commands.CreateBlockedTime;

public sealed record CreateBlockedTimeCommand(
    Guid TenantId,
    CreateBlockedTimeRequestDto Body
) : IRequest<BlockedTimeDto>;
