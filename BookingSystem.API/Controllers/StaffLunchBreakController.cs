using BookingSystem.Application.DTOs.LunchBreaks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tenants/{tenantId:guid}/staff/{staffId:guid}/lunch-breaks")]
public sealed class StaffLunchBreakController : ControllerBase
{
    private readonly IMediator _mediator;

    public StaffLunchBreakController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> List(Guid tenantId, Guid staffId, CancellationToken ct)
        => Ok(await _mediator.Send(
            new GetStaffLunchBreaksQuery(tenantId, staffId), ct));


    [HttpPut("{dayOfWeek:int}")]
    public async Task<IActionResult> Upsert(
        Guid tenantId,
        Guid staffId,
        int dayOfWeek,
        UpsertStaffLunchBreakDto body,
        CancellationToken ct)
        => Ok(await _mediator.Send(
            new UpsertStaffLunchBreakCommand(
                tenantId,
                staffId,
                dayOfWeek,
                body), ct));


    [HttpDelete("{dayOfWeek:int}")]
    public async Task<IActionResult> Delete(
        Guid tenantId,
        Guid staffId,
        int dayOfWeek,
        CancellationToken ct)
    {
        await _mediator.Send(
            new DeleteStaffLunchBreakCommand(
                tenantId,
                staffId,
                dayOfWeek), ct);

        return NoContent();
    }
}