using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class EmployeesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<EmployeeAdminResourceModel>>> Get([FromQuery] GetEmployeesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<EmployeeAdminResourceModel>> Create([FromBody] UpsertEmployeeCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeAdminResourceModel>> Update(Guid id, [FromBody] UpsertEmployeeCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
}