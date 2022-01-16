using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Results;
using Kalendario.Core.Entities;
using Kalendario.Core.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Queries.Admin;

[Authorize(typeof(Appointment), Appointment.ViewRole)]
public class GetAppointmentHistoryQuery : IKalendarioProtectedQuery<GetAppointmentHistoryResult>
{
    public Guid Id { get; set; }

    public class Handler : IRequestHandler<GetAppointmentHistoryQuery, GetAppointmentHistoryResult>
    {
        private readonly IKalendarioDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserManager _currentUserManager;

        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
        {
            _context = context;
            _mapper = mapper;
            _currentUserManager = currentUserManager;
        }

        public async Task<GetAppointmentHistoryResult> Handle(GetAppointmentHistoryQuery request,
            CancellationToken cancellationToken)
        {
            await ValidateAccess(request, cancellationToken);

            var auditEntities = await _context.AuditEntities
                .Where(a => a.EntityTable == nameof(_context.Appointments))
                .Where(a => a.EntityId == request.Id.ToString())
                .ToListAsync(cancellationToken);

            var result = new GetAppointmentHistoryResult();

            foreach (var auditEntity in auditEntities.OrderByDescending(e => e.DateCreated).ThenBy(e => e.DateModified))
            {
                var entity = auditEntity.Deserialize<Appointment>();

                ApplicationUser applicationUser = null;

                if (entity.CustomerId.HasValue)
                    entity.Customer = await _context.Customers.FindAsync(entity.CustomerId);

                if (entity.EmployeeId != Guid.Empty)
                    entity.Employee = await _context.Employees.FindAsync(entity.EmployeeId);

                if (entity.ServiceId.HasValue)
                    entity.Service = await _context.Services.FindAsync(entity.ServiceId);

                if (entity.UserCreated != null)
                    applicationUser = await _context.Users.FindAsync(entity.UserCreated);

                if (entity.UserModified != null)
                    applicationUser = await _context.Users.FindAsync(entity.UserModified);

                var model = _mapper.Map<AppointmentHistoryAdminResourceModel>(entity);

                if (applicationUser != null)
                    model.User = _mapper.Map<ApplicationUserAdminResourceModel>(applicationUser);

                model.EntityState = auditEntity.EntityState;
                model.Date = entity.DateCreated ?? entity.DateModified ?? DateTime.Now;
                result.Entities.Add(model);
            }

            return result;
        }

        private async Task ValidateAccess(GetAppointmentHistoryQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FindAsync(new object[] {request.Id}, cancellationToken);

            if (appointment == null || appointment.AccountId != _currentUserManager.CurrentUserAccountId)
                throw new NotFoundException(nameof(Appointment), request.Id);
        }
    }
}