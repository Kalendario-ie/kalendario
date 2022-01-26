using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

public class GetAllRolesQuery : IKalendarioProtectedQuery<IEnumerable<RoleAdminResourceModel>>
{
    public class Handler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleAdminResourceModel>>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IKalendarioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleAdminResourceModel>> Handle(GetAllRolesQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Roles
                .Select(domain => _mapper.Map<RoleAdminResourceModel>(domain))
                .ToListAsync(cancellationToken);
        }
    }
}