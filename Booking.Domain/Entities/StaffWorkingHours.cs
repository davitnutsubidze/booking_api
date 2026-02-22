using Booking.Domain.Common;

namespace BookingSystem.Domain.Entities;

public class StaffWorkingHours : BaseEntity
{
    public Guid StaffId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public bool IsDayOff { get; set; }
}
