namespace BookingSystem.Application.DTOs;

public sealed record AvailabilityByStaffDto(
    Guid StaffId,
    string StaffName,
    IReadOnlyList<AvailabilitySlotDto> Slots
);

public sealed record AvailabilityResponseDto(
    DateOnly DateUtc,
    int SlotMinutes,
    int DurationMinutes,
    IReadOnlyList<AvailabilityByStaffDto> Staff // <-- was Slots
);