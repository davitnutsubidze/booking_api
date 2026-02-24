using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IStaffLunchBreakRepository
{
    Task<StaffLunchBreak?> GetByStaffAndDayAsync(Guid staffId, DayOfWeek dayOfWeek, CancellationToken ct = default);
    Task<List<StaffLunchBreak>> GetByStaffAsync(Guid staffId, CancellationToken ct = default);
    Task AddAsync(StaffLunchBreak entity, CancellationToken ct = default);
    void Remove(StaffLunchBreak entity);
}