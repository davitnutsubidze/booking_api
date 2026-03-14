using Booking.Domain.Entities;
using BookingSystem.API.Contracts;
using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.DTOs.Tenant;
using BookingSystem.Application.Features.Services.Commands.CreateService;
using BookingSystem.Application.Features.Services.Commands.DeleteService;
using BookingSystem.Application.Features.Services.Commands.UpdateService;
using BookingSystem.Application.Features.Tenant.Queries.GetTenantBySlug;
using BookingSystem.Application.Features.Tenants.Commands.CreateTenant;
using BookingSystem.Application.Features.Tenants.Commands.DeleteTenant;
using BookingSystem.Application.Features.Tenants.Commands.UpdateTenant;
using BookingSystem.Application.Features.Tenants.Queries.GetTenantById;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants")]
public sealed class TenantController : ControllerBase
{
    private readonly IMediator _mediator;
    public TenantController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<TenantDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Tenant>>> GetAll(CancellationToken ct)
    {
        var tenants = await _mediator.Send(new GetAllTenantsQuery(), ct);

        return Ok(tenants);
    }

    [HttpGet("{slug}")]

    [ProducesResponseType(typeof(ApiResponse<TenantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string slug, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTenantBySlugQuery(slug), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTenantByIdQuery(id), ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TenantDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<TenantDto>> Create([FromBody] CreateTenantCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTenantDto body, CancellationToken ct)
    => Ok(await _mediator.Send(new UpdateTenantCommand(id, body), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteTenantCommand(id), ct);
        return NoContent();
    }


}
