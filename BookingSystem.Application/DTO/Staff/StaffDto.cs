namespace BookingSystem.Application.DTO.Staff;

public sealed record StaffDto(
    Guid Id,
    Guid TenantId,
    string FirstName,
    string LastName,
    string? Phone,
    string? Bio,
    bool IsActive,
    IReadOnlyList<Guid> ServiceIds
);
