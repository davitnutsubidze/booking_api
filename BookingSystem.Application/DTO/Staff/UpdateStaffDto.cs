namespace BookingSystem.Application.DTO.Staff;

public sealed record UpdateStaffDto(
    string FirstName,
    string LastName,
    string? Phone,
    string? Bio,
    bool IsActive
);
