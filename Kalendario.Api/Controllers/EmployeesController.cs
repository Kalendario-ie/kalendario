using Kalendario.Application.Commands.Admin;
using Kalendario.Application.Common.Exceptions;
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
    public async Task<ActionResult<EmployeeAdminResourceModel>> Update(Guid id,
        [FromBody] UpsertEmployeeCommand command)
    {
        command.Id = id;
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteEmployeeCommand
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPost("UploadFile/{id}")]
    public async Task<ActionResult<EmployeeAdminResourceModel>> UploadFile(Guid id, IFormFile formFile)
    {
        if (formFile.Length == 0)
            throw new BadRequestException("Invalid file.");

        var stream = formFile.OpenReadStream();
        try
        {
            var command = new UpsertEmployeePhotoCommand {Id = id, Image = stream};
            return await Mediator.Send(command);
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }
}