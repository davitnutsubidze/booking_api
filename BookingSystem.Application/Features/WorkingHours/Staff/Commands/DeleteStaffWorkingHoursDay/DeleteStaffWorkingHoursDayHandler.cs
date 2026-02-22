using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.DeleteStaffWorkingHoursDay;

public sealed class DeleteStaffWorkingHoursDayHandler : IRequestHandler<DeleteStaffWorkingHoursDayCommand>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStaffWorkingHoursRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteStaffWorkingHoursDayHandler(
        IStaffRepository staffRepo,
        IStaffWorkingHoursRepository repo,
        IUnitOfWork uow)
    {
        _staffRepo = staffRepo;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteStaffWorkingHoursDayCommand request, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (staff.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        var day = (DayOfWeek)request.DayOfWeek;

        var entity = await _repo.GetByStaffAndDayAsync(request.StaffId, day, ct)
            ?? throw new NotFoundException("Staff working hours for this day not found.");

        _repo.Remove(entity);
        await _uow.SaveChangesAsync(ct);
    }
}