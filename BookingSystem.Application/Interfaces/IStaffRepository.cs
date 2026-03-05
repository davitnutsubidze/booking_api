using Booking.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IStaffRepository
{
    Task<List<Staff>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default);
    Task<Staff?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<string?> GetStaffNameAsync(Guid tenantId, Guid staffId, CancellationToken ct = default);

    Task<IReadOnlyList<Staff>> GetActiveByTenantAsync(Guid tenantId, Guid serviceId, CancellationToken ct);
    Task AddAsync(Staff staff, CancellationToken ct = default);
    void Remove(Staff staff);
}
