using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.Services;

namespace BookingSystem.Application.Interfaces;

public interface ITenantRepository
{


    Task<List<Tenant>> GetAllAsync(CancellationToken ct = default);
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default);

    Task AddAsync(Tenant tenant, CancellationToken ct = default);
    Task UpdateAsync(Tenant tenant, CancellationToken ct = default);
    Task DeleteAsync(Tenant tenant, CancellationToken ct = default);
}
