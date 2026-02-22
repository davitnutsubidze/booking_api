using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations;

public class StaffServiceConfiguration : IEntityTypeConfiguration<StaffService>
{
    public void Configure(EntityTypeBuilder<StaffService> builder)
    {
        // Composite Primary Key
        builder.HasKey(x => new { x.StaffId, x.ServiceId });

        // Relationships (თუ navigation-ები გაქვს entity-ებში)
        builder.HasOne(x => x.Staff)
            .WithMany(s => s.StaffServices)
            .HasForeignKey(x => x.StaffId);

        builder.HasOne(x => x.Service)
            .WithMany(s => s.StaffServices)
            .HasForeignKey(x => x.ServiceId);


        // ეს navigation არ გვაქვს ამ ეტაპზე
        //builder.ToTable("StaffServices");
    }
}
