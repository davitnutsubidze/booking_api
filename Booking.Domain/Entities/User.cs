using Booking.Domain.Common;
using Booking.Domain.Enums;

namespace Booking.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid? TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
