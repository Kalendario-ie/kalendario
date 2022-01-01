using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Service), Service.ViewRole)]
public class GetServicesRequest : BaseGetAllRequest<ServiceAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetServicesRequest, Service, ServiceAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService) 
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<Service> FilterEntities(IQueryable<Service> entities,
            GetServicesRequest request)
        {
            return entities.Where(e => e.Name.ToLowerInvariant().Contains(request.Search.ToLowerInvariant()));
        }
    }
}