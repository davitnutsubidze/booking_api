using BookingSystem.Application.DTOs.Services;
using MediatR;

namespace BookingSystem.Application.Features.Services.Commands.UpdateService;

public sealed record UpdateServiceCommand(Guid TenantId, Guid Id, UpdateServiceDto Body) : IRequest<ServiceDto>;
