using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.Public;
using Kalendario.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Public;

public class FindCompanyQuery : IPublicQuery<CompanyDetailsResourceModel>
{
    public string Name { get; set; }

    public class Handler : IRequestHandler<FindCompanyQuery, CompanyDetailsResourceModel>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IKalendarioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CompanyDetailsResourceModel> Handle(FindCompanyQuery request,
            CancellationToken cancellationToken)
        {
            var model = await _context.Accounts
                .Include(a => a.Services)
                .Include(a => a.ServiceCategories)
                .FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);

            if (model == default)
            {
                throw new NotFoundException(nameof(Account), request.Name);
            }

            return _mapper.Map<CompanyDetailsResourceModel>(model);
        }
    }
}