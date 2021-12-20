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
    public abstract class BaseGetAllRequest<TResourceModel> : IQuery<GetAllResult<TResourceModel>>
    {
        public string Search { get; set; }
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
            IQueryable<TDomain> entities = _context.GetDbSet<TDomain>(_context);

            if (request.Search != null)
            {
                entities = FilterEntities(entities, request);
            }
            return new GetAllResult<TResourceModel>
            {
                Entities = await entities
                    .Select(domain => _mapper.Map<TResourceModel>(domain))
                    .ToListAsync(cancellationToken)
            };
        }

        protected abstract IQueryable<TDomain> FilterEntities(IQueryable<TDomain> entities, TRequest request);
    }
}