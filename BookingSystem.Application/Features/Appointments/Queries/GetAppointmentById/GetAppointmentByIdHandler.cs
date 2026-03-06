using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
{
    private readonly IAppointmentRepository _appointments;

    public GetAppointmentByIdHandler(IAppointmentRepository appointments)
        => _appointments = appointments;

    public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken ct)
    {
        var a = await _appointments.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Appointment not found.");

        return new AppointmentDto(
            a.Id,
            a.TenantId,
            a.ServiceId,
            a.StaffId,
            a.CustomerId,
            a.StartDateTime,
            a.EndDateTime,
            a.Status,
            a.Notes,
            a.Staff.FirstName,
            a.Service.Name
        );
    }
}
