using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class WorkingHoursRepository : IWorkingHoursRepository
{
    private readonly AppDbContext _db;

    public WorkingHoursRepository(AppDbContext db) => _db = db;

    public Task<TenantWorkingHours?> GetTenantHoursAsync(
        Guid tenantId,
        DayOfWeek day,
        CancellationToken ct = default)
    {
        return _db.TenantWorkingHours
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DayOfWeek == day, ct);
    }

    public Task<StaffWorkingHours?> GetStaffHoursAsync(
        Guid staffId,
        DayOfWeek day,
        CancellationToken ct = default)
    {
        return _db.StaffWorkingHours
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffId == staffId && x.DayOfWeek == day, ct);
    }
}
