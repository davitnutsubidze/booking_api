using BookingSystem.Application.DTOs.Customers;
using BookingSystem.Application.Interfaces;
using MediatR;

namespace BookingSystem.Application.Features.Customers.Queries.GetCustomersByTenantId;

public sealed class GetCustomersByTenantIdHandler
    : IRequestHandler<GetCustomersByTenantIdQuery, List<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersByTenantIdHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersByTenantIdQuery request, CancellationToken ct)
    {
        return await _customerRepository.GetByTenantIdAsync(request.TenantId, ct);
    }
}