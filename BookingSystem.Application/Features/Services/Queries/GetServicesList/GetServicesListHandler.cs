using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Services.Queries.GetServicesList;

public sealed class GetServicesListHandler : IRequestHandler<GetServicesListQuery, List<ServiceDto>>
{
    private readonly IServiceCrudRepository _repo;
    public GetServicesListHandler(IServiceCrudRepository repo) => _repo = repo;

    //public async Task<List<ServiceDto>> Handle(GetServicesListQuery request, CancellationToken ct)
    //{
    //    var items = request.StaffId is null
    //                   ? await _repo.GetByTenantAsync(request.TenantId, ct)
    //                   : await _repo.GetByTenantAndStaffAsync(request.TenantId, request.StaffId.Value, ct);


    //    return items.Select(s => new ServiceDto(
    //        s.Id, s.TenantId, s.Name, s.Description, s.DurationMinutes, s.Price, s.Currency, s.IsActive
    //    )).ToList();
    //}

    public async Task<List<ServiceDto>> Handle(GetServicesListQuery request, CancellationToken ct)
    {
        // Case 1: Owner view + staffId => all services with assigned flag
        if (!request.filterByStaff && request.StaffId is not null)
        {
            return await _repo.GetAllWithAssignmentAsync(request.TenantId, request.StaffId.Value, ct);

            //return rows.Select(r => new ServiceDto(
            //    r.Id,
            //    r.TenantId,
            //    r.Name,
            //    r.Description,
            //    r.DurationMinutes,
            //    r.Price,
            //    r.Currency,
            //    r.IsActive,
            //    r.AssignedToStaff
            //)).ToList();
        }

        // Case 2: staff filter only (non-owner) => only staff services
        if (request.StaffId is not null)
        {
            var items = await _repo.GetByTenantAndStaffAsync(request.TenantId, request.StaffId.Value, ct);

            return items.Select(s => new ServiceDto(
                s.Id, s.TenantId, s.Name, s.Description, s.DurationMinutes,
                s.Price, s.Currency, s.IsActive,
                true // რადგან filter staff-ზეა
            )).ToList();
        }

        // Case 3: all services (no staff filter)
        var all = await _repo.GetByTenantAsync(request.TenantId, ct);
        return all.Select(s => new ServiceDto(
            s.Id, s.TenantId, s.Name, s.Description, s.DurationMinutes,
            s.Price, s.Currency, s.IsActive,
            false
        )).ToList();
    }
}
