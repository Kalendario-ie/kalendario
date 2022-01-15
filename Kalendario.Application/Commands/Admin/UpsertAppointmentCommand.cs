using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Extensions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Appointment), $"{Appointment.CreateRole},{Appointment.UpdateRole}")]
public class UpsertAppointmentCommand : BaseUpsertAdminCommand<AppointmentAdminResourceModel>
{
    private DateTime _start;
    private DateTime? _end;
    public DateTime Start { get => _start.ToUniversalTime(); set => _start = value; }

    public DateTime? End { get => _end?.ToUniversalTime() ?? _end; set => _end = value; }

    public string InternalNotes { get; set; }

    public Guid CustomerId { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid ServiceId { get; set; }
    
    public bool IgnoreTimeClashes { get; set; }

    public class Handler : BaseUpsertAdminCommandHandler<UpsertAppointmentCommand, Appointment,
        AppointmentAdminResourceModel>
    {
        public double ServicePrice { get; set; }
        public DateTime EndTime { get; set; }

        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<Appointment> Entities => Context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Service)
            .Include(a => a.Employee);

        protected override void UpdateDomain(Appointment domain, UpsertAppointmentCommand request)
        {
            domain.Start = request.Start;
            domain.End = EndTime;
            domain.InternalNotes = request.InternalNotes;
            domain.CustomerId = request.CustomerId;
            domain.EmployeeId = request.EmployeeId;
            domain.ServiceId = request.ServiceId;
            domain.Price = ServicePrice;
        }

        protected override async Task AdditionalValidation(UpsertAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();

            if (request.End.HasValue && request.End.Value <= request.Start)
                validationFailures.Add(new ValidationFailure(nameof(request.Start), "Start time can't be after end time."));
            
            var customer = await Context.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken);
            if (customer == null || customer.AccountId != CurrentUserManager.CurrentUserAccountId)
                validationFailures.Add(new ValidationFailure(nameof(request.CustomerId), "Customer does not exist."));

            var employee = await Context.Employees
                .Include(s => s.Services)
                .Include(e => e.Schedule)
                .ThenInclude(s => s.Frames)
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
            if (employee == null || employee.AccountId != CurrentUserManager.CurrentUserAccountId)
                validationFailures.Add(new ValidationFailure(nameof(request.EmployeeId), "Employee does not exist."));

            var schedule = employee?.Schedule;
            if (!request.IgnoreTimeClashes && schedule == null)
                validationFailures.Add(new ValidationFailure(nameof(request.EmployeeId), "Employee has no schedule."));

            var service = employee?.Services.FirstOrDefault(s => s.Id == request.ServiceId);
            if (service == null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.CustomerId),
                    "Service does not exist or is not provided by this employee."));
                throw new ValidationException(validationFailures);
            }

            EndTime = request.End ?? request.Start.Add(service.Duration);
            if (!request.IgnoreTimeClashes && schedule != null && !schedule.HasAvailability(request.Start, EndTime))
                validationFailures.Add(new ValidationFailure(nameof(request.EmployeeId),
                    "Employee has no availability for the selected times."));

            var hasOverlappingAppointments = await Context.Appointments
                .Where(a => a.EmployeeId == request.EmployeeId && request.Id.HasValue && request.Id.Value != a.Id)
                .BetweenDates(request.Start, EndTime)
                .AnyAsync(cancellationToken);
            
            if (!request.IgnoreTimeClashes && hasOverlappingAppointments)
                validationFailures.Add(new ValidationFailure(nameof(request.Start),
                    "Appointment times overlaps with other appointments."));

            if (validationFailures.Any())
                throw new ValidationException(validationFailures);

            ServicePrice = service.Price;
        }
    }
}