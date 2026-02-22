using Booking.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.Entities
{
    public class Tenant : AuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }

        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string TimeZone { get; set; } = "UTC";

        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}
