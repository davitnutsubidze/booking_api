using MediatR;

namespace BookingSystem.Application.Features.StaffServices.Commands.SetStaffServices;

public sealed record SetStaffServicesCommand(
    Guid TenantId,
    Guid StaffId,
    List<Guid> ServiceIds
) : IRequest;
