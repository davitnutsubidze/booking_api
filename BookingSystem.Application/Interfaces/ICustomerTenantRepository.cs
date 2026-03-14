
namespace BookingSystem.Application.Interfaces
{
    public interface ICustomerTenantRepository
    {
        Task<bool> ExistsAsync(Guid customerId, Guid tenantId, CancellationToken ct);
        Task AddAsync(CustomerTenant entity, CancellationToken ct);
    }
}
