using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

public class GetAppointmentsRequest : IQuery<GetAppointmentsResult>
{
    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public Guid CustomerId { get; set; }

    public Guid EmployeeId { get; set; }
        
    public class Handler : IRequestHandler<GetAppointmentsRequest, GetAppointmentsResult>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IKalendarioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAppointmentsResult> Handle(GetAppointmentsRequest request, CancellationToken cancellationToken)
        {
            var appointments = _context.Appointments
                .Where(a => a.Start >= request.From && a.End <= request.To);

            if (request.CustomerId != Guid.Empty)
            {
                appointments = appointments.Where(a => a.CustomerId == request.CustomerId);
            }

            if (request.EmployeeId != Guid.Empty)
            {
                appointments = appointments.Where(a => a.EmployeeId == request.EmployeeId);
            }

            return new GetAppointmentsResult
            {
                Appointments = await appointments
                    .Select(a => _mapper.Map<AppointmentAdminResourceModel>(a))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}