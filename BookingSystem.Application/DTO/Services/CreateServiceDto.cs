namespace BookingSystem.Application.DTOs.Services;

public sealed record CreateServiceDto(
    string Name,
    string? Description,
    int DurationMinutes,
    decimal? Price,
    string? Currency,
    bool IsActive = true
);
