using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence.Repositories;

public sealed class ServiceCrudRepository : IServiceCrudRepository
{
    private readonly AppDbContext _db;
    public ServiceCrudRepository(AppDbContext db) => _db = db;

    public Task<List<ServiceDto>> GetByTenantAsync(Guid tenantId,CancellationToken ct = default)
        => _db.Services
            .AsNoTracking()
            .Where(s => s.TenantId == tenantId)
            .OrderBy(s => s.Name)
            .Select(s => new ServiceDto(
            s.Id,
            s.TenantId,
            s.Name,
            s.Description,
            s.DurationMinutes,
            s.Price,
            s.Currency,
            s.IsActive,
            s.StaffServices.Select(ss => ss.StaffId).Distinct().ToList(),
            null
        ))
            .ToListAsync(ct);
    public Task<List<ServiceDto>> GetByTenantAndStaffAsync(Guid tenantId, Guid? staffId, CancellationToken ct = default)
    => _db.Services
        .AsNoTracking()
        .Where(s => s.TenantId == tenantId &&
                    s.StaffServices.Any(ss => ss.StaffId == staffId))
        .OrderBy(s => s.Name)
         .Select(s => new ServiceDto(
                s.Id,
                s.TenantId,
                s.Name,
                s.Description,
                s.DurationMinutes,
                s.Price,
                s.Currency,
                s.IsActive,
                s.StaffServices.Select(ss => ss.StaffId).ToList(), // StaffIds
                s.StaffServices.Any(ss => ss.StaffId == staffId)   // AssignedToStaff
            ))
        .ToListAsync(ct);

    public Task<List<ServiceDto>> GetAllWithAssignmentAsync(
        Guid tenantId,
        Guid staffId,
        CancellationToken ct = default)
    {
        return _db.Services
            .AsNoTracking()
            .Where(s => s.TenantId == tenantId)
            .OrderBy(s => s.Name)
            .Select(s => new ServiceDto(
                s.Id,
                s.TenantId,
                s.Name,
                s.Description,
                s.DurationMinutes,
                s.Price,
                s.Currency,
                s.IsActive,
                s.StaffServices.Select(ss => ss.StaffId).ToList(), // StaffIds
                s.StaffServices.Any(ss => ss.StaffId == staffId)   // AssignedToStaff
            ))
            .ToListAsync(ct);
    }

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Services.FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task AddAsync(Service service, CancellationToken ct = default)
        => _db.Services.AddAsync(service, ct).AsTask();

    public void Remove(Service service) => _db.Services.Remove(service);
}
