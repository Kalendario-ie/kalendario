using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Kalendario.Core.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(ApplicationUser), AuthorizationHelper.MasterRole)]
public class GetApplicationUsersQuery : IKalendarioProtectedQuery<GetAllResult<ApplicationUserAdminResourceModel>>
{
    public string Search { get; set; }

    public int Start { get; set; } = 0;

    public int Length { get; set; } = 100;
    
    public class Handler : IRequestHandler<GetApplicationUsersQuery, GetAllResult<ApplicationUserAdminResourceModel>>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<GetAllResult<ApplicationUserAdminResourceModel>> Handle(GetApplicationUsersQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAllResult<ApplicationUserAdminResourceModel>();
            var entities = _context.Users
                .Where(e => e.AccountId == _currentUserService.AccountId);

            result.TotalCount = await entities.CountAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                entities = entities.Where(u => u.NormalizedUserName.Contains(request.Search.ToUpper()));
            }

            result.FilteredCount = await entities.CountAsync(cancellationToken);

            entities = entities.OrderBy(domain => domain.Id).Skip(request.Start).Take(request.Length);

            result.Entities = await entities
                .Select(domain => _mapper.Map<ApplicationUserAdminResourceModel>(domain))
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}