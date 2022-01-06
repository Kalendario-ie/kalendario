using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(ServiceCategory), ServiceCategory.ViewRole)]
public class GetServiceCategoriesQuery : BaseGetAllQuery<ServiceCategoryAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetServiceCategoriesQuery, ServiceCategory, ServiceCategoryAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService) 
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<ServiceCategory> Entities => Context.ServiceCategories;

        protected override IQueryable<ServiceCategory> FilterEntities(IQueryable<ServiceCategory> entities,
            GetServiceCategoriesQuery query)
        {
            return entities.Where(e => e.Name.ToLowerInvariant().Contains(query.Search.ToLowerInvariant()));
        }
    }
}