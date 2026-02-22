using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class WorkingHoursCrudRepository : IWorkingHoursCrudRepository
{
    private readonly AppDbContext _db;
    public WorkingHoursCrudRepository(AppDbContext db) => _db = db;

    public Task<List<TenantWorkingHours>> GetTenantWeekAsync(Guid tenantId, CancellationToken ct = default)
        => _db.TenantWorkingHours
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);

    public async Task ReplaceTenantWeekAsync(Guid tenantId, List<TenantWorkingHours> items, CancellationToken ct = default)
    {
        var existing = await _db.TenantWorkingHours.Where(x => x.TenantId == tenantId).ToListAsync(ct);
        _db.TenantWorkingHours.RemoveRange(existing);
        await _db.TenantWorkingHours.AddRangeAsync(items, ct);
    }

    public Task<List<StaffWorkingHours>> GetStaffWeekAsync(Guid staffId, CancellationToken ct = default)
        => _db.StaffWorkingHours
            .AsNoTracking()
            .Where(x => x.StaffId == staffId)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);

    public async Task ReplaceStaffWeekAsync(Guid staffId, List<StaffWorkingHours> items, CancellationToken ct = default)
    {
        var existing = await _db.StaffWorkingHours.Where(x => x.StaffId == staffId).ToListAsync(ct);
        _db.StaffWorkingHours.RemoveRange(existing);
        await _db.StaffWorkingHours.AddRangeAsync(items, ct);
    }

    public Task<TenantWorkingHours?> GetBusinessDayAsync(Guid tenantId, DayOfWeek dayOfWeek, CancellationToken ct = default)
    => _db.TenantWorkingHours
        .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DayOfWeek == dayOfWeek, ct);
}
