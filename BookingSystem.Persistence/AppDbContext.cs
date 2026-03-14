using Booking.Domain.Entities;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Tenant> Tenant => Set<Tenant>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<StaffService> StaffServices => Set<StaffService>();
    public DbSet<BlockedTime> BlockedTimes => Set<BlockedTime>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Payment> Payment => Set<Payment>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<StaffLunchBreak> StaffLunchBreaks => Set<StaffLunchBreak>();
    public DbSet<User> User => Set<User>();
    public DbSet<TenantWorkingHours> TenantWorkingHours => Set<TenantWorkingHours>();
    public DbSet<StaffWorkingHours> StaffWorkingHours => Set<StaffWorkingHours>();
    public DbSet<CustomerTenant> CustomerTenants => Set<CustomerTenant>();


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // ავტომატურად იპოვის Configurations ფოლდერში ყველა კონფიგს
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}
