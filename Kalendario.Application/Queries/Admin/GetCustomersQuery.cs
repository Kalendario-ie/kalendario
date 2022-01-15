using System.Linq;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.Queries.Admin.Common;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Customer), Customer.ViewRole)]
public class GetCustomersQuery : BaseGetAllQuery<CustomerAdminResourceModel>
{
    public class Handler : BaseGetAllRequestHandler<GetCustomersQuery, Customer, CustomerAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
            : base(context, mapper, currentUserService)
        {
        }

        protected override IQueryable<Customer> Entities => Context.Customers;

        protected override IQueryable<Customer> FilterEntities(IQueryable<Customer> entities, GetCustomersQuery query)
        {
            return entities.Where(e => e.Name.ToLower().Contains(query.Search.ToLower()));
        }
    }
}