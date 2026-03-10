using BookingSystem.Application.DTOs.Tenant;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Tenant.Queries.GetTenantBySlug;

public sealed class GetTenantBySlugHandler
    : IRequestHandler<GetTenantBySlugQuery, TenantDto>
{
    private readonly ITenantRepository _repo;

    public GetTenantBySlugHandler(ITenantRepository repo) => _repo = repo;

    public async Task<TenantDto> Handle(GetTenantBySlugQuery request, CancellationToken ct)
    {
        var tenant = await _repo.GetBySlugAsync(request.slug, ct);

        return new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.Description,
            tenant.Phone,
            tenant.Email,
            tenant.Address,
            tenant.TimeZone,
            tenant.IsActive
        );

    }
}