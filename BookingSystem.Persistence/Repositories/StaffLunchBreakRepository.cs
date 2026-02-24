using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class StaffLunchBreakRepository : IStaffLunchBreakRepository
{
    private readonly AppDbContext _db;
    public StaffLunchBreakRepository(AppDbContext db) => _db = db;

    public Task<StaffLunchBreak?> GetByStaffAndDayAsync(Guid staffId, DayOfWeek dayOfWeek, CancellationToken ct = default)
        => _db.StaffLunchBreaks
            .FirstOrDefaultAsync(x => x.StaffId == staffId && x.DayOfWeek == dayOfWeek, ct);

    public Task<List<StaffLunchBreak>> GetByStaffAsync(Guid staffId, CancellationToken ct = default)
        => _db.StaffLunchBreaks
            .Where(x => x.StaffId == staffId)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);

    public Task AddAsync(StaffLunchBreak entity, CancellationToken ct = default)
        => _db.StaffLunchBreaks.AddAsync(entity, ct).AsTask();

    public void Remove(StaffLunchBreak entity)
        => _db.StaffLunchBreaks.Remove(entity);
}