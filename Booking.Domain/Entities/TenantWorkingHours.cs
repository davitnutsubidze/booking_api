using Booking.Domain.Common;

namespace BookingSystem.Domain.Entities;

public class TenantWorkingHours : BaseEntity
{
    public Guid TenantId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public bool IsClosed { get; set; }
}
