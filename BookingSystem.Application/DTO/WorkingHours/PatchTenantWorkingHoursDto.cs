namespace BookingSystem.Application.DTOs.WorkingHours;

public sealed record PatchTenantWorkingHoursDto(
    string? StartTime,   // "HH:mm"
    string? EndTime,     // "HH:mm"
    bool? IsClosed
);