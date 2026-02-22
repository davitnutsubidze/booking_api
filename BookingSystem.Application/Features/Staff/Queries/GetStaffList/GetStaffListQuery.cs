using BookingSystem.Application.DTO.Staff;
using MediatR;

namespace BookingSystem.Application.Features.Staff.Queries.GetStaffList;

public sealed record GetStaffListQuery(Guid TenantId) : IRequest<List<StaffDto>>;
