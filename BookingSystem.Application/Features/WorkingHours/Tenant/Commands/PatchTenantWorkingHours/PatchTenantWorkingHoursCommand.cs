using BookingSystem.Application.DTO.WorkingHours.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.PatchTenantWorkingHours;

public sealed record PatchTenantWorkingHoursCommand(
    Guid TenantId,
    int DayOfWeek,
    PatchTenantWorkingHoursDto Body
) : IRequest;