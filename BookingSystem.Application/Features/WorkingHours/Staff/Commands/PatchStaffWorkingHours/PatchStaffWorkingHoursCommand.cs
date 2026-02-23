using BookingSystem.Application.DTO.WorkingHours.Staff;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;

public sealed record PatchStaffWorkingHoursCommand(
    Guid StaffId,
    int DayOfWeek,
    PatchWorkingHoursDayDto Body
) : IRequest<StaffWorkingHoursDayDto>;
