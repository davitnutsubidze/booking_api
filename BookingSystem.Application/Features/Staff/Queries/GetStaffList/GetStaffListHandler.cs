using BookingSystem.Application.DTO.Staff;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Queries.GetStaffList;

public sealed class GetStaffListHandler : IRequestHandler<GetStaffListQuery, List<StaffDto>>
{
    private readonly IStaffRepository _repo;
    public GetStaffListHandler(IStaffRepository repo) => _repo = repo;

    public async Task<List<StaffDto>> Handle(GetStaffListQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByTenantAsync(request.TenantId, ct);

        return items.Select(s => new StaffDto(
            s.Id, s.TenantId, s.FirstName, s.LastName, s.Phone, s.Bio, s.IsActive
        )).ToList();
    }
}
