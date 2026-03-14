using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;


    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _db.User.AddAsync(user, ct);
    }

    public Task<User?> FindByTenantAndEmailOrPhoneAsync(
    Guid tenantId,
    string? email,
    string phone,
    CancellationToken ct)
    {
        return _db.User
            .FirstOrDefaultAsync(x =>
                x.TenantId == tenantId &&
                (
                    x.Phone == phone ||
                    (!string.IsNullOrEmpty(email) && x.Email == email)
                ),
                ct);
    }
}
