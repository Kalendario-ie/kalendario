using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class AppointmentsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAppointmentsResult>> Get([FromQuery] GetAppointmentsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentAdminResourceModel>> Create([FromBody] UpsertAppointmentCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AppointmentAdminResourceModel>> Update(Guid id,
        [FromBody] UpsertAppointmentCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
    
    
    [HttpPost("lock")]
    public async Task<ActionResult<AppointmentAdminResourceModel>> CreateTimeLock([FromBody] UpsertTimeLockCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("lock/{id}")]
    public async Task<ActionResult<AppointmentAdminResourceModel>> UpdateTimeLock(Guid id,
        [FromBody] UpsertTimeLockCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }

    [HttpGet("history/{id}")]
    public async Task<ActionResult<GetAppointmentHistoryResult>> History(Guid id)
    {
        return await Mediator.Send(new GetAppointmentHistoryQuery {Id = id});
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteAppointmentCommand
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }
}