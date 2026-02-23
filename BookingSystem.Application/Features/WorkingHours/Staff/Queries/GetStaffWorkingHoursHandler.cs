using BookingSystem.Application.DTOs.WorkingHours;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Queries.GetStaffWorkingHours;

public sealed class GetStaffWorkingHoursHandler
    : IRequestHandler<GetStaffWorkingHoursQuery, List<StaffWorkingHoursDayDto>>
{
    private readonly IWorkingHoursCrudRepository _repo;

    public GetStaffWorkingHoursHandler(IWorkingHoursCrudRepository repo) => _repo = repo;

    public async Task<List<StaffWorkingHoursDayDto>> Handle(GetStaffWorkingHoursQuery request, CancellationToken ct)
    {
        var items = await _repo.GetStaffWeekAsync(request.StaffId, ct);

        return items.Select(x => new StaffWorkingHoursDayDto(
            x.StaffId,
            (int)x.DayOfWeek,
            x.StartTime.ToString("HH:mm"),
            x.EndTime.ToString("HH:mm"),
            x.IsDayOff
        )).ToList();
    }
}
