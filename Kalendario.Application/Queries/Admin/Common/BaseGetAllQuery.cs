using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Results;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin.Common
{
    public abstract class BaseGetAllQuery<TResourceModel> : IKalendarioProtectedQuery<GetAllResult<TResourceModel>>
    {
        public string Search { get; set; }

        public int Start { get; set; } = 0;

        public int Length { get; set; } = 100;
    }

    public abstract class BaseGetAllRequestHandler<TRequest, TDomain, TResourceModel> :
        IRequestHandler<TRequest, GetAllResult<TResourceModel>>
        where TRequest : BaseGetAllQuery<TResourceModel>
        where TDomain : AccountEntity
    {
        protected readonly IKalendarioDbContext Context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        protected BaseGetAllRequestHandler(
            IKalendarioDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            Context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<GetAllResult<TResourceModel>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllResult<TResourceModel>();
            var entities = Entities
                .Where(e => e.AccountId == _currentUserService.AccountId);

            result.TotalCount = await entities.CountAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                entities = FilterEntities(entities, request);
            }

            result.FilteredCount = await entities.CountAsync(cancellationToken);

            entities = entities.Skip(request.Start).Take(request.Length).OrderBy(domain => domain.Id);

            result.Entities = await entities
                .Select(domain => _mapper.Map<TResourceModel>(domain))
                .ToListAsync(cancellationToken);

            return result;
        }

        protected abstract IQueryable<TDomain> Entities { get; }

        protected abstract IQueryable<TDomain> FilterEntities(IQueryable<TDomain> entities, TRequest request);
    }
}