using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Appointments.Commands.UpdateAppointmentStatus;

public sealed class UpdateAppointmentStatusHandler : IRequestHandler<UpdateAppointmentStatusCommand>
{
    private readonly IAppointmentRepository _appointments;
    private readonly IUnitOfWork _uow;

    public UpdateAppointmentStatusHandler(IAppointmentRepository appointments, IUnitOfWork uow)
    {
        _appointments = appointments;
        _uow = uow;
    }

    public async Task Handle(UpdateAppointmentStatusCommand request, CancellationToken ct)
    {
        // repo-ში UpdateStatusAsync ახლა InvalidOperationException ისვრის თუ ვერ იპოვა
        // მოდი აქვე გადავამოწმოთ სუფთად:
        var existing = await _appointments.GetByIdAsync(request.Id, ct);
        if (existing is null)
            throw new NotFoundException("Appointment not found.");

        existing.Status = request.Status;
        await _uow.SaveChangesAsync(ct);
    }
}
