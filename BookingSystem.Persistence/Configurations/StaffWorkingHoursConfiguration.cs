using Booking.Domain.Entities;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations;

public class StaffWorkingHoursConfiguration : IEntityTypeConfiguration<StaffWorkingHours>
{
    public void Configure(EntityTypeBuilder<StaffWorkingHours> builder)
    {
        // უნიკალური იყოს კვირის დღე და ვერ გამეორდეს 
        builder.HasIndex(x => new { x.StaffId, x.DayOfWeek }).IsUnique();


    }
}
