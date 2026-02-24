using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.BlockedTimes;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class BlockedTimeCrudRepository : IBlockedTimeCrudRepository
{
    private readonly AppDbContext _db;
    public BlockedTimeCrudRepository(AppDbContext db) => _db = db;

    public async Task<List<BlockedTime>> GetRangeAsync(Guid tenantId, Guid? staffId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
    {
        var query = _db.BlockedTimes
            .AsNoTracking()
            .Where(x =>
                x.TenantId == tenantId &&
                x.StartDateTimeUtc < toUtc &&
                x.EndDateTimeUtc > fromUtc);

        // თუ staffId მითითებულია → მივიღოთ staff-specific + tenant-wide (StaffId == null)
        if (staffId is not null)
        {
            var sid = staffId.Value;
            query = query.Where(x => x.StaffId == null || x.StaffId == sid);
        }

        return await query.OrderBy(x => x.StartDateTimeUtc).ToListAsync(ct);
    }


    public Task<List<BlockedTimeDto>> GetRangeWithStaffNameAsync(Guid tenantId, Guid? staffId, DateTime fromUtc, DateTime toUtc, CancellationToken ct)
    {
        var q =
           from bt in _db.BlockedTimes.AsNoTracking()

           join s in _db.Staff.AsNoTracking()
               on bt.StaffId equals s.Id into staffJoin

           from staff in staffJoin.DefaultIfEmpty()

           where bt.TenantId == tenantId
                 && bt.StartDateTimeUtc < toUtc
                 && bt.EndDateTimeUtc > fromUtc
                 && (staffId == null || bt.StaffId == staffId)

           orderby bt.StartDateTimeUtc

           select new BlockedTimeDto(
               bt.Id,
               bt.TenantId,
               bt.StaffId,
               staff != null
                   ? staff.FirstName + " " + staff.LastName
                   : null,
               bt.StartDateTimeUtc,
               bt.EndDateTimeUtc,
               bt.Reason
           );

        return q.ToListAsync(ct);
    }



    public Task<BlockedTime?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.BlockedTimes.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(BlockedTime entity, CancellationToken ct = default)
        => _db.BlockedTimes.AddAsync(entity, ct).AsTask();

    public void Remove(BlockedTime entity)
        => _db.BlockedTimes.Remove(entity);

}
