namespace BookingSystem.Application.DTO.Staff;

public sealed record CreateStaffDto(
    string FirstName,
    string LastName,
    string? Phone,
    string? Bio,
    bool IsActive = true
);
