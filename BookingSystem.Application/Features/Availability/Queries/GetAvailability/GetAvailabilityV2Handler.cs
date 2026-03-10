using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Features.Availability.Queries.GetAvailabilityV2;
using BookingSystem.Application.Interfaces;
using MediatR;

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
        if (request.SlotMinutes is < 5 or > 400)
            throw new ConflictException("SlotMinutes must be between 5 and 400.");

        var service = await _services.GetByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service not found.");

        if (service.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        // Time zone
        var tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tbilisi");

        // staff list selection
        List<Booking.Domain.Entities.Staff> staffs;

        if (request.StaffId is Guid staffId)
        {
            var one = await _staff.GetByIdAsync(staffId, ct)
                ?? throw new NotFoundException("Staff not found.");

            staffs = new() { one };
        }
        else
        {
            // დაგჭირდება repo მეთოდი: GetActiveByTenantAsync(...)
            staffs = (await _staff.GetActiveByTenantAsync(request.TenantId, request.ServiceId, ct)).ToList();
        }

        // optional: თუ staffId==null და staff არ მოიძებნა
        if (staffs.Count == 0)
            return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, service.DurationMinutes, new List<AvailabilityByStaffDto>());

        var durationMinutes = service.DurationMinutes;

        var results = new List<AvailabilityByStaffDto>(staffs.Count);

        foreach (var s in staffs)
        {
            if (!s.IsActive)
            {
                results.Add(new AvailabilityByStaffDto(s.Id, s.FullName, Array.Empty<AvailabilitySlotDto>()));
                continue;
            }

            var slots = await GetSlotsForStaffAsync(
                request.TenantId,
                s.Id,
                request.DateUtc,
                request.SlotMinutes,
                durationMinutes,
                tz,
                ct);

            results.Add(new AvailabilityByStaffDto(s.Id, s.FullName, slots));
        }

        return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, durationMinutes, results);
    }

    private async Task<IReadOnlyList<AvailabilitySlotDto>> GetSlotsForStaffAsync(
        Guid tenantId,
        Guid staffId,
        DateOnly date,
        int slotMinutes,
        int durationMinutes,
        TimeZoneInfo tz,
        CancellationToken ct)
    {
        var day = date.DayOfWeek;

        // 1) working hours
        var staffHours = await _hours.GetStaffHoursAsync(staffId, day, ct);
        if (staffHours is not null && staffHours.IsDayOff)
            return Array.Empty<AvailabilitySlotDto>();

        var tenantHours = await _hours.GetTenantHoursAsync(tenantId, day, ct);
        if (staffHours is null)
        {
            if (tenantHours is null || tenantHours.IsClosed)
                return Array.Empty<AvailabilitySlotDto>();
        }

        var startTime = staffHours?.StartTime ?? tenantHours!.StartTime;
        var endTime = staffHours?.EndTime ?? tenantHours!.EndTime;

        var workStartLocal = date.ToDateTime(startTime, DateTimeKind.Unspecified);
        var workEndLocal = date.ToDateTime(endTime, DateTimeKind.Unspecified);

        var workStartUtc = TimeZoneInfo.ConvertTimeToUtc(workStartLocal, tz);
        var workEndUtc = TimeZoneInfo.ConvertTimeToUtc(workEndLocal, tz);

        if (workEndUtc <= workStartUtc)
            return Array.Empty<AvailabilitySlotDto>();

        // 2) busy intervals
        var appts = await _appointments.GetStaffRangeAsync(staffId, workStartUtc, workEndUtc, ct);

        var blocked = await _blocked.GetRangeAsync(
            tenantId,
            staffId,
            workStartUtc,
            workEndUtc,
            ct);

        var busy = new List<(DateTime Start, DateTime End)>();

        busy.AddRange(appts.Select(a => (a.StartDateTime, a.EndDateTime)));
        busy.AddRange(blocked.Select(b => (b.StartDateTimeUtc, b.EndDateTimeUtc)));

        // 2.1) lunch
        var lunch = await _lunch.GetByStaffAndDayAsync(staffId, day, ct);

        if (lunch is not null && lunch.IsEnabled)
        {
            var lunchStartUtc = TimeZoneInfo.ConvertTimeToUtc(
                date.ToDateTime(lunch.StartTime, DateTimeKind.Unspecified), tz);

            var lunchEndUtc = TimeZoneInfo.ConvertTimeToUtc(
                date.ToDateTime(lunch.EndTime, DateTimeKind.Unspecified), tz);

            if (lunchEndUtc > lunchStartUtc)
            {
                var clippedStart = lunchStartUtc < workStartUtc ? workStartUtc : lunchStartUtc;
                var clippedEnd = lunchEndUtc > workEndUtc ? workEndUtc : lunchEndUtc;

                if (clippedEnd > clippedStart)
                    busy.Add((clippedStart, clippedEnd));
            }
        }

        // 3) generate slots
        var slots = new List<AvailabilitySlotDto>();

        for (var slotStart = workStartUtc;
             slotStart.AddMinutes(durationMinutes) <= workEndUtc;
             slotStart = slotStart.AddMinutes(slotMinutes))
        {
            var slotEnd = slotStart.AddMinutes(durationMinutes);

            var overlaps = busy.Any(i => i.Start < slotEnd && i.End > slotStart);
            if (!overlaps)
                slots.Add(new AvailabilitySlotDto(slotStart, slotEnd));
        }

        return slots;
    }
}