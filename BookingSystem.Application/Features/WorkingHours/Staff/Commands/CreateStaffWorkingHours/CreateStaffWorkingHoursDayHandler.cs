using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.DTOs.WorkingHours;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.CreateStaffWorkingHoursDay;

public sealed class CreateStaffWorkingHoursDayHandler
    : IRequestHandler<CreateStaffWorkingHoursDayCommand, StaffWorkingHoursDayDto>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStaffWorkingHoursRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateStaffWorkingHoursDayHandler(
        IStaffRepository staffRepo,
        IStaffWorkingHoursRepository repo,
        IUnitOfWork uow)
    {
        _staffRepo = staffRepo;
        _repo = repo;
        _uow = uow;
    }

    public async Task<StaffWorkingHoursDayDto> Handle(CreateStaffWorkingHoursDayCommand request, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (staff.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to this tenant.");

        var day = (DayOfWeek)request.Body.DayOfWeek;

        // ✅ duplicate check
        if (await _repo.ExistsAsync(request.StaffId, day, ct))
            throw new ConflictException("Working hours for this day already exist. Use PATCH/PUT to update.");

        var start = TimeOnly.ParseExact(request.Body.StartTime, "HH:mm", CultureInfo.InvariantCulture);
        var end = TimeOnly.ParseExact(request.Body.EndTime, "HH:mm", CultureInfo.InvariantCulture);

        if (!request.Body.IsDayOff && end <= start)
            throw new ConflictException("EndTime must be after StartTime when day is not off.");

        var entity = new StaffWorkingHours
        {
            StaffId = request.StaffId,
            DayOfWeek = day,
            StartTime = start,
            EndTime = end,
            IsDayOff = request.Body.IsDayOff,
        };

        await _repo.AddAsync(entity, ct);

        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex) when (ex.InnerException?.Message.Contains("duplicate") == true)
        {
            // Optional: თუ unique constraint-მა დაიჭირა race-condition
            throw new ConflictException("Working hours for this day already exist.");
        }

        return new StaffWorkingHoursDayDto(entity.StaffId, (int) entity.DayOfWeek,  entity.StartTime.ToString("HH:mm"), entity.EndTime.ToString("HH:mm"), entity.IsDayOff);
    }
}