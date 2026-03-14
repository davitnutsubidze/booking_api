using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");

        b.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(50);

        b.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.PasswordHash)
            .IsRequired();

        b.Property(x => x.Role)
            .IsRequired();

        b.Property(x => x.IsActive)
            .IsRequired();

        b.HasOne(x => x.Tenant)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        //b.HasIndex(x => x.Email).IsUnique(); // გლობალურად უნიკალური
        b.HasIndex(x => new { x.TenantId, x.Email }).IsUnique(); //  tenant-ზე უნიკალური 
    }
}