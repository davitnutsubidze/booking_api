using BookingSystem.Application.DTOs.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.Tenants.Commands.UpdateTenant;

public sealed record UpdateTenantCommand(
    Guid Id, UpdateTenantDto body) : IRequest<TenantDto?>;