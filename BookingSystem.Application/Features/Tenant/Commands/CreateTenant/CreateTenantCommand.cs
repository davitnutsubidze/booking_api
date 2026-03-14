using BookingSystem.Application.DTOs.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.Tenants.Commands.CreateTenant;

public sealed record CreateTenantCommand(
    string Name,
    string Slug,
    string? Description,
    string Phone,
    string Email,
    string Address,
    string TimeZone
) : IRequest<TenantDto>;