using BookingSystem.API.Contracts;
using BookingSystem.Application.DTOs.Services;
using BookingSystem.Application.Features.Services.Commands.CreateService;
using BookingSystem.Application.Features.Services.Commands.DeleteService;
using BookingSystem.Application.Features.Services.Commands.UpdateService;
using BookingSystem.Application.Features.Services.Queries.GetServiceById;
using BookingSystem.Application.Features.Services.Queries.GetServicesList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/services")]
public sealed class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ServicesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<ServiceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(Guid tenantId, CancellationToken ct, [FromQuery] Guid? staffId,
    [FromQuery] bool filterByStaff = true)
    => Ok(await _mediator.Send(new GetServicesListQuery(tenantId, staffId, filterByStaff), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid tenantId, Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetServiceByIdQuery(tenantId, id), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid tenantId, [FromBody] CreateServiceDto body, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateServiceCommand(tenantId, body), ct);
        return Created($"/api/tenants/{tenantId}/services/{result.Id}", result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid tenantId, Guid id, [FromBody] UpdateServiceDto body, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateServiceCommand(tenantId, id, body), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid tenantId, Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteServiceCommand(tenantId, id), ct);
        return NoContent();
    }
}
