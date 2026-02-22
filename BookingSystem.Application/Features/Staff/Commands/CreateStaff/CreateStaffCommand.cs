using BookingSystem.Application.DTO.Staff;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Commands.CreateStaff;

public sealed record CreateStaffCommand(Guid TenantId, CreateStaffDto Body) : IRequest<StaffDto>;
