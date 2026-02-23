namespace BookingSystem.Application.DTO.WorkingHours.Tenant;

public sealed record PatchTenantWorkingHoursDto(
    string? StartTime,   // "HH:mm"
    string? EndTime,     // "HH:mm"
    bool? IsClosed
);