using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class BlockedTimeRepository : IBlockedTimeRepository
{
    private readonly AppDbContext _db;

    public BlockedTimeRepository(AppDbContext db) => _db = db;

    public async Task<List<BlockedTime>> GetRangeAsync(
        Guid tenantId,
        Guid? staffId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct = default)
    {
        var query = _db.BlockedTimes
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId
                        && x.StartDateTimeUtc < toUtc
                        && x.EndDateTimeUtc > fromUtc);

        // თუ staffId გადმოგვეცა → ვაბრუნებთ:
        // 1) ამ staff-ის blocked times
        // 2) tenant-wide blocked times (StaffId == null)
        if (staffId is not null)
        {
            var sid = staffId.Value;
            query = query.Where(x => x.StaffId == null || x.StaffId == sid);
        }

        return await query
            .OrderBy(x => x.StartDateTimeUtc)
            .ToListAsync(ct);
    }
}
