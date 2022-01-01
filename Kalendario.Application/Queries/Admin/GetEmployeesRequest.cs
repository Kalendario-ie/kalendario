using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Employee), Employee.ViewRole)]
public class GetEmployeesRequest : BaseGetAllRequest<EmployeeAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetEmployeesRequest, Employee, EmployeeAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<Employee> FilterEntities(IQueryable<Employee> entities, GetEmployeesRequest request)
        {
            return entities.Where(e => e.Name.ToLowerInvariant().Contains(request.Search.ToLowerInvariant()));
        }
    }
}