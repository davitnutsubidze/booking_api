using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.BlockedTimes;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IBlockedTimeCrudRepository
{
    Task<List<BlockedTime>> GetRangeAsync(Guid tenantId, Guid? staffId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);

    Task<List<BlockedTimeDto>> GetRangeWithStaffNameAsync(Guid tenantId, Guid? staffId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);

    Task<BlockedTime?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(BlockedTime entity, CancellationToken ct = default);

    void Remove(BlockedTime entity);
}
