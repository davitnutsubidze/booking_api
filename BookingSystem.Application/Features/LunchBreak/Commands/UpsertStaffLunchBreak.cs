using BookingSystem.Application.DTOs.LunchBreaks;
using MediatR;

public sealed record UpsertStaffLunchBreakCommand(
    Guid TenantId,
    Guid StaffId,
    int DayOfWeek,
    UpsertStaffLunchBreakDto Body
) : IRequest<StaffLunchBreakDto>;