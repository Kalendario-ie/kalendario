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
    public class GetEmployeesRequest : IQuery<GetEmployeesResult>
    {
        public class Handler : IRequestHandler<GetEmployeesRequest, GetEmployeesResult>
        {
            private readonly IKalendarioDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IKalendarioDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetEmployeesResult> Handle(GetEmployeesRequest request, CancellationToken cancellationToken)
            {
                return new GetEmployeesResult
                {
                    Employees = await _context.Employees
                        .Select(employee => _mapper.Map<Employee, EmployeeResourceModel>(employee))
                        .ToListAsync(cancellationToken)
                };
            }
        }
    }
}