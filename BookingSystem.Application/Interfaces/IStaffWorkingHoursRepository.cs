using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IStaffWorkingHoursRepository
{
    Task<bool> ExistsAsync(Guid staffId, DayOfWeek dayOfWeek, CancellationToken ct = default);
    Task AddAsync(StaffWorkingHours entity, CancellationToken ct = default);

    Task<StaffWorkingHours?> GetByStaffAndDayAsync(Guid staffId, DayOfWeek dayOfWeek, CancellationToken ct = default);
    void Remove(StaffWorkingHours entity);
}