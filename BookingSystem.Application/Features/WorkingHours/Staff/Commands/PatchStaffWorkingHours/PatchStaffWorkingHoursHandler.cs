using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTO.WorkingHours.Staff;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;

public sealed class PatchStaffWorkingHoursHandler : IRequestHandler<PatchStaffWorkingHoursCommand, StaffWorkingHoursDayDto>
{
    private readonly IStaffWorkingHoursRepository _repo;
    private readonly IUnitOfWork _uow;

    public PatchStaffWorkingHoursHandler(IStaffWorkingHoursRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task <StaffWorkingHoursDayDto> Handle(PatchStaffWorkingHoursCommand request, CancellationToken ct)
    {
        var day = (DayOfWeek)request.DayOfWeek;

        var entity = await _repo.GetByStaffAndDayAsync(request.StaffId, day, ct)
            ?? throw new NotFoundException("Working hours not found for this day.");

        // Apply patch (only what user sent)
        if (request.Body.IsDayOff is not null)
            entity.IsDayOff = request.Body.IsDayOff.Value;

        if (request.Body.StartTime is not null)
            entity.StartTime = ParseTime(request.Body.StartTime);

        if (request.Body.EndTime is not null)
            entity.EndTime = ParseTime(request.Body.EndTime);

        // Validate logical constraints (only when open)
        if (!entity.IsDayOff && entity.EndTime <= entity.StartTime)
            throw new ConflictException("EndTime must be after StartTime when day is open.");

        await _uow.SaveChangesAsync(ct);

        return new StaffWorkingHoursDayDto(
            entity.StaffId,
            (int)entity.DayOfWeek, 
            entity.StartTime.ToString("HH:mm:ss"), 
            entity.EndTime.ToString("HH:mm:ss"), 
            entity.IsDayOff);
    }

    private static TimeOnly ParseTime(string value)
        => TimeOnly.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture);
}
