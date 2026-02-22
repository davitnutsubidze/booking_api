using Booking.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> FindByPhoneAsync(Guid tenantId, string phone, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
}
