using BookingSystem.Application.DTOs.Services;
using MediatR;

namespace BookingSystem.Application.Features.Services.Queries.GetServicesList;

public sealed record GetServicesListQuery(
    Guid TenantId, 
    Guid? StaffId,
    bool filterByStaff = true) : IRequest<List<ServiceDto>>;
