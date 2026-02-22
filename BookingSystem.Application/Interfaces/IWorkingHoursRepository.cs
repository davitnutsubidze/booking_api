using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IWorkingHoursRepository
{
    Task<TenantWorkingHours?> GetTenantHoursAsync(Guid tenantId, DayOfWeek day, CancellationToken ct);
    Task<StaffWorkingHours?> GetStaffHoursAsync(Guid staffId, DayOfWeek day, CancellationToken ct);
}
