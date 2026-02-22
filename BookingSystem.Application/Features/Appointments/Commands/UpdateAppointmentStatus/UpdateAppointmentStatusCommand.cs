using Booking.Domain.Enums;
using MediatR;

namespace BookingSystem.Application.Features.Appointments.Commands.UpdateAppointmentStatus;

public sealed record UpdateAppointmentStatusCommand(
    Guid Id,
    AppointmentStatus Status
) : IRequest;
