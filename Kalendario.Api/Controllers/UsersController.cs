using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Queries.Admin;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllResult<ApplicationUserAdminResourceModel>>> Get([FromQuery] GetApplicationUsersQuery query)
    {
        return await Mediator.Send(query);
    }
}