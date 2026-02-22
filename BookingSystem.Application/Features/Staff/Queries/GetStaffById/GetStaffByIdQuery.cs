using BookingSystem.Application.DTO.Staff;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Queries.GetStaffById;

public sealed record GetStaffByIdQuery(Guid TenantId, Guid Id) : IRequest<StaffDto>;
