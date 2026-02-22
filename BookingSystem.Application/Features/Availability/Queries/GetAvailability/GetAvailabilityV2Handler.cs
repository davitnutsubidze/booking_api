using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;

namespace BookingSystem.Application.Features.Availability.Queries.GetAvailabilityV2;

public sealed class GetAvailabilityV2Handler
    : IRequestHandler<GetAvailabilityV2Query, AvailabilityResponseDto>
{
    private readonly IServiceRepository _services;
    private readonly IWorkingHoursRepository _hours;
    private readonly IBlockedTimeRepository _blocked;
    private readonly IAppointmentRepository _appointments;

    public GetAvailabilityV2Handler(
        IServiceRepository services,
        IWorkingHoursRepository hours,
        IBlockedTimeRepository blocked,
        IAppointmentRepository appointments)
    {
        _services = services;
        _hours = hours;
        _blocked = blocked;
        _appointments = appointments;
    }

    public async Task<AvailabilityResponseDto> Handle(GetAvailabilityV2Query request, CancellationToken ct)
    {
        if (request.SlotMinutes is < 5 or > 60)
            throw new ConflictException("SlotMinutes must be between 5 and 60.");

        var service = await _services.GetByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service not found.");

        if (service.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        var durationMinutes = service.DurationMinutes;

        // 1) Determine working hours window for the day
        var day = request.DateUtc.DayOfWeek;

        // staff override first (if exists)
        var staffHours = await _hours.GetStaffHoursAsync(request.StaffId, day, ct);
        if (staffHours is not null && staffHours.IsDayOff)
        {
            return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, durationMinutes, new());
        }

        // tenant hours fallback
        var businessHours = await _hours.GetTenantHoursAsync(request.TenantId, day, ct);
        if (staffHours is null)
        {
            if (businessHours is null || businessHours.IsClosed)
                return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, durationMinutes, new());
        }

        var startTime = staffHours?.StartTime ?? businessHours!.StartTime;
        var endTime = staffHours?.EndTime ?? businessHours!.EndTime;

        // convert DateOnly + TimeOnly -> DateTime UTC
        var workStartUtc = request.DateUtc.ToDateTime(startTime, DateTimeKind.Utc);
        var workEndUtc = request.DateUtc.ToDateTime(endTime, DateTimeKind.Utc);

        if (workEndUtc <= workStartUtc)
            return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, durationMinutes, new());

        // 2) Load "busy" intervals: appointments + blocked times (merge conceptually)
        var appts = await _appointments.GetStaffRangeAsync(request.StaffId, workStartUtc, workEndUtc, ct);

        var blocked = await _blocked.GetRangeAsync(
            request.TenantId,
            request.StaffId,
            workStartUtc,
            workEndUtc,
            ct);

        // convert both to a common interval list
        var busyIntervals = new List<(DateTime Start, DateTime End)>();

        busyIntervals.AddRange(appts.Select(a => (a.StartDateTime, a.EndDateTime)));

        // IMPORTANT: adjust these property names if your BlockedTime uses StartDateTime/EndDateTime
        busyIntervals.AddRange(blocked.Select(b => (b.StartDateTimeUtc, b.EndDateTimeUtc)));

        // 3) Generate candidate slots, skip if overlaps any busy interval
        var slots = new List<AvailabilitySlotDto>();

        for (var slotStart = workStartUtc;
             slotStart.AddMinutes(durationMinutes) <= workEndUtc;
             slotStart = slotStart.AddMinutes(request.SlotMinutes))
        {
            var slotEnd = slotStart.AddMinutes(durationMinutes);

            var overlaps = busyIntervals.Any(i => i.Start < slotEnd && i.End > slotStart);
            if (!overlaps)
                slots.Add(new AvailabilitySlotDto(slotStart, slotEnd));
        }

        return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, durationMinutes, slots);
    }
}
