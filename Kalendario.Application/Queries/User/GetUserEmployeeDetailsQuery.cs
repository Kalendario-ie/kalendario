using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.User;
using Kalendario.Application.Results.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.User;

public class GetUserEmployeeDetailsQuery : IKalendarioProtectedQuery<GetUserEmployeeDetailsResult>
{
    public class Handler : IRequestHandler<GetUserEmployeeDetailsQuery, GetUserEmployeeDetailsResult>
    {
        private readonly IKalendarioDbContext _kalendarioDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public Handler(IKalendarioDbContext kalendarioDbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _kalendarioDbContext = kalendarioDbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<GetUserEmployeeDetailsResult> Handle(GetUserEmployeeDetailsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.EmployeeId == Guid.Empty)
            {
                throw new NotFoundException(nameof(_currentUserService.EmployeeId), _currentUserService.UserId);
            }

            var employee = await _kalendarioDbContext.Employees
                .Include(e => e.Services)
                .Include(e => e.Schedule)
                .ThenInclude(s => s.Frames)
                .FirstOrDefaultAsync(e => e.Id == _currentUserService.EmployeeId, cancellationToken);

            if (employee == default)
            {
                throw new NotFoundException(nameof(_currentUserService.EmployeeId), _currentUserService.EmployeeId);
            }

            return new GetUserEmployeeDetailsResult
            {
                Employee = _mapper.Map<EmployeeUserResourceModel>(employee)
            };
        }
    }
}