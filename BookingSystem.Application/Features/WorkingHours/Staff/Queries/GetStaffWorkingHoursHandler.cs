using BookingSystem.Application.DTO.WorkingHours.Staff;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Queries.GetStaffWorkingHours;

public sealed class GetStaffWorkingHoursHandler
    : IRequestHandler<GetStaffWorkingHoursQuery, List<PatchWorkingHoursDayDto>>
{
    private readonly IWorkingHoursCrudRepository _repo;

    public GetStaffWorkingHoursHandler(IWorkingHoursCrudRepository repo) => _repo = repo;

    public async Task<List<PatchWorkingHoursDayDto>> Handle(GetStaffWorkingHoursQuery request, CancellationToken ct)
    {
        var items = await _repo.GetStaffWeekAsync(request.StaffId, ct);

        return items.Select(x => new PatchWorkingHoursDayDto(
            x.StaffId,
            (int)x.DayOfWeek,
            x.StartTime.ToString("HH:mm"),
            x.EndTime.ToString("HH:mm"),
            x.IsDayOff
        )).ToList();
    }
}
