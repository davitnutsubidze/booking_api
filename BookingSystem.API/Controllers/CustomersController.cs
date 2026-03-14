using BookingSystem.Application.DTOs.Customers;
using BookingSystem.Application.Features.Customers.Queries.GetCustomersByTenantId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("tenant/{tenantId:guid}")]
    public async Task<ActionResult<List<CustomerDto>>> GetByTenantId(
        Guid tenantId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCustomersByTenantIdQuery(tenantId), ct);
        return Ok(result);
    }
}