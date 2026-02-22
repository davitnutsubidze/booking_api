using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Availability.Queries.GetAvailabilityV2;

public sealed record GetAvailabilityV2Query(
    Guid TenantId,
    Guid StaffId,
    Guid ServiceId,
    DateOnly DateUtc,
    int SlotMinutes = 15
) : IRequest<AvailabilityResponseDto>;
