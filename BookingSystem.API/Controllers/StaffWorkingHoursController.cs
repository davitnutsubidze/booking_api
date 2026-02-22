using BookingSystem.Application.DTOs.WorkingHours;
using BookingSystem.Application.Features.WorkingHours.Staff.Commands.CreateStaffWorkingHoursDay;
using BookingSystem.Application.Features.WorkingHours.Staff.Commands.DeleteStaffWorkingHoursDay;
using BookingSystem.Application.Features.WorkingHours.Staff.Commands.setStaffWorkingHours;
using BookingSystem.Application.Features.WorkingHours.Staff.Queries.GetStaffWorkingHours;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/staff/{staffId:guid}/working-hours")]
public sealed class StaffWorkingHoursController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffWorkingHoursController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<StaffWorkingHoursDayDto>>> Get(Guid staffId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStaffWorkingHoursQuery(staffId), ct));

    [HttpPut]
    public async Task<IActionResult> Put(Guid staffId, [FromBody] List<StaffWorkingHoursDayDto> days, CancellationToken ct)
    {
        await _mediator.Send(new SetStaffWorkingHoursCommand(staffId, days), ct);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDay(Guid tenantId, Guid staffId, [FromBody] CreateStaffWorkingHoursDayDto body, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateStaffWorkingHoursDayCommand(tenantId, staffId, body), ct);
        return Created($"/api/tenants/{tenantId}/staff/{staffId}/working-hours/{id}", new { id });
    }

    [HttpDelete("{dayOfWeek:int}")]
    public async Task<IActionResult> DeleteDay(Guid tenantId, Guid staffId, int dayOfWeek, CancellationToken ct)
    {
        await _mediator.Send(new DeleteStaffWorkingHoursDayCommand(tenantId, staffId, dayOfWeek), ct);
        return NoContent();
    }
}
