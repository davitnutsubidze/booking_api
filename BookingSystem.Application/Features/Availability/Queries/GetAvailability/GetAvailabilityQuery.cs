using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Availability.Queries.GetAvailability;

public sealed record GetAvailabilityQuery(
    Guid TenantId,
    Guid StaffId,
    Guid ServiceId,
    DateOnly DateUtc,
    int SlotMinutes = 15
) : IRequest<AvailabilityResponseDto>;
