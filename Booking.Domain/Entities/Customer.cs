using Booking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.Entities
{
    public class Customer : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Notes { get; set; }

        public ICollection<CustomerTenant> CustomerTenants { get; set; } = new List<CustomerTenant>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}
