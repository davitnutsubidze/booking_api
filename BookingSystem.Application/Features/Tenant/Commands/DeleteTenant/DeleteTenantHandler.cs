using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Tenants.Commands.DeleteTenant;

public sealed class DeleteTenantHandler
    : IRequestHandler<DeleteTenantCommand, bool>
{
    private readonly ITenantRepository _repo;

    public DeleteTenantHandler(ITenantRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteTenantCommand request, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(request.Id, ct);
        if (tenant is null)
            return false;

        await _repo.DeleteAsync(tenant, ct);
        return true;
    }
}