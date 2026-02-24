namespace BookingSystem.Application.DTOs.LunchBreaks;

public sealed record UpsertStaffLunchBreakDto(
    string StartTime,   // "HH:mm"
    string EndTime,     // "HH:mm"
    bool IsEnabled
);