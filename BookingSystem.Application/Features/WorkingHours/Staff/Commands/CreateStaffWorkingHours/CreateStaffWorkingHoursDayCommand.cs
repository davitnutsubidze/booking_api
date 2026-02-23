using BookingSystem.Application.DTO.WorkingHours.Staff;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.CreateStaffWorkingHoursDay;

public sealed record CreateStaffWorkingHoursDayCommand(
    Guid TenantId,
    Guid StaffId,
    CreateStaffWorkingHoursDayDto Body
) : IRequest<PatchWorkingHoursDayDto>;