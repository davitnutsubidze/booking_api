using MediatR;

namespace BookingSystem.Application.Features.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(
    Guid TenantId,
    Guid ServiceId,
    Guid StaffId,
    string CustomerFirstName,
    string CustomerLastName,
    string CustomerPhone,
    string? CustomerEmail,
    DateTime StartDateTimeUtc,
    string? Notes
) : IRequest<Guid>;
