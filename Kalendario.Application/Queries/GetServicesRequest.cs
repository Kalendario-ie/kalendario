using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Queries.Common;
using Kalendario.Application.ResourceModel;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Queries
{
    public class GetServicesRequest : BaseGetAllRequest<ServiceResourceModel>
    {
        public class Handler : BaseGetAllRequestHandler<GetServicesRequest, Service, ServiceResourceModel>
        {
            public Handler(IKalendarioDbContext context, IMapper mapper) : base(context, mapper)
            {
            }

            protected override IQueryable<Service> FilterEntities(IQueryable<Service> entities,
                GetServicesRequest request)
            {
                return entities.Where(e => e.Name.ToLowerInvariant().Contains(request.Search.ToLowerInvariant()));
            }
        }
    }
}