using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BookingSystem.Persistence.Repositories
{
    public sealed class CustomerTenantRepository : ICustomerTenantRepository
    {
        private readonly AppDbContext _db;
        public CustomerTenantRepository(AppDbContext db) => _db = db;

        public Task<bool> ExistsAsync(Guid customerId, Guid tenantId, CancellationToken ct)
        {
            return _db.CustomerTenants
                .AnyAsync(x => x.CustomerId == customerId && x.TenantId == tenantId, ct);
        }

        public async Task AddAsync(CustomerTenant entity, CancellationToken ct)
        {
            await _db.CustomerTenants.AddAsync(entity, ct);
        }
    }
}
