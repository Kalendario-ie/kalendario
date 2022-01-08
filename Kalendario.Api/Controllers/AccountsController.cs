using Kalendario.Application.Commands.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers;

[ApiController]
public class AccountsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateAccountCommand command)
    {
        return await Mediator.Send(command);
    }
}