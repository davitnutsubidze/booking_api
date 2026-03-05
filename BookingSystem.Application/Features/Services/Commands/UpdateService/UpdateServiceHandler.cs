using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Services.Commands.UpdateService;

public sealed class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, ServiceDto>
{
    private readonly IServiceCrudRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateServiceHandler(IServiceCrudRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<ServiceDto> Handle(UpdateServiceCommand request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Service not found.");

        if (s.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        s.Name = request.Body.Name;
        s.Description = request.Body.Description;
        s.DurationMinutes = request.Body.DurationMinutes;
        s.Price = request.Body.Price;
        s.Currency = request.Body.Currency;
        s.IsActive = request.Body.IsActive;

        await _uow.SaveChangesAsync(ct);

        return new ServiceDto(s.Id, s.TenantId, s.Name, s.Description, s.DurationMinutes, s.Price, s.Currency, s.IsActive, s.StaffServices.Select(ss => ss.StaffId).Distinct().ToList());
    }
}
