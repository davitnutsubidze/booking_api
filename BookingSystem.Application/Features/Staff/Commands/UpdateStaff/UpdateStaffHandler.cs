using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTO.Staff;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Commands.UpdateStaff;

public sealed class UpdateStaffHandler : IRequestHandler<UpdateStaffCommand, StaffDto>
{
    private readonly IStaffRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateStaffHandler(IStaffRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<StaffDto> Handle(UpdateStaffCommand request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (s.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        s.FirstName = request.Body.FirstName;
        s.LastName = request.Body.LastName;
        s.Phone = request.Body.Phone;
        s.Bio = request.Body.Bio;
        s.IsActive = request.Body.IsActive;

        await _uow.SaveChangesAsync(ct);

        return new StaffDto(s.Id, s.TenantId, s.FirstName, s.LastName, s.Phone, s.Bio, s.IsActive, s.StaffServices.Select(ss => ss.ServiceId).ToList());
    }
}
