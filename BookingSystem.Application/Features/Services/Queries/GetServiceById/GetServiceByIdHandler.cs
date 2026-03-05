using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Services.Queries.GetServiceById;

public sealed class GetServiceByIdHandler : IRequestHandler<GetServiceByIdQuery, ServiceDto>
{
    private readonly IServiceCrudRepository _repo;
    public GetServiceByIdHandler(IServiceCrudRepository repo) => _repo = repo;

    public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Service not found.");

        if (s.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        return new ServiceDto(s.Id, s.TenantId, s.Name, s.Description, s.DurationMinutes, s.Price, s.Currency, s.IsActive, s.StaffServices.Select(ss => ss.StaffId).Distinct().ToList());
    }
}
