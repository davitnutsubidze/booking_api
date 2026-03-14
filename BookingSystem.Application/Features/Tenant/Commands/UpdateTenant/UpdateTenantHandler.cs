using BookingSystem.Application.DTOs;
using BookingSystem.Application.DTOs.Tenant;
using BookingSystem.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace BookingSystem.Application.Features.Tenants.Commands.UpdateTenant;

public sealed class UpdateTenantHandler
    : IRequestHandler<UpdateTenantCommand, TenantDto?>
{
    private readonly ITenantRepository _repo;

    public UpdateTenantHandler(ITenantRepository repo)
    {
        _repo = repo;
    }

    public async Task<TenantDto?> Handle(UpdateTenantCommand request, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(request.Id, ct);
        if (tenant is null)
            return null;

        var slugOwner = await _repo.GetBySlugAsync(request.body.Slug, ct);
        if (slugOwner is not null && slugOwner.Id != request.Id)
            throw new ValidationException("Slug already exists.");

        tenant.Name = request.body.Name;
        tenant.Slug = request.body.Slug;
        tenant.Description = request.body.Description;
        tenant.Phone = request.body.Phone;
        tenant.Email = request.body.Email;
        tenant.Address = request.body.Address;
        tenant.TimeZone = request.body.TimeZone;
        tenant.IsActive = request.body.IsActive;

        await _repo.UpdateAsync(tenant, ct);

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