using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class ServicesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<ServiceAdminResourceModel>>> Get([FromQuery] GetServicesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceAdminResourceModel>> Create([FromBody] UpsertServiceCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceAdminResourceModel>> Update(Guid id, [FromBody] UpsertServiceCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteServiceCommand
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }
}