using Booking.Domain.Common;
using Booking.Domain.Entities;

public sealed class StaffLunchBreak : BaseEntity
{

    public Guid TenantId { get; set; }
    public Guid StaffId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }   // 0..6
    public TimeOnly StartTime { get; set; }    // local სამუშაო დრო
    public TimeOnly EndTime { get; set; }

    public bool IsEnabled { get; set; } = true;

    public Staff Staff { get; set; } = default!;
}