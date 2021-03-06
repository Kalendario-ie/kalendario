using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Employee), Employee.ViewRole)]
public class GetEmployeesQuery : BaseGetAllQuery<EmployeeAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetEmployeesQuery, Employee, EmployeeAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<Employee> Entities => Context.Employees.Include(e => e.EmployeeServices);

        protected override IQueryable<Employee> FilterEntities(IQueryable<Employee> entities, GetEmployeesQuery query)
        {
            return entities.Where(e => e.Name.ToLower().Contains(query.Search.ToLower()));
        }
    }
}