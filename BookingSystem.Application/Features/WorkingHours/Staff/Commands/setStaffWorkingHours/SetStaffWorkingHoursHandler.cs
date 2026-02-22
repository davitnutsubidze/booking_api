using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;

public sealed class SetStaffWorkingHoursHandler : IRequestHandler<SetStaffWorkingHoursCommand>
{
    private readonly IWorkingHoursCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public SetStaffWorkingHoursHandler(IWorkingHoursCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(SetStaffWorkingHoursCommand request, CancellationToken ct)
    {
        var items = new List<StaffWorkingHours>();

        foreach (var d in request.Days)
        {
            var start = ParseTime(d.StartTime);
            var end = ParseTime(d.EndTime);

            if (!d.IsDayOff && end <= start)
                throw new ConflictException($"EndTime must be after StartTime for DayOfWeek={d.DayOfWeek}.");

            items.Add(new StaffWorkingHours
            {
                StaffId = request.StaffId,
                DayOfWeek = (DayOfWeek)d.DayOfWeek,
                StartTime = start,
                EndTime = end,
                IsDayOff = d.IsDayOff
            });
        }

        await _repo.ReplaceStaffWeekAsync(request.StaffId, items, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private static TimeOnly ParseTime(string value)
        => TimeOnly.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture);
}
