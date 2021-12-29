using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Queries.Common;
using Kalendario.Application.ResourceModel;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Queries.Admin;

public class GetEmployeesRequest : BaseGetAllRequest<EmployeeResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetEmployeesRequest, Employee, EmployeeResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Employee> FilterEntities(IQueryable<Employee> entities, GetEmployeesRequest request)
        {
            return entities.Where(e => e.Name.ToLowerInvariant().Contains(request.Search.ToLowerInvariant()));
        }
    }
}