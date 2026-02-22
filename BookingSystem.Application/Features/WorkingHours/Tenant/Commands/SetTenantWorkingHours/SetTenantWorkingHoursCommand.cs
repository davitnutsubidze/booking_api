using BookingSystem.Application.DTOs.WorkingHours;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.SetTenantWorkingHours;

public sealed record SetTenantWorkingHoursCommand(
    Guid TenantId,
    List<WorkingHoursDayDto> Days
) : IRequest;
