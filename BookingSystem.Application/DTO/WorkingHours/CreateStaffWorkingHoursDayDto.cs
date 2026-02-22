namespace BookingSystem.Application.DTOs.WorkingHours;

public sealed record CreateStaffWorkingHoursDayDto(
    int DayOfWeek,
    string StartTime,
    string EndTime,
    bool IsDayOff
);