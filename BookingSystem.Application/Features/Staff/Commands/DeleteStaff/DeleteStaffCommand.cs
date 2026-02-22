using MediatR;

namespace BookingSystem.Application.Features.Staff.Commands.DeleteStaff;

public sealed record DeleteStaffCommand(Guid TenantId, Guid Id) : IRequest;
