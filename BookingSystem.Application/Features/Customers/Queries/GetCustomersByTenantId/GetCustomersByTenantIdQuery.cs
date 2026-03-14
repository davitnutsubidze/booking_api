using BookingSystem.Application.DTOs.Customers;
using MediatR;

namespace BookingSystem.Application.Features.Customers.Queries.GetCustomersByTenantId;

public sealed record GetCustomersByTenantIdQuery(Guid TenantId)
    : IRequest<List<CustomerDto>>;