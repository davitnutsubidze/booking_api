using Booking.Domain.Entities;

public class CustomerTenant
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public DateTime FirstVisitAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastVisitAt { get; set; }

    public string? Notes { get; set; }
    public bool IsBlocked { get; set; } = false;
    public bool IsDeleted { get; set; } = false;

}