using BookingSystem.Application.DTOs.BlockedTimes;
using BookingSystem.Application.Features.BlockedTimes.Commands.CreateBlockedTime;
using BookingSystem.Application.Features.BlockedTimes.Commands.DeleteBlockedTime;
using BookingSystem.Application.Features.BlockedTimes.Queries.GetBlockedTimes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/blocked-times")]
public sealed class BlockedTimesController : ControllerBase
{
    private readonly IMediator _mediator;
    public BlockedTimesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(
        Guid tenantId,
        [FromQuery] DateTime fromUtc,
        [FromQuery] DateTime toUtc,
        [FromQuery] Guid? staffId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBlockedTimesQuery(tenantId, fromUtc, toUtc, staffId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid tenantId, [FromBody] CreateBlockedTimeRequestDto body, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateBlockedTimeCommand(tenantId, body), ct);
        return CreatedAtAction(
        nameof(Get),
        new { tenantId = tenantId, fromUtc = result.StartUtc, toUtc = result.EndUtc },
        result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid tenantId, Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBlockedTimeCommand(tenantId, id), ct);
        return NoContent();
    }
}
