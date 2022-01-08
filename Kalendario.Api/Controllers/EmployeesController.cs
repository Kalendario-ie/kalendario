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
}