using BookingSystem.Application.DTOs.LunchBreaks;
using BookingSystem.Application.Interfaces;
using MediatR;
using static System.Net.Mime.MediaTypeNames;


namespace BookingSystem.Application.Features.Services.Queries.GetStaffLunchBreak;

public sealed class GetStaffLunchBreakHandler: IRequestHandler<GetStaffLunchBreaksQuery, List<StaffLunchBreakDto>>
{
    private readonly IStaffLunchBreakRepository _repo;

    public GetStaffLunchBreakHandler(IStaffLunchBreakRepository repo) => _repo = repo;


    public async Task<List<StaffLunchBreakDto>> Handle(GetStaffLunchBreaksQuery request, CancellationToken ct)
    {
        var list = await _repo.GetByStaffAsync(request.StaffId, ct);

        return list.Select(x => new StaffLunchBreakDto(
            x.Id,
            x.TenantId,
            x.StaffId,
            (int)x.DayOfWeek,
            x.StartTime.ToString("HH:mm"),
            x.EndTime.ToString("HH:mm"),
            x.IsEnabled
        )).ToList();
    }
}
