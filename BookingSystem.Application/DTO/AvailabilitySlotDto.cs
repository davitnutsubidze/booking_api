namespace BookingSystem.Application.DTOs;

public sealed record AvailabilitySlotDto(DateTime StartUtc, DateTime EndUtc);
