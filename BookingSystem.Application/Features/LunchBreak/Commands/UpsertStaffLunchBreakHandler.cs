using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using BookingSystem.Application.DTOs.LunchBreaks;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

public sealed class UpsertStaffLunchBreakHandler
    : IRequestHandler<UpsertStaffLunchBreakCommand, StaffLunchBreakDto>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStaffLunchBreakRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpsertStaffLunchBreakHandler(
        IStaffRepository staffRepo,
        IStaffLunchBreakRepository repo,
        IUnitOfWork uow)
    {
        _staffRepo = staffRepo;
        _repo = repo;
        _uow = uow;
    }

    public async Task<StaffLunchBreakDto> Handle(
        UpsertStaffLunchBreakCommand request,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff not found.");

        if (staff.TenantId != request.TenantId)
            throw new ConflictException("Staff does not belong to tenant.");

        var day = (DayOfWeek)request.DayOfWeek;

        var start = TimeOnly.ParseExact(request.Body.StartTime, "HH:mm", CultureInfo.InvariantCulture);
        var end = TimeOnly.ParseExact(request.Body.EndTime, "HH:mm", CultureInfo.InvariantCulture);

        if (request.Body.IsEnabled && end <= start)
            throw new ConflictException("EndTime must be after StartTime.");

        var entity = await _repo.GetByStaffAndDayAsync(request.StaffId, day, ct);

        if (entity == null)
        {
            entity = new StaffLunchBreak
            {
                Id = Guid.NewGuid(),
                TenantId = request.TenantId,
                StaffId = request.StaffId,
                DayOfWeek = day,
                StartTime = start,
                EndTime = end,
                IsEnabled = request.Body.IsEnabled
            };

            await _repo.AddAsync(entity, ct);
        }
        else
        {
            entity.StartTime = start;
            entity.EndTime = end;
            entity.IsEnabled = request.Body.IsEnabled;
        }

        await _uow.SaveChangesAsync(ct);

        return new StaffLunchBreakDto(
            entity.Id,
            entity.TenantId,
            entity.StaffId,
            (int)entity.DayOfWeek,
            entity.StartTime.ToString("HH:mm"),
            entity.EndTime.ToString("HH:mm"),
            entity.IsEnabled
        );
    }
}