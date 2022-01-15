using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Schedule), Schedule.ViewRole)]
public class GetSchedulesQuery : BaseGetAllQuery<ScheduleAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetSchedulesQuery, Schedule, ScheduleAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<Schedule> Entities => Context.Schedules.Include(s => s.Frames);

        protected override IQueryable<Schedule> FilterEntities(IQueryable<Schedule> entities,
            GetSchedulesQuery query)
        {
            return entities.Where(e => e.Name.ToLower().Contains(query.Search.ToLower()));
        }
    }
}