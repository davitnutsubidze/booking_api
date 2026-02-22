namespace BookingSystem.Application.DTOs.WorkingHours;

public sealed record WorkingHoursDayDto(
    int DayOfWeek,          // 0..6 (Sunday=0)
    string StartTime,       // "09:00"
    string EndTime,         // "18:00"
    bool IsClosed
);
