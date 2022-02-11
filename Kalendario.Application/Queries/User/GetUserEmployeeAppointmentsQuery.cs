using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Extensions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Results.User;

public class GetUserEmployeeAppointmentsQuery : IKalendarioProtectedQuery<GetUserEmployeeAppointmentsResult>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    public class Hanlder : IRequestHandler<GetUserEmployeeAppointmentsQuery,GetUserEmployeeAppointmentsResult>
    {
        private readonly IKalendarioDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public Hanlder(IKalendarioDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        
        public async Task<GetUserEmployeeAppointmentsResult> Handle(GetUserEmployeeAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.EmployeeId == Guid.Empty)
            {
                throw new NotFoundException(nameof(_currentUserService.EmployeeId), _currentUserService.UserId);
            }

            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Where(a => a.EmployeeId == _currentUserService.EmployeeId)
                .BetweenDates(request.FromDate, request.ToDate)
                .ToListAsync(cancellationToken);

            return new GetUserEmployeeAppointmentsResult
            {
                Entities =
                    appointments.Select(appointment => _mapper.Map<AppointmentUserResourceModel>(appointment))
            };
        }
    }
}