using Booking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.Entities
{
    public class Staff : AuditableEntity
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Bio { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<StaffService> StaffServices { get; set; } = new List<StaffService>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}
