using MediatR;

public sealed record DeleteStaffLunchBreakCommand(
    Guid TenantId,
    Guid StaffId,
    int DayOfWeek
) : IRequest;