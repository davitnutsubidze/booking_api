namespace BookingSystem.Application.DTOs;

public sealed record AvailabilityResponseDto(
    DateOnly DateUtc,
    int SlotMinutes,
    int DurationMinutes,
    List<AvailabilitySlotDto> Slots
);
