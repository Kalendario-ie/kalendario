using Kalendario.Application.Queries.Public;
using Kalendario.Application.ResourceModels.Public;
using Kalendario.Application.Results.Public;
using Microsoft.AspNetCore.Mvc;

namespace Kalendario.Api.Controllers.Public;

[ApiController]
public class CompaniesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SearchCompaniesResult>> Search([FromQuery] SearchCompaniesQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("{name}")]
    public async Task<ActionResult<CompanyDetailsResourceModel>> Find(string name)
    {
        
        return await Mediator.Send(new FindCompanyQuery() {Name = name});
    }
    
        
    [HttpGet("slots")]
    public async Task<ActionResult<FindAppointmentAvailabilityResult>> Slots([FromQuery] FindAppointmentAvailabilityQuery query)
    {
        
        return await Mediator.Send(query);
    }
}