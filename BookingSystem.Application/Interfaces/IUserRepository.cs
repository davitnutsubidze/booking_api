using Booking.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct = default);
    Task<User?> FindByTenantAndEmailOrPhoneAsync(
    Guid tenantId,
    string? email,
    string phone,
    CancellationToken ct);

}
