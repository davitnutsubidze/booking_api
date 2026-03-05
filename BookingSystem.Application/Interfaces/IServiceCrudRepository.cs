using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.Services;

namespace BookingSystem.Application.Interfaces;

public interface IServiceCrudRepository
{
    Task<List<ServiceDto>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default);

    Task<List<ServiceDto>> GetByTenantAndStaffAsync(Guid tenantId, Guid? staffId, CancellationToken ct = default);

    Task<List<ServiceDto>> GetAllWithAssignmentAsync(
    Guid tenantId,
    Guid staffId,
    CancellationToken ct = default);

    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Service service, CancellationToken ct = default);
    void Remove(Service service);
}
