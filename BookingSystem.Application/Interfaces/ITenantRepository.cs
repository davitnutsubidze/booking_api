using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.Services;

namespace BookingSystem.Application.Interfaces;

public interface ITenantRepository
{
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default);
}
