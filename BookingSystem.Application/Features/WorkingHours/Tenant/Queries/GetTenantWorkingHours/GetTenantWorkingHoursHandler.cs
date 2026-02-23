using BookingSystem.Application.DTO.WorkingHours.Tenant;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Queries.GetTenantWorkingHours;

public sealed class GetTenantWorkingHoursHandler
    : IRequestHandler<GetTenantWorkingHoursQuery, List<TenantWorkingHoursDayDto>>
{
    private readonly IWorkingHoursCrudRepository _repo;

    public GetTenantWorkingHoursHandler(IWorkingHoursCrudRepository repo) => _repo = repo;

    public async Task<List<TenantWorkingHoursDayDto>> Handle(GetTenantWorkingHoursQuery request, CancellationToken ct)
    {
        var items = await _repo.GetTenantWeekAsync(request.TenantId, ct);

        return items.Select(x => new TenantWorkingHoursDayDto(
            (int)x.DayOfWeek,
            x.StartTime.ToString("HH:mm"),
            x.EndTime.ToString("HH:mm"),
            x.IsClosed
        )).ToList();
    }
}
