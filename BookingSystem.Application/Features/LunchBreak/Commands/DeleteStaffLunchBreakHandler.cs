using BookingSystem.Application.Common.Exceptions;
using BookingSystem.Application.Interfaces;
using MediatR;


namespace BookingSystem.Application.Features.LunchBreak.Commands.DeleteStaffLunchBreak;

public sealed class DeleteStaffLunchBreak : IRequest<DeleteStaffLunchBreakCommand>
{

    private readonly IStaffLunchBreakRepository _repo;
    private readonly IUnitOfWork _uow;




    public async Task Handle(DeleteStaffLunchBreakCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByStaffAndDayAsync(
            request.StaffId,
            (DayOfWeek)request.DayOfWeek,
            ct);

        if (entity == null)
            throw new NotFoundException("Lunch break not found.");

        _repo.Remove(entity);
        await _uow.SaveChangesAsync(ct);
    }
}