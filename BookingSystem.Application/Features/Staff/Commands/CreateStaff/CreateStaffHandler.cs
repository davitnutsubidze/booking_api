using Booking.Domain.Entities;
using BookingSystem.Application.DTO.Staff;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using MediatR;
using System.Globalization;

namespace BookingSystem.Application.Features.Staff.Commands.CreateStaff;

public sealed class CreateStaffHandler : IRequestHandler<CreateStaffCommand, StaffDto>
{
    private readonly IStaffRepository _repo;
    private readonly IStaffLunchBreakRepository _lunchRepo;
    private readonly IUnitOfWork _uow;

    public CreateStaffHandler(IStaffRepository repo, IStaffLunchBreakRepository lunchRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _lunchRepo = lunchRepo;
        _uow = uow;
    }

    public async Task<StaffDto> Handle(CreateStaffCommand request, CancellationToken ct)
    {
        var s = new Booking.Domain.Entities.Staff
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            FirstName = request.Body.FirstName,
            LastName = request.Body.LastName,
            Phone = request.Body.Phone,
            Bio = request.Body.Bio,
            IsActive = request.Body.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(s, ct);

        // ✅ Default LunchBreak გენერაცია

        var lunchStart = new TimeOnly(13, 0);
        var lunchEnd = new TimeOnly(14, 0);

        for (int i = 0; i < 7; i++)
        {
            var lunch = new StaffLunchBreak
            {
                Id = Guid.NewGuid(),
                TenantId = request.TenantId,
                StaffId = s.Id,
                DayOfWeek = (DayOfWeek)i,
                StartTime = lunchStart,
                EndTime = lunchEnd,
                IsEnabled = true,
            };

            await _lunchRepo.AddAsync(lunch, ct);
        }

        await _uow.SaveChangesAsync(ct);

        return new StaffDto(s.Id, s.TenantId, s.FirstName, s.LastName, s.Phone, s.Bio, s.IsActive);
    }
}
