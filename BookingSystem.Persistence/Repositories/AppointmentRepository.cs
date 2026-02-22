using Booking.Domain.Entities;
using Booking.Domain.Enums;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Appointment appointment, CancellationToken ct = default)
    {
        await _db.Appointments.AddAsync(appointment, ct);
    }

    public Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return _db.Appointments.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<bool> HasOverlapAsync(Guid staffId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default)
    {
        // overlap rule: existing.Start < newEnd AND existing.End > newStart
        return _db.Appointments.AnyAsync(a =>
            a.StaffId == staffId &&
            a.Status != Booking.Domain.Enums.AppointmentStatus.Cancelled &&
            a.StartDateTime < endUtc &&
            a.EndDateTime > startUtc,
            ct);
    }

    public async Task<List<Appointment>> GetByBusinessRangeAsync(
    Guid tenantId,
    DateTime fromUtc,
    DateTime toUtc,
    CancellationToken ct = default)
    {
        return await _db.Appointments
            .Where(a => a.TenantId == tenantId &&
                        a.StartDateTime < toUtc &&
                        a.EndDateTime > fromUtc)
            .OrderBy(a => a.StartDateTime)
            .ToListAsync(ct);
    }

    public async Task UpdateStatusAsync(Guid id, AppointmentStatus status, CancellationToken ct = default)
    {
        var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (appointment is null)
            throw new InvalidOperationException("Appointment not found.");

        appointment.Status = status;
    }

    public async Task<List<Appointment>> GetStaffRangeAsync(
    Guid staffId,
    DateTime fromUtc,
    DateTime toUtc,
    CancellationToken ct = default)
    {
        return await _db.Appointments
            .Where(a => a.StaffId == staffId &&
                        a.StartDateTime < toUtc &&
                        a.EndDateTime > fromUtc &&
                        a.Status != Booking.Domain.Enums.AppointmentStatus.Cancelled)
            .OrderBy(a => a.StartDateTime)
            .ToListAsync(ct);
    }

}
