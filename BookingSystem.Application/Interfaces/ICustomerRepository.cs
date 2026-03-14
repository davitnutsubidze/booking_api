using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.Customers;

namespace BookingSystem.Application.Interfaces;
public interface ICustomerRepository
{
    Task<Customer?> FindByPhoneAsync(Guid tenantId, string phone, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);

    Task<List<CustomerDto>> GetByTenantIdAsync(Guid tenantId, CancellationToken ct = default);

}
