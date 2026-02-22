using BookingSystem.Application.DTOs.BlockedTimes;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.BlockedTimes.Queries.GetBlockedTimes;

public sealed class GetBlockedTimesHandler : IRequestHandler<GetBlockedTimesQuery, List<BlockedTimeDto>>
{
    private readonly IBlockedTimeCrudRepository _repo;

    public GetBlockedTimesHandler(IBlockedTimeCrudRepository repo) => _repo = repo;

    public async Task<List<BlockedTimeDto>> Handle(GetBlockedTimesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetRangeAsync(request.TenantId, request.StaffId, request.FromUtc, request.ToUtc, ct);

        return items.Select(x => new BlockedTimeDto(
            x.Id,
            x.TenantId, // tenant id
            x.StaffId,
            x.StartDateTimeUtc,
            x.EndDateTimeUtc,
            x.Reason
        )).ToList();
    }
}
