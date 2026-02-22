using Booking.Domain.Entities;
using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;

namespace BookingSystem.Application.Features.StaffServices.Commands.ToggleStaffService;

public sealed class ToggleStaffServiceHandler
    : IRequestHandler<ToggleStaffServiceCommand, ToggleStaffServiceResult>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IServiceCrudRepository _serviceRepo;
    private readonly IStaffServiceRepository _staffServiceRepo;
    private readonly IUnitOfWork _uow;

    public ToggleStaffServiceHandler(
        IStaffRepository staffRepo,
        IServiceCrudRepository serviceRepo,
        IStaffServiceRepository staffServiceRepo,
        IUnitOfWork uow)
    {
        _staffRepo = staffRepo;
        _serviceRepo = serviceRepo;
        _staffServiceRepo = staffServiceRepo;
        _uow = uow;
    }

    public async Task<ToggleStaffServiceResult> Handle(ToggleStaffServiceCommand request, CancellationToken ct)
    {
        // Staff belongs to tenant?
        var staff = await _staffRepo.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (staff.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        // Service belongs to tenant?
        var service = await _serviceRepo.GetByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service not found.");

        if (service.TenantId != request.TenantId)
            throw new ConflictException("Service does not belong to this tenant.");

        // Toggle
        var exists = await _staffServiceRepo.ExistsAsync(request.StaffId, request.ServiceId, ct);

        if (exists)
        {
            await _staffServiceRepo.RemoveAsync(request.StaffId, request.ServiceId, ct);
            await _uow.SaveChangesAsync(ct);
            return new ToggleStaffServiceResult(false);
        }

        await _staffServiceRepo.AddAsync(new StaffService
        {
            StaffId = request.StaffId,
            ServiceId = request.ServiceId
        }, ct);

        await _uow.SaveChangesAsync(ct);
        return new ToggleStaffServiceResult(true);
    }
}