using BookingSystem.Application.DTOs.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.Tenant.Queries.GetTenantBySlug;

public sealed record GetTenantBySlugQuery(string slug) : IRequest<TenantDto>;