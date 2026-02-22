using BookingSystem.Application.Features.Availability.Queries.GetAvailability;
using BookingSystem.Application.Features.Availability.Queries.GetAvailabilityV2;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AvailabilityController : ControllerBase
{
    private readonly IMediator _mediator;
    public AvailabilityController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Guid tenantId,
        [FromQuery] Guid staffId,
        [FromQuery] Guid serviceId,
        [FromQuery] DateOnly dateUtc,
        [FromQuery] int slotMinutes = 15,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetAvailabilityV2Query(tenantId, staffId, serviceId, dateUtc, slotMinutes),
            ct);

        return Ok(result);
    }
}
