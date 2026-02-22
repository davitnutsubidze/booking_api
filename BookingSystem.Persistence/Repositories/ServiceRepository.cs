using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _db;

    public ServiceRepository(AppDbContext db) => _db = db;

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Services.FirstOrDefaultAsync(x => x.Id == id, ct);
}
