using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTO.Staff;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Queries.GetStaffById;

public sealed class GetStaffByIdHandler : IRequestHandler<GetStaffByIdQuery, StaffDto>
{
    private readonly IStaffRepository _repo;
    public GetStaffByIdHandler(IStaffRepository repo) => _repo = repo;

    public async Task<StaffDto> Handle(GetStaffByIdQuery request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (s.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        return new StaffDto(s.Id, s.TenantId, s.FirstName, s.LastName, s.Phone, s.Bio, s.IsActive, s.StaffServices.Select(ss => ss.ServiceId).ToList());
    }
}
