using BookingSystem.Application.DTO.Staff;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Commands.CreateStaff;

public sealed class CreateStaffHandler : IRequestHandler<CreateStaffCommand, StaffDto>
{
    private readonly IStaffRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateStaffHandler(IStaffRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<StaffDto> Handle(CreateStaffCommand request, CancellationToken ct)
    {
        var s = new Booking.Domain.Entities.Staff
        {
            TenantId = request.TenantId,
            FirstName = request.Body.FirstName,
            LastName = request.Body.LastName,
            Phone = request.Body.Phone,
            Bio = request.Body.Bio,
            IsActive = request.Body.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(s, ct);
        await _uow.SaveChangesAsync(ct);

        return new StaffDto(s.Id, s.TenantId, s.FirstName, s.LastName, s.Phone, s.Bio, s.IsActive);
    }
}
