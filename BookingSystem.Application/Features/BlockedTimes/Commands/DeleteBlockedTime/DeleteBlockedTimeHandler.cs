using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.BlockedTimes.Commands.DeleteBlockedTime;

public sealed class DeleteBlockedTimeHandler : IRequestHandler<DeleteBlockedTimeCommand>
{
    private readonly IBlockedTimeCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteBlockedTimeHandler(IBlockedTimeCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteBlockedTimeCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("BlockedTime not found.");

        if (entity.TenantId != request.TenantId)
            throw new ConflictException("BlockedTime does not belong to this tenant.");

        _repo.Remove(entity);
        await _uow.SaveChangesAsync(ct);
    }
}
