using BookingSystem.Application.Features.Appointments.Commands.CreateAppointment;
using BookingSystem.Application.Features.Appointments.Commands.UpdateAppointmentStatus;
using BookingSystem.Application.Features.Appointments.Queries.GetAppointmentById;
using BookingSystem.Application.Features.Appointments.Queries.GetAppointmentsRange;
using BookingSystem.Application.Features.Availability.Queries.GetAvailabilityV2;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return Ok(new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _mediator.Send(new GetAppointmentByIdQuery(id), ct);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetRange(
        [FromQuery] Guid tenantId,
        [FromQuery] DateTime fromUtc,
        [FromQuery] DateTime toUtc,
        CancellationToken ct)
    {
        var list = await _mediator.Send(new GetAppointmentsRangeQuery(tenantId, fromUtc, toUtc), ct);
        return Ok(list);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAppointmentStatusRequest body, CancellationToken ct)
    {
        await _mediator.Send(new UpdateAppointmentStatusCommand(id, body.Status), ct);
        return NoContent();
    }

}

public sealed record UpdateAppointmentStatusRequest(Booking.Domain.Enums.AppointmentStatus Status);
