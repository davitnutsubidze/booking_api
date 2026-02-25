using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTO.WorkingHours.Tenant;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.PatchTenantWorkingHours;

public sealed class PatchTenantWorkingHoursHandler : IRequestHandler<PatchTenantWorkingHoursCommand, TenantWorkingHoursDayDto>
{
    private readonly IWorkingHoursCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public PatchTenantWorkingHoursHandler(IWorkingHoursCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<TenantWorkingHoursDayDto> Handle(PatchTenantWorkingHoursCommand request, CancellationToken ct)
    {
        var day = (DayOfWeek)request.DayOfWeek;

        var entity = await _repo.GetBusinessDayAsync(request.TenantId, day, ct)
            ?? throw new NotFoundException("Working hours not found for this day.");

        // Apply patch (only what user sent)
        if (request.Body.IsClosed is not null)
            entity.IsClosed = request.Body.IsClosed.Value;

        if (request.Body.StartTime is not null)
            entity.StartTime = ParseTime(request.Body.StartTime);

        if (request.Body.EndTime is not null)
            entity.EndTime = ParseTime(request.Body.EndTime);

        // Validate logical constraints (only when open)
        if (!entity.IsClosed && entity.EndTime <= entity.StartTime)
            throw new ConflictException("EndTime must be after StartTime when day is open.");

        await _uow.SaveChangesAsync(ct);
        return new TenantWorkingHoursDayDto((int) entity.DayOfWeek, entity.StartTime.ToString("HH:mm"), entity.EndTime.ToString("HH:mm"), entity.IsClosed);
    }

    private static TimeOnly ParseTime(string value)
        => TimeOnly.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture);
}