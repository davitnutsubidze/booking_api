namespace BookingSystem.Application.DTOs.Customers;

public sealed record CustomerDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string Phone,
    string? Email,
    string? Notes,
    DateTime FirstVisitAt,
    DateTime? LastVisitAt,
    bool IsBlocked
);