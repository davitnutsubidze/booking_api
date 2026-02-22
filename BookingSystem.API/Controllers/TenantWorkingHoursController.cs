using BookingSystem.Application.DTOs.WorkingHours;
using BookingSystem.Application.Features.WorkingHours.Tenant.Commands.PatchTenantWorkingHours;
using BookingSystem.Application.Features.WorkingHours.Tenant.Commands.SetTenantWorkingHours;
using BookingSystem.Application.Features.WorkingHours.Tenant.Queries.GetTenantWorkingHours;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/Tenants/{tenantId:guid}/working-hours")]
public sealed class TenantWorkingHoursController : ControllerBase
{
    private readonly IMediator _mediator;
    public TenantWorkingHoursController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<WorkingHoursDayDto>>> Get(Guid tenantId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTenantWorkingHoursQuery(tenantId), ct));

    [HttpPut]
    public async Task<IActionResult> Put(Guid tenantId, [FromBody] List<WorkingHoursDayDto> days, CancellationToken ct)
    {
        await _mediator.Send(new SetTenantWorkingHoursCommand(tenantId, days), ct);
        return NoContent();
    }

    [HttpPatch("{dayOfWeek:int}")]
    public async Task<IActionResult> Patch(
        Guid tenantId,
        int dayOfWeek,
        [FromBody] PatchTenantWorkingHoursDto body,
        CancellationToken ct)
    {
        await _mediator.Send(new PatchTenantWorkingHoursCommand(tenantId, dayOfWeek, body), ct);
        return NoContent();
    }
}
