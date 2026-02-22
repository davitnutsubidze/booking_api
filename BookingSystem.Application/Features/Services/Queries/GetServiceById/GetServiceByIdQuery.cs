using BookingSystem.Application.DTOs.Services;
using MediatR;

namespace BookingSystem.Application.Features.Services.Queries.GetServiceById;

public sealed record GetServiceByIdQuery(Guid TenantId, Guid Id) : IRequest<ServiceDto>;
