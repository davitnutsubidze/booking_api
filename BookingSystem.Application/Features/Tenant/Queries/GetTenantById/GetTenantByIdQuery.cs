using BookingSystem.Application.DTOs.Tenant;
using MediatR;

namespace BookingSystem.Application.Features.Tenants.Queries.GetTenantById;

public sealed record GetTenantByIdQuery(Guid Id) : IRequest<TenantDto?>;