using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Appointments.Queries.GetAppointmentsRange;

public sealed class GetAppointmentsRangeHandler
    : IRequestHandler<GetAppointmentsRangeQuery, List<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointments;

    public GetAppointmentsRangeHandler(IAppointmentRepository appointments)
        => _appointments = appointments;

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsRangeQuery request, CancellationToken ct)
    {
        var items = await _appointments.GetByBusinessRangeAsync(
            request.TenantId,
            request.FromUtc,
            request.ToUtc,
            ct);

        return items.Select(a => new AppointmentDto(
            a.Id, a.TenantId, a.ServiceId, a.StaffId, a.CustomerId,
            a.StartDateTimeUtc, a.EndDateTimeUtc, a.Status, a.Notes,
            a.StaffName, a.ServiceName
        )).ToList();
    }
}
