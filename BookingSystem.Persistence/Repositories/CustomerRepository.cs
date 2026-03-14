using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.Customers;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db) => _db = db;

    public Task<Customer?> FindByPhoneAsync(Guid tenantId, string phone, CancellationToken ct = default)
        => _db.Customers.FirstOrDefaultAsync(x => x.Phone == phone, ct);

    public Task AddAsync(Customer customer, CancellationToken ct = default)
        => _db.Customers.AddAsync(customer, ct).AsTask();

    public async Task<List<CustomerDto>> GetByTenantIdAsync(Guid tenantId, CancellationToken ct = default)
    {
        return await _db.CustomerTenants
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .Select(x => new CustomerDto(
                x.Customer.Id,
                x.Customer.UserId,
                x.Customer.FirstName,
                x.Customer.LastName,
                x.Customer.Phone,
                x.Customer.Email,
                x.Customer.Notes,
                x.FirstVisitAt,
                x.LastVisitAt,
                x.IsBlocked
            ))
            .ToListAsync(ct);
    }
}
