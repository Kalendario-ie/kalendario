using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Queries.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Queries.Admin;

public class GetCustomersRequest : BaseGetAllRequest<CustomerAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetCustomersRequest, Customer, CustomerAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Customer> FilterEntities(IQueryable<Customer> entities, GetCustomersRequest request)
        {
            return entities.Where(e => e.Name.ToLowerInvariant().Contains(request.Search.ToLowerInvariant()));
        }
    }
}