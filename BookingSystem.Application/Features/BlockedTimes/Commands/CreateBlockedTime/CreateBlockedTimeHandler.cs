using Booking.Domain.Entities;
using BookingSystem.Application.DTOs.BlockedTimes;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.BlockedTimes.Commands.CreateBlockedTime;

public sealed class CreateBlockedTimeHandler : IRequestHandler<CreateBlockedTimeCommand, BlockedTimeDto>
{
    
    private readonly IBlockedTimeCrudRepository _repo;
    private readonly IStaffRepository _staff;
    private readonly IUnitOfWork _uow;

    public CreateBlockedTimeHandler(IBlockedTimeCrudRepository repo, IUnitOfWork uow, IStaffRepository staff)
    {
        _repo = repo;
        _uow = uow;
        _staff = staff;
    }

    public async Task<BlockedTimeDto> Handle(CreateBlockedTimeCommand request, CancellationToken ct)
    {
        var entity = new BlockedTime
        {
            TenantId = request.TenantId,
            StaffId = request.Body.StaffId,
            StartDateTimeUtc = request.Body.StartUtc,
            EndDateTimeUtc = request.Body.EndUtc,
            Reason = request.Body.Reason,
            CreatedAt = DateTime.UtcNow // თუ NOT NULL გაქვს
        };

        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        string? staffName = null;
        if (entity.StaffId is not null)
        {
            staffName = await _staff.GetStaffNameAsync(entity.TenantId, entity.StaffId.Value, ct);
        }

        return new BlockedTimeDto(
            entity.Id,
            entity.TenantId,
            entity.StaffId,
            staffName,
            entity.StartDateTimeUtc,
            entity.EndDateTimeUtc,
            entity.Reason
        ); ;
    }
}
