using Booking.Domain.Entities;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IStaffServiceRepository
{
    Task<List<StaffService>> GetByStaffIdAsync(Guid staffId, CancellationToken ct = default);
    Task ReplaceStaffServicesAsync(Guid staffId, IEnumerable<Guid> serviceIds, CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid staffId, Guid serviceId, CancellationToken ct = default);
    Task AddAsync(StaffService entity, CancellationToken ct = default);
    Task RemoveAsync(Guid staffId, Guid serviceId, CancellationToken ct = default);
}
