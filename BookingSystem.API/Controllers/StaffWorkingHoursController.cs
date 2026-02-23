using BookingSystem.Application.DTO.WorkingHours.Staff;
using BookingSystem.Application.Features.WorkingHours.Staff.Commands.CreateStaffWorkingHoursDay;
using BookingSystem.Application.Features.WorkingHours.Staff.Commands.DeleteStaffWorkingHoursDay;
using BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;
using BookingSystem.Application.Features.WorkingHours.Staff.Queries.GetStaffWorkingHours;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId}/staff/{staffId:guid}/working-hours")]
public sealed class StaffWorkingHoursController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffWorkingHoursController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<PatchWorkingHoursDayDto>>> Get(Guid staffId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStaffWorkingHoursQuery(staffId), ct));

    [HttpPatch("{dayOfWeek:int}")]
    public async Task<ActionResult<PatchWorkingHoursDayDto>> Patch(
        Guid staffId,
        int dayOfWeek, 
        [FromBody] PatchWorkingHoursDayDto body, 
        CancellationToken ct)
    {
        var result = await _mediator.Send(new PatchStaffWorkingHoursCommand(staffId, dayOfWeek, body), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDay(Guid tenantId, Guid staffId, [FromBody] CreateStaffWorkingHoursDayDto body, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateStaffWorkingHoursDayCommand(tenantId, staffId, body), ct);
        return Created($"/api/tenants/{tenantId}/staff/{staffId}/working-hours/{result.DayOfWeek}", result);
    } 

    [HttpDelete("{dayOfWeek:int}")]
    public async Task<IActionResult> DeleteDay(Guid tenantId, Guid staffId, int dayOfWeek, CancellationToken ct)
    {
        await _mediator.Send(new DeleteStaffWorkingHoursDayCommand(tenantId, staffId, dayOfWeek), ct);
        return NoContent();
    }
}
