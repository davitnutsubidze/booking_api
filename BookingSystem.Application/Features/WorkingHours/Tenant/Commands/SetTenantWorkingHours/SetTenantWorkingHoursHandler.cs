using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.SetTenantWorkingHours;

public sealed class SetTenantWorkingHoursHandler : IRequestHandler<SetTenantWorkingHoursCommand>
{
    private readonly IWorkingHoursCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public SetTenantWorkingHoursHandler(IWorkingHoursCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(SetTenantWorkingHoursCommand request, CancellationToken ct)
    {
        var items = new List<TenantWorkingHours>();

        foreach (var d in request.Days)
        {
            var start = ParseTime(d.StartTime);
            var end = ParseTime(d.EndTime);

            if (!d.IsClosed && end <= start)
                throw new ConflictException($"EndTime must be after StartTime for DayOfWeek={d.DayOfWeek}.");

            items.Add(new TenantWorkingHours
            {
                TenantId = request.TenantId,
                DayOfWeek = (DayOfWeek)d.DayOfWeek,
                StartTime = start,
                EndTime = end,
                IsClosed = d.IsClosed
                // თუ CreatedAt გაქვს NOT NULL, ან default მიეცი DB-ში,
                // ან აქ ჩაწერე CreatedAt = DateTime.UtcNow
            });
        }

        await _repo.ReplaceTenantWeekAsync(request.TenantId, items, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private static TimeOnly ParseTime(string value)
        => TimeOnly.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture);
}
