using BookingSystem.API.Contracts;
using BookingSystem.Application.DTOs.Tenant;
using BookingSystem.Application.Features.Tenant.Queries.GetTenantBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants")]
public sealed class TenantController : ControllerBase
{
    private readonly IMediator _mediator;
    public TenantController(IMediator mediator) => _mediator = mediator;


    [HttpGet("{slug}")]

    [ProducesResponseType(typeof(ApiResponse<TenantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string slug, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTenantBySlugQuery(slug), ct));
}

