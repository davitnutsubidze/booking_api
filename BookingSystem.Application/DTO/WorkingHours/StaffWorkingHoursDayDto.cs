namespace BookingSystem.Application.DTOs.WorkingHours;

public sealed record StaffWorkingHoursDayDto(
    int DayOfWeek,          // 0..6
    string StartTime,       // "10:00"
    string EndTime,         // "16:00"
    bool IsDayOff
);
