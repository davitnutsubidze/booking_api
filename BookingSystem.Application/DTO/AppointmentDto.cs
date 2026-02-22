using Booking.Domain.Enums;

namespace BookingSystem.Application.DTOs;

public sealed record AppointmentDto(
    Guid Id,
    Guid TenantId,
    Guid ServiceId,
    Guid StaffId,
    Guid CustomerId,
    DateTime StartDateTimeUtc,
    DateTime EndDateTimeUtc,
    AppointmentStatus Status,
    string? Notes
);
