using Booking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.Entities
{
    public class Service : AuditableEntity
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int DurationMinutes { get; set; }

        public decimal? Price { get; set; }
        public string? Currency { get; set; } = "USD";

        public bool IsActive { get; set; } = true;

        public ICollection<StaffService> StaffServices { get; set; } = new List<StaffService>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}
