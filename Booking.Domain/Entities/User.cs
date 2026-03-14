using Booking.Domain.Common;
using Booking.Domain.Enums;

namespace Booking.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public string Phone { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;
        public UserRole Role { get; set; } = default!; // Customer / Staff / Owner / SuperAdmin

        public bool IsActive { get; set; } = true;

    }

}
