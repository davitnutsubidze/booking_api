using Booking_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Booking_API.data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<CompanyEmployees> CompanyEmployees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
    new Company { Id = 1, Name = "Alpha Tech", Details = "Software development company", Rate = 4.5, CreatedDate = new DateTime(2026, 2, 9, 12, 0, 0, DateTimeKind.Utc), UpdatedDate = new DateTime(2026, 2, 9, 12, 0, 0, DateTimeKind.Utc) },
    new Company { Id = 2, Name = "Beta Solutions", Details = "IT consulting services", Rate = 4.2, CreatedDate = new DateTime(2026, 2, 9, 12, 5, 0, DateTimeKind.Utc), UpdatedDate = new DateTime(2026, 2, 9, 12, 5, 0, DateTimeKind.Utc) },
    new Company { Id = 3, Name = "Gamma Group", Details = "Business analytics and BI", Rate = 4.8, CreatedDate = new DateTime(2026, 2, 9, 12, 10, 0, DateTimeKind.Utc), UpdatedDate = new DateTime(2026, 2, 9, 12, 10, 0, DateTimeKind.Utc) },
    new Company { Id = 4, Name = "Delta Systems", Details = "Cloud infrastructure services", Rate = 4.0, CreatedDate = new DateTime(2026, 2, 9, 12, 15, 0, DateTimeKind.Utc), UpdatedDate = new DateTime(2026, 2, 9, 12, 15, 0, DateTimeKind.Utc) },
    new Company { Id = 5, Name = "Epsilon Labs", Details = "AI & machine learning startup", Rate = 4.9, CreatedDate = new DateTime(2026, 2, 9, 12, 20, 0, DateTimeKind.Utc), UpdatedDate = new DateTime(2026, 2, 9, 12, 20, 0, DateTimeKind.Utc) });
        }
    }
}
