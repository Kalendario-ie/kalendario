using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class SchedulingPanelsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<SchedulingPanelAdminResourceModel>>> Get([FromQuery] GetSchedulingPanelsQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<SchedulingPanelAdminResourceModel>> Create([FromBody] UpsertSchedulingPanelCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<SchedulingPanelAdminResourceModel>> Update(Guid id, [FromBody] UpsertSchedulingPanelCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteSchedulingPanelCommand
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }
}