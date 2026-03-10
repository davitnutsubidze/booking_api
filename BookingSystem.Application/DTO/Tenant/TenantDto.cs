using Booking.Domain.Enums;

namespace BookingSystem.Application.DTOs.Tenant;

public sealed record TenantDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string Phone,
    string Email,
    string Address,
    string TimeZone,
    bool IsActive
);