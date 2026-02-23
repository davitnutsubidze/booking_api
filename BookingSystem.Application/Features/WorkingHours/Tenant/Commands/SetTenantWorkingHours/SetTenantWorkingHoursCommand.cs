using BookingSystem.Application.DTO.WorkingHours.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Commands.SetTenantWorkingHours;

public sealed record SetTenantWorkingHoursCommand(
    Guid TenantId,
    List<TenantWorkingHoursDayDto> Days
) : IRequest;
