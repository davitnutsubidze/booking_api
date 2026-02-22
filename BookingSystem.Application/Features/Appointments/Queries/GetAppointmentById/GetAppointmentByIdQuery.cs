using BookingSystem.Application.DTOs;
using MediatR;

namespace BookingSystem.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(Guid Id) : IRequest<AppointmentDto>;
