using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(SchedulingPanel), SchedulingPanel.ViewRole)]
public class GetSchedulingPanelsQuery : BaseGetAllQuery<SchedulingPanelAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetSchedulingPanelsQuery, SchedulingPanel, SchedulingPanelAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService) 
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<SchedulingPanel> Entities => Context.SchedulingPanels.Include(s => s.Employees);

        protected override IQueryable<SchedulingPanel> FilterEntities(IQueryable<SchedulingPanel> entities,
            GetSchedulingPanelsQuery query)
        {
            return entities.Where(e => e.Name.ToLower().Contains(query.Search.ToLower()));
        }
    }
}