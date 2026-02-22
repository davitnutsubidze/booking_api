using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Commands.DeleteStaff;

public sealed class DeleteStaffHandler : IRequestHandler<DeleteStaffCommand>
{
    private readonly IStaffRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteStaffHandler(IStaffRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteStaffCommand request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (s.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        _repo.Remove(s);
        await _uow.SaveChangesAsync(ct);
    }
}
