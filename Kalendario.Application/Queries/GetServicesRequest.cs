using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModel;
using Kalendario.Application.Results;
using Kalendario.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries
{
    public class GetServicesRequest : IQuery<GetServicesResult>
    {
        public class Handler : IRequestHandler<GetServicesRequest, GetServicesResult>
        {
            private readonly IKalendarioDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IKalendarioDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetServicesResult> Handle(GetServicesRequest request, CancellationToken cancellationToken)
            {
                return new GetServicesResult
                {
                    Services = await _context.Services
                        .Select(service => _mapper.Map<ServiceResourceModel>(service))
                        .ToListAsync(cancellationToken)
                };
            }
        }
    }
}