using Booking.Domain.Common;
using Booking.Domain.Enums;


namespace Booking.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";

        public PaymentStatus Status { get; set; }

        public string Provider { get; set; } = null!;
        public string? TransactionId { get; set; }
    }

}
