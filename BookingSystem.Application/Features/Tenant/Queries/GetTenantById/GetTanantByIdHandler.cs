using BookingSystem.Application.DTOs.Tenant;
using BookingSystem.Application.Features.Tenants.Queries.GetTenantById;
using BookingSystem.Application.Interfaces;
using MediatR;

public class GetTenantByIdHandler
    : IRequestHandler<GetTenantByIdQuery, TenantDto>
{
    private readonly ITenantRepository _repo;

    public GetTenantByIdHandler(ITenantRepository repo)
    {
        _repo = repo;
    }

    public async Task<TenantDto?> Handle(GetTenantByIdQuery request, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(request.Id, ct);

        if (tenant is null)
            return null;

        return new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.Description,
            tenant.Phone,
            tenant.Email,
            tenant.Address,
            tenant.TimeZone,
            tenant.IsActive);
    }

}