using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class RoleGroupsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<RoleGroupAdminResourceModel>>> Get([FromQuery] GetRoleGroupsQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("Roles")]
    public async Task<IEnumerable<RoleAdminResourceModel>> GetAllRoles([FromQuery] GetAllRolesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<RoleGroupAdminResourceModel>> Create([FromBody] UpsertApplicationRoleGroupCommand query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleGroupAdminResourceModel>> Update(Guid id, [FromBody] UpsertApplicationRoleGroupCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
}