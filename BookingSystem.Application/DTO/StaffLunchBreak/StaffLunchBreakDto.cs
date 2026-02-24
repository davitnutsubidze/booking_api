namespace BookingSystem.Application.DTOs.LunchBreaks;

public sealed record StaffLunchBreakDto(
    Guid Id,
    Guid TenantId,
    Guid StaffId,
    int DayOfWeek,
    string StartTime,
    string EndTime,
    bool IsEnabled
);