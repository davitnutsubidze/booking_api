using Booking.Domain.Entities;
using BookingSystem.Application.Interfaces;
using MediatR;

public class GetAllTenantsHandler
    : IRequestHandler<GetAllTenantsQuery, List<Tenant>>
{
    private readonly ITenantRepository _repo;

    public GetAllTenantsHandler(ITenantRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Tenant>> Handle(
        GetAllTenantsQuery request,
        CancellationToken ct)
    {
        return await _repo.GetAllAsync(ct);
    }
}