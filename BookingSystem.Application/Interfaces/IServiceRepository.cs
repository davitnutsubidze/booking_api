using Booking.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
