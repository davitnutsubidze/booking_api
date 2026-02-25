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
    private readonly IStaffLunchBreakRepository _lunch;
    private readonly IStaffRepository _staff;

    public GetAvailabilityV2Handler(
        IServiceRepository services,
        IWorkingHoursRepository hours,
        IBlockedTimeRepository blocked,
        IAppointmentRepository appointments,
        IStaffLunchBreakRepository lunch,
        IStaffRepository staff)
    {
        _services = services;
        _hours = hours;
        _blocked = blocked;
        _appointments = appointments;
        _lunch = lunch;
        _staff = staff;
    }

    public async Task<AvailabilityResponseDto> Handle(GetAvailabilityV2Query request, CancellationToken ct)
    {
        if (request.SlotMinutes is < 5 or > 60)
            throw new ConflictException("SlotMinutes must be between 5 and 60.");

        var service = await _services.GetByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service not found.");

        //  Staff check (exists + tenant + active)
        var staff = await _staff.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (service.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        if (!staff.IsActive)
            return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, service.DurationMinutes, new());

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

        // convert DateOnly + TimeOnly (local Time) -> convert local time to DateTime UTC  -> 
        var tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tbilisi");
        var workStartLocal = request.DateUtc.ToDateTime(startTime, DateTimeKind.Unspecified);
        var workEndLocal = request.DateUtc.ToDateTime(endTime, DateTimeKind.Unspecified);

        var workStartUtc = TimeZoneInfo.ConvertTimeToUtc(workStartLocal, tz);
        var workEndUtc = TimeZoneInfo.ConvertTimeToUtc(workEndLocal, tz);

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


        // 2.1) Staff lunch break for that day (weekly recurring)
        var lunch = await _lunch.GetByStaffAndDayAsync(request.StaffId, day, ct);

        if (lunch is not null && lunch.IsEnabled)
        {
            // convert DateOnly + TimeOnly (local Time) -> convert local time to DateTime UTC  -> 
            var lunchStartLocal = request.DateUtc.ToDateTime(lunch.StartTime, DateTimeKind.Unspecified);
            var lunchEndLocal = request.DateUtc.ToDateTime(lunch.EndTime, DateTimeKind.Unspecified);

            var lunchStartUtc = TimeZoneInfo.ConvertTimeToUtc(lunchStartLocal, tz);
            var lunchEndUtc = TimeZoneInfo.ConvertTimeToUtc(lunchEndLocal, tz);

            if (lunchEndUtc > lunchStartUtc)
            {
                // Clip lunch to working window (so it doesn't block outside work hours)
                var clippedStart = lunchStartUtc < workStartUtc ? workStartUtc : lunchStartUtc;
                var clippedEnd = lunchEndUtc > workEndUtc ? workEndUtc : lunchEndUtc;

                if (clippedEnd > clippedStart)
                    busyIntervals.Add((clippedStart, clippedEnd));
            }
        }

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
