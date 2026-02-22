namespace BookingSystem.Application.DTOs.BlockedTimes;

public sealed record BlockedTimeDto(
    Guid Id,
    Guid TenantId,
    Guid? StaffId,
    DateTime StartUtc,
    DateTime EndUtc,
    string? Reason
);
