namespace BookingSystem.Application.DTO.WorkingHours.Staff;

public sealed record PatchWorkingHoursDayDto(
    Guid StaffId,
    int DayOfWeek,          // 0..6
    string? StartTime,       // "10:00"
    string? EndTime,         // "16:00"
    bool? IsDayOff
);
