using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Commands.DeleteStaffWorkingHoursDay;

public sealed record DeleteStaffWorkingHoursDayCommand(
    Guid TenantId,
    Guid StaffId,
    int DayOfWeek
) : IRequest;