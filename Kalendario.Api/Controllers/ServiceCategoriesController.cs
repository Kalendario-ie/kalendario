using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class ServiceCategoriesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<ServiceCategoryAdminResourceModel>>> Get([FromQuery] GetServiceCategoriesQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceCategoryAdminResourceModel>> Create([FromBody] UpsertServiceCategoryCommand command)
    {
        return await Mediator.Send(command);
    }
    
        
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceCategoryAdminResourceModel>> Update(Guid id, [FromBody] UpsertServiceCategoryCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }
}