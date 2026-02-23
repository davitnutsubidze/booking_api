using BookingSystem.Application.DTO.WorkingHours.Staff;
using MediatR;

namespace BookingSystem.Application.Features.WorkingHours.Staff.Queries.GetStaffWorkingHours;

public sealed record GetStaffWorkingHoursQuery(Guid StaffId)
    : IRequest<List<PatchWorkingHoursDayDto>>;
