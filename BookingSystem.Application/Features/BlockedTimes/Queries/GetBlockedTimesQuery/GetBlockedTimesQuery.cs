using BookingSystem.Application.DTOs.BlockedTimes;
using MediatR;

namespace BookingSystem.Application.Features.BlockedTimes.Queries.GetBlockedTimes;

public sealed record GetBlockedTimesQuery(
    Guid TenantId,
    DateTime FromUtc,
    DateTime ToUtc,
    Guid? StaffId
) : IRequest<List<BlockedTimeDto>>;
