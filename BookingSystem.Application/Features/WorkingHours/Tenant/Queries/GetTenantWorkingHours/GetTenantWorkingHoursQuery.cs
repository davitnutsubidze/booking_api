using BookingSystem.Application.DTOs.WorkingHours;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Tenant.Queries.GetTenantWorkingHours;

public sealed record GetTenantWorkingHoursQuery(Guid TenantId)
    : IRequest<List<WorkingHoursDayDto>>;
