using BookingSystem.Application.DTO.Staff;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Commands.UpdateStaff;

public sealed record UpdateStaffCommand(Guid TenantId, Guid Id, UpdateStaffDto Body) : IRequest<StaffDto>;
