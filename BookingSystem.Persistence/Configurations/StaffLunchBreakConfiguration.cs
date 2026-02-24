using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations;

public sealed class StaffLunchBreakConfiguration : IEntityTypeConfiguration<StaffLunchBreak>
{
    public void Configure(EntityTypeBuilder<StaffLunchBreak> b)
    {
        b.ToTable("StaffLunchBreaks");

        b.HasKey(x => x.Id);

        // ✅ ერთ staff-ზე ერთ დღეზე ერთი ლანჩი
        b.HasIndex(x => new { x.StaffId, x.DayOfWeek })
            .IsUnique();

        // ✅ სწრაფი lookup tenant-ზე
        b.HasIndex(x => x.TenantId);

        b.Property(x => x.StartTime)
            .IsRequired();

        b.Property(x => x.EndTime)
            .IsRequired();

        b.Property(x => x.IsEnabled)
            .IsRequired();


        // FK Staff
        b.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}