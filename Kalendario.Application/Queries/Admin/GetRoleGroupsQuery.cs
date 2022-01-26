using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(ApplicationRoleGroup), ApplicationRoleGroup.ViewRole)]
public class GetRoleGroupsQuery : BaseGetAllQuery<RoleGroupAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetRoleGroupsQuery, ApplicationRoleGroup, RoleGroupAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<ApplicationRoleGroup> Entities => Context.RoleGroups.Include(s => s.Roles);

        protected override IQueryable<ApplicationRoleGroup> FilterEntities(IQueryable<ApplicationRoleGroup> entities,
            GetRoleGroupsQuery query)
        {
            return entities.Where(e => e.Name.ToLower().Contains(query.Search.ToLower()));
        }
    }
}