using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _db;

    public TenantRepository(AppDbContext db) => _db = db;

    public Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default)
        => _db.Tenant.FirstOrDefaultAsync(x => x.Slug == slug, ct);

    public  Task<List<Tenant>> GetAllAsync(CancellationToken ct = default)
        => _db.Tenant.AsNoTracking().ToListAsync(ct);

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default)
        =>_db.Tenant.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default)
        => _db.Tenant.AnyAsync(x => x.Slug == slug, ct);

    public async Task AddAsync(Tenant tenant, CancellationToken ct = default)
    {
        await _db.Tenant.AddAsync(tenant, ct);
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken ct = default)
    {
        _db.Tenant.Update(tenant);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Tenant tenant, CancellationToken ct = default)
    {
        _db.Tenant.Remove(tenant);
        await _db.SaveChangesAsync(ct);
    }

}
