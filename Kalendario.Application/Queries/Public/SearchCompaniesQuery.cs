using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.Public;
using Kalendario.Application.Results.Public;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Public;

public class SearchCompaniesQuery : IPublicQuery<SearchCompaniesResult>
{
    public string Search { get; set; }
    
    public class Handler : IRequestHandler<SearchCompaniesQuery, SearchCompaniesResult>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IKalendarioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SearchCompaniesResult> Handle(SearchCompaniesQuery request, CancellationToken cancellationToken)
        {
            return new SearchCompaniesResult()
            {
                Entities = await _context.Accounts.Where(a => a.Name.ToLower().Contains(request.Search.ToLower()))
                    .ProjectTo<CompanySearchResourceModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
            };
        }
    }
}