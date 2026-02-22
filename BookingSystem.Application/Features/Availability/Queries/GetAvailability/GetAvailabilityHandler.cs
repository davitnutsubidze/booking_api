using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Availability.Queries.GetAvailability;

public sealed class GetAvailabilityHandler : IRequestHandler<GetAvailabilityQuery, AvailabilityResponseDto>
{
    private readonly IAppointmentRepository _appointments;
    private readonly IServiceRepository _services;

    public GetAvailabilityHandler(IAppointmentRepository appointments, IServiceRepository services)
    {
        _appointments = appointments;
        _services = services;
    }

    public async Task<AvailabilityResponseDto> Handle(GetAvailabilityQuery request, CancellationToken ct)
    {
        if (request.SlotMinutes is < 5 or > 60)
            throw new ConflictException("SlotMinutes must be between 5 and 60.");

        var service = await _services.GetByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service not found.");

        if (service.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        var duration = service.DurationMinutes;

        // MVP working window: 09:00–18:00 UTC
        var dayStart = request.DateUtc.ToDateTime(new TimeOnly(9, 0), DateTimeKind.Utc);
        var dayEnd = request.DateUtc.ToDateTime(new TimeOnly(18, 0), DateTimeKind.Utc);

        // მოიტანე ამ staff-ის existing appointments ამ დღისთვის
        // (range filter: Start < dayEnd && End > dayStart)
        var busy = await _appointments.GetStaffRangeAsync(request.StaffId, dayStart, dayEnd, ct);

        // Generate slots
        var slots = new List<AvailabilitySlotDto>();
        for (var start = dayStart; start.AddMinutes(duration) <= dayEnd; start = start.AddMinutes(request.SlotMinutes))
        {
            var end = start.AddMinutes(duration);

            var overlaps = busy.Any(a => a.StartDateTime < end && a.EndDateTime > start);
            if (!overlaps)
                slots.Add(new AvailabilitySlotDto(start, end));
        }

        return new AvailabilityResponseDto(request.DateUtc, request.SlotMinutes, duration, slots);
    }
}
