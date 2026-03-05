using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Services.Queries.GetServicesList;

public sealed class GetServicesListHandler : IRequestHandler<GetServicesListQuery, List<ServiceDto>>
{
    private readonly IServiceCrudRepository _repo;
    public GetServicesListHandler(IServiceCrudRepository repo) => _repo = repo;

    public async Task<List<ServiceDto>> Handle(GetServicesListQuery request, CancellationToken ct)
    {
        // Case 1: Owner view + staffId => all services with assigned flag
        if (!request.filterByStaff && request.StaffId is not null)
        {
            return await _repo.GetAllWithAssignmentAsync(request.TenantId, request.StaffId.Value, ct);

        }

        // Case 2: staff filter only (non-owner) => only staff services
        if (request.StaffId is not null)
        {
            return await _repo.GetByTenantAndStaffAsync(request.TenantId, request.StaffId, ct);
        }

        // Case 3: all services (no staff filter)
        return await _repo.GetByTenantAsync(request.TenantId, ct);
    }
}
