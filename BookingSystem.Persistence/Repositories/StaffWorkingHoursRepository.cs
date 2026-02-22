using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class StaffWorkingHoursRepository : IStaffWorkingHoursRepository
{
    private readonly AppDbContext _db;
    public StaffWorkingHoursRepository(AppDbContext db) => _db = db;

    public Task<bool> ExistsAsync(Guid staffId, DayOfWeek dayOfWeek, CancellationToken ct = default)
        => _db.StaffWorkingHours.AnyAsync(x => x.StaffId == staffId && x.DayOfWeek == dayOfWeek, ct);

    public Task AddAsync(StaffWorkingHours entity, CancellationToken ct = default)
        => _db.StaffWorkingHours.AddAsync(entity, ct).AsTask();

    public Task<StaffWorkingHours?> GetByStaffAndDayAsync(Guid staffId, DayOfWeek dayOfWeek, CancellationToken ct = default)
    => _db.StaffWorkingHours.FirstOrDefaultAsync(x => x.StaffId == staffId && x.DayOfWeek == dayOfWeek, ct);

    public void Remove(StaffWorkingHours entity)
        => _db.StaffWorkingHours.Remove(entity);
}