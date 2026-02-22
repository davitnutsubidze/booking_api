using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.StaffServices.Commands.SetStaffServices;

public sealed class SetStaffServicesHandler : IRequestHandler<SetStaffServicesCommand>
{
    private readonly IStaffRepository _staff;
    private readonly IServiceCrudRepository _services; // ან შენი IServiceRepository
    private readonly IStaffServiceRepository _staffServices;
    private readonly IUnitOfWork _uow;

    public SetStaffServicesHandler(
        IStaffRepository staff,
        IServiceCrudRepository services,
        IStaffServiceRepository staffServices,
        IUnitOfWork uow)
    {
        _staff = staff;
        _services = services;
        _staffServices = staffServices;
        _uow = uow;
    }

    public async Task Handle(SetStaffServicesCommand request, CancellationToken ct)
    {
        // 1) არსებობს სტაფი + და ეკუთვნის შესაბამის ტენატს
        var staff = await _staff.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (staff.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        // 2) ყველა სერვისი არსებობს თ არა + ეკუთვნის ტენატს
        // Efficient way: fetch tenant services and compare
        var tenantServices = await _services.GetByTenantAsync(request.TenantId, ct);
        var tenantServiceIds = tenantServices.Select(s => s.Id).ToHashSet();

        var invalid = request.ServiceIds.Where(id => !tenantServiceIds.Contains(id)).ToList();
        if (invalid.Count > 0)
            throw new NotFoundException("One or more services not found for this tenant.");

        // 3) Replace join rows
        await _staffServices.ReplaceStaffServicesAsync(request.StaffId, request.ServiceIds, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
