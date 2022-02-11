using Kalendario.Application.Queries.User;
using Kalendario.Application.Results.User;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers.User;

[ApiController]
public class UserEmployeeController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetUserEmployeeDetailsResult>> Get()
    {
        return await Mediator.Send(new GetUserEmployeeDetailsQuery());
    }

    [HttpGet("Appointments")]
    public async Task<ActionResult<GetUserEmployeeAppointmentsResult>> Appointments(
        [FromQuery] GetUserEmployeeAppointmentsQuery query)
    {
        return await Mediator.Send(query);
    }
}