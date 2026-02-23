namespace BookingSystem.Application.DTO.WorkingHours.Staff;

public sealed record CreateStaffWorkingHoursDayDto(
    int DayOfWeek,
    string StartTime,
    string EndTime,
    bool IsDayOff
);