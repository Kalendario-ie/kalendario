using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Extensions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

public class GetAppointmentsQuery : IKalendarioProtectedQuery<GetAppointmentsResult>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public Guid? CustomerId { get; set; }

    public IEnumerable<Guid> EmployeeIds { get; set; }

    public class Handler : IRequestHandler<GetAppointmentsQuery, GetAppointmentsResult>
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

        public async Task<GetAppointmentsResult> Handle(GetAppointmentsQuery query, CancellationToken cancellationToken)
        {
            var appointments = _context.Appointments
                .Where(a => a.AccountId == _currentUserService.AccountId)
                .BetweenDates(query.FromDate, query.ToDate);
                

            if (query.CustomerId.HasValue && query.CustomerId != Guid.Empty)
            {
                appointments = appointments.Where(a => a.CustomerId == query.CustomerId);
            }

            if (query.EmployeeIds.Any())
            {
                appointments = appointments.Where(a => query.EmployeeIds.Contains(a.EmployeeId));
            }

            return new GetAppointmentsResult
            {
                Entities = await appointments
                    .Select(a => _mapper.Map<AppointmentAdminResourceModel>(a))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}