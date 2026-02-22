using Booking.Domain.Entities;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IBlockedTimeRepository
{
    Task<List<BlockedTime>> GetRangeAsync(
        Guid tenantId,
        Guid? staffId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct);
}
