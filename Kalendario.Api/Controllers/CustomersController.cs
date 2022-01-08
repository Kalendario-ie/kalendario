using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class CustomersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<CustomerAdminResourceModel>>> Get([FromQuery] GetCustomersQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<CustomerAdminResourceModel>> Create([FromBody] UpsertCustomerCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerAdminResourceModel>> Update(Guid id, [FromBody] UpsertCustomerCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
}