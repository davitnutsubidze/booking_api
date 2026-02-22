using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Appointments.Queries.GetAppointmentsRange;

public sealed record GetAppointmentsRangeQuery(
    Guid TenantId,
    DateTime FromUtc,
    DateTime ToUtc
) : IRequest<List<AppointmentDto>>;
