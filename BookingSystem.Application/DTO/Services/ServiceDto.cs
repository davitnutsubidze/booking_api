namespace BookingSystem.Application.DTOs.Services;

public sealed record ServiceDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string? Description,
    int DurationMinutes,
    decimal? Price,
    string? Currency,
    bool IsActive,
    bool? AssignedToStaff = null
);
