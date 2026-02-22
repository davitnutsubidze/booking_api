using Booking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.Entities
{
    public class BlockedTime : AuditableEntity
    {
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public Guid? StaffId { get; set; }
        public Staff? Staff { get; set; }

        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }

        public string? Reason { get; set; }
    }

}
