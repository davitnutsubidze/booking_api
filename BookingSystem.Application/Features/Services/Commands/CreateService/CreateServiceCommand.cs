using BookingSystem.Application.DTOs.Services;
using MediatR;

namespace BookingSystem.Application.Features.Services.Commands.CreateService;

public sealed record CreateServiceCommand(Guid TenantId, CreateServiceDto Body) : IRequest<ServiceDto>;
