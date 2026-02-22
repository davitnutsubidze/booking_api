namespace BookingSystem.Application.DTOs.BlockedTimes;

public sealed record CreateBlockedTimeRequestDto(
    Guid? StaffId,
    DateTime StartUtc,
    DateTime EndUtc,
    string? Reason
);
