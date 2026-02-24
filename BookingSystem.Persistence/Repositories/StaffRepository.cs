using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _db;
    public StaffRepository(AppDbContext db) => _db = db;

    public Task<List<Staff>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default)
        => _db.Staff
            .AsNoTracking()
            .Where(s => s.TenantId == tenantId)
            .OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
            .ToListAsync(ct);

    public Task<string?> GetStaffNameAsync(Guid tenantId, Guid staffId, CancellationToken ct = default)
    {
        return _db.Staff
            .AsNoTracking()
            .Where(s => s.TenantId == tenantId && s.Id == staffId)
            .Select(s => s.FirstName + " " + s.LastName)
            .FirstOrDefaultAsync(ct);
    }

    public Task<Staff?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Staff.FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task AddAsync(Staff staff, CancellationToken ct = default)
        => _db.Staff.AddAsync(staff, ct).AsTask();

    public void Remove(Staff staff) => _db.Staff.Remove(staff);
}
