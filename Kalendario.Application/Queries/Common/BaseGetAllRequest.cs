using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Common
{
    public abstract class BaseGetAllRequest<TResourceModel> : IKalendarioProtectedQuery<GetAllResult<TResourceModel>>
    {
        public string Search { get; set; }

        public int Start { get; set; } = 0;

        public int Length { get; set; } = 100;
    }

    public abstract class BaseGetAllRequestHandler<TRequest, TDomain, TResourceModel> :
        IRequestHandler<TRequest, GetAllResult<TResourceModel>>
        where TRequest : BaseGetAllRequest<TResourceModel>
        where TDomain : class
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;

        protected BaseGetAllRequestHandler(IKalendarioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAllResult<TResourceModel>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllResult<TResourceModel>();
            IQueryable<TDomain> entities = _context.GetDbSet<TDomain>(_context);

            result.TotalCount = await entities.CountAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                entities = FilterEntities(entities, request);
            }

            result.FilteredCount = await entities.CountAsync(cancellationToken);

            entities = entities.Skip(request.Start).Take(request.Length);

            result.Entities = await entities
                .Select(domain => _mapper.Map<TResourceModel>(domain))
                .ToListAsync(cancellationToken);

            return result;
        }

        protected abstract IQueryable<TDomain> FilterEntities(IQueryable<TDomain> entities, TRequest request);
    }
}