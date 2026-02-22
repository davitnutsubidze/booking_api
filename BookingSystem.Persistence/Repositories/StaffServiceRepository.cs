using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class StaffServiceRepository : IStaffServiceRepository
{
    private readonly AppDbContext _db;
    public StaffServiceRepository(AppDbContext db) => _db = db;

    public Task<List<StaffService>> GetByStaffIdAsync(Guid staffId, CancellationToken ct = default)
        => _db.StaffServices
            .AsNoTracking()
            .Where(x => x.StaffId == staffId)
            .ToListAsync(ct);

    public async Task ReplaceStaffServicesAsync(Guid staffId, IEnumerable<Guid> serviceIds, CancellationToken ct = default)
    {
        var existing = await _db.StaffServices
            .Where(x => x.StaffId == staffId)
            .ToListAsync(ct);

        _db.StaffServices.RemoveRange(existing);

        var rows = serviceIds.Select(serviceId => new StaffService
        {
            StaffId = staffId,
            ServiceId = serviceId
        });

        await _db.StaffServices.AddRangeAsync(rows, ct);
    }


    public Task<bool> ExistsAsync(Guid staffId, Guid serviceId, CancellationToken ct = default)
        => _db.StaffServices.AnyAsync(x => x.StaffId == staffId && x.ServiceId == serviceId, ct);

    public Task AddAsync(StaffService entity, CancellationToken ct = default)
        => _db.StaffServices.AddAsync(entity, ct).AsTask();

    public async Task RemoveAsync(Guid staffId, Guid serviceId, CancellationToken ct = default)
    {
        var row = await _db.StaffServices.FirstOrDefaultAsync(
            x => x.StaffId == staffId && x.ServiceId == serviceId, ct);

        if (row is not null)
            _db.StaffServices.Remove(row);
    }
}
