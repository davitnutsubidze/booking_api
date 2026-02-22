using BookingSystem.Application.DTOs.WorkingHours;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;

public sealed record SetStaffWorkingHoursCommand(
    Guid StaffId,
    List<StaffWorkingHoursDayDto> Days
) : IRequest;
