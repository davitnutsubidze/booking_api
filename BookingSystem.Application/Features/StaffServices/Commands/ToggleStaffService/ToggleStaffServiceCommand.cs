using MediatR;

namespace BookingSystem.Application.Features.StaffServices.Commands.ToggleStaffService;

public sealed record ToggleStaffServiceCommand(
    Guid TenantId,
    Guid StaffId,
    Guid ServiceId
) : IRequest<ToggleStaffServiceResult>;

public sealed record ToggleStaffServiceResult(bool Assigned);