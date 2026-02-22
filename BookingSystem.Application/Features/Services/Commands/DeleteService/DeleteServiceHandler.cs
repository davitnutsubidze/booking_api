using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Services.Commands.DeleteService;

public sealed class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IServiceCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteServiceHandler(IServiceCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Service not found.");

        if (s.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        _repo.Remove(s);
        await _uow.SaveChangesAsync(ct);
    }
}
