using Booking.Domain.Entities;
using Booking.Domain.Enums;

namespace BookingSystem.Application.Interfaces;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment, CancellationToken ct = default);

    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> HasOverlapAsync(
        Guid staffId,
        DateTime startUtc,
        DateTime endUtc,
        CancellationToken ct = default);

    Task<List<Appointment>> GetByBusinessRangeAsync(
        Guid tenantId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct = default);

    Task UpdateStatusAsync(Guid id, AppointmentStatus status, CancellationToken ct = default);



    Task<List<Appointment>> GetStaffRangeAsync(
        Guid staffId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct = default);

}
