using BookingSystem.Application.DTO.WorkingHours.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Queries.GetTenantWorkingHours;

public sealed record GetTenantWorkingHoursQuery(Guid TenantId)
    : IRequest<List<TenantWorkingHoursDayDto>>;
