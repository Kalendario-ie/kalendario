using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class SchedulesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<ScheduleAdminResourceModel>>> Get([FromQuery] GetSchedulesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<ScheduleAdminResourceModel>> Create([FromBody] UpsertScheduleCommand command)
    {
        return await Mediator.Send(command);
    }
    
        
    [HttpPut("{id}")]
    public async Task<ActionResult<ScheduleAdminResourceModel>> Update(Guid id, [FromBody] UpsertScheduleCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteScheduleCommand
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }
}