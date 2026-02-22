using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IWorkingHoursCrudRepository
{
    Task<List<TenantWorkingHours>> GetTenantWeekAsync(Guid tenantId, CancellationToken ct = default);
    Task ReplaceTenantWeekAsync(Guid tenantId, List<TenantWorkingHours> items, CancellationToken ct = default);

    Task<List<StaffWorkingHours>> GetStaffWeekAsync(Guid staffId, CancellationToken ct = default);
    Task ReplaceStaffWeekAsync(Guid staffId, List<StaffWorkingHours> items, CancellationToken ct = default);

    Task<TenantWorkingHours?> GetBusinessDayAsync(Guid tenantId, DayOfWeek dayOfWeek, CancellationToken ct = default);
}
