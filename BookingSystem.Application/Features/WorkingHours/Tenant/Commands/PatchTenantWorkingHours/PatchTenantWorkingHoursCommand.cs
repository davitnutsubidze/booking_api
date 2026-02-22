using BookingSystem.Application.DTOs.WorkingHours;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.PatchTenantWorkingHours;

public sealed record PatchTenantWorkingHoursCommand(
    Guid TenantId,
    int DayOfWeek,
    PatchTenantWorkingHoursDto Body
) : IRequest;