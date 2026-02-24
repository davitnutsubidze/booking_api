using BookingSystem.Application.DTOs.LunchBreaks;
using MediatR;

public sealed record GetStaffLunchBreaksQuery(
    Guid TenantId,
    Guid StaffId
) : IRequest<List<StaffLunchBreakDto>>;