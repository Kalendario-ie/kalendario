using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Domain;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Customer), Customer.ViewRole)]
public class GetCustomersRequest : BaseGetAllRequest<CustomerAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetCustomersRequest, Customer, CustomerAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<Customer> FilterEntities(IQueryable<Customer> entities, GetCustomersRequest request)
        {
            return entities.Where(e => e.Name.ToLowerInvariant().Contains(request.Search.ToLowerInvariant()));
        }
    }
}