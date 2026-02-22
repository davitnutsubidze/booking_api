using BookingSystem.Application.DTO.Staff;
using BookingSystem.Application.Features.Staff.Commands.CreateStaff;
using BookingSystem.Application.Features.Staff.Commands.DeleteStaff;
using BookingSystem.Application.Features.Staff.Commands.UpdateStaff;
using BookingSystem.Application.Features.Staff.Queries.GetStaffById;
using BookingSystem.Application.Features.Staff.Queries.GetStaffList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/staff")]
public sealed class StaffController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> List(Guid tenantId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStaffListQuery(tenantId), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid tenantId, Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStaffByIdQuery(tenantId, id), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid tenantId, [FromBody] CreateStaffDto body, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateStaffCommand(tenantId, body), ct);
        return Created($"/api/tenants/{tenantId}/staff/{result.Id}", result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid tenantId, Guid id, [FromBody] UpdateStaffDto body, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStaffCommand(tenantId, id, body), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid tenantId, Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteStaffCommand(tenantId, id), ct);
        return NoContent();
    }
}
