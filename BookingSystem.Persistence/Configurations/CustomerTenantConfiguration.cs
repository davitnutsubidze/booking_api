using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Persistence.Configurations;

public sealed class CustomerTenantConfiguration : IEntityTypeConfiguration<CustomerTenant>
{
    public void Configure(EntityTypeBuilder<CustomerTenant> b)
    {
        b.ToTable("CustomerTenants");

        // Composite Primary Key
        b.HasKey(x => new { x.CustomerId, x.TenantId });

        b.Property(x => x.FirstVisitAt)
            .IsRequired();

        b.Property(x => x.IsBlocked)
            .IsRequired();

        b.Property(x => x.IsDeleted)
            .IsRequired();

        b.Property(x => x.Notes)
            .HasMaxLength(2000);

        b.HasOne(x => x.Customer)
            .WithMany(x => x.CustomerTenants)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Tenant)
            .WithMany(x => x.CustomerTenants)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Useful indexes
        b.HasIndex(x => new { x.TenantId, x.IsBlocked });
        b.HasIndex(x => new { x.TenantId, x.LastVisitAt });
    }
}