namespace BookingSystem.Application.DTOs.BlockedTimes;

public sealed record BlockedTimeDto(
    Guid Id,
    Guid TenantId,
    Guid? StaffId,
    string? StaffName,
    DateTime StartUtc,
    DateTime EndUtc,
    string? Reason
);
