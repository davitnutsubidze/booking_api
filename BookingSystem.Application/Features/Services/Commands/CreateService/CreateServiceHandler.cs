using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Services.Commands.CreateService;

public sealed class CreateServiceHandler : IRequestHandler<CreateServiceCommand, ServiceDto>
{
    private readonly IServiceCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateServiceHandler(IServiceCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<ServiceDto> Handle(CreateServiceCommand request, CancellationToken ct)
    {
        var service = new Booking.Domain.Entities.Service
        {
            TenantId = request.TenantId,
            Name = request.Body.Name,
            Description = request.Body.Description,
            DurationMinutes = request.Body.DurationMinutes,
            Price = request.Body.Price,
            Currency = request.Body.Currency,
            IsActive = request.Body.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(service, ct);
        await _uow.SaveChangesAsync(ct);

        return new ServiceDto(
            service.Id, service.TenantId, service.Name, service.Description,
            service.DurationMinutes, service.Price, service.Currency, service.IsActive
        );
    }
}
