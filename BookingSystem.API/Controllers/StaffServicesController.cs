using BookingSystem.Application.Features.StaffServices.Commands.SetStaffServices;
using BookingSystem.Application.Features.StaffServices.Commands.ToggleStaffService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/staff/{staffId:guid}/services")]
public sealed class StaffServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffServicesController(IMediator mediator) => _mediator = mediator;

    [HttpPut]
    public async Task<IActionResult> Set(
        Guid tenantId,
        Guid staffId,
        [FromBody] List<Guid> serviceIds,
        CancellationToken ct)
    {
        await _mediator.Send(new SetStaffServicesCommand(tenantId, staffId, serviceIds), ct);
        return NoContent();
    }


    [HttpPut("{serviceId:guid}/toggle")]
    public async Task<IActionResult> Toggle(Guid tenantId, Guid staffId, Guid serviceId, CancellationToken ct)
    {
        var result = await _mediator.Send(new ToggleStaffServiceCommand(tenantId, staffId, serviceId), ct);
        return Ok(result); // { "assigned": true/false }
    }
}
