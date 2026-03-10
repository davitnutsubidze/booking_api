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
}
