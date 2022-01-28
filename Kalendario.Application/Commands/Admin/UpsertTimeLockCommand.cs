using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Kalendario.Application.Authorization;
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
public class UpsertTimeLockCommand : BaseUpsertAdminCommand<AppointmentAdminResourceModel>
{
    private DateTime _start;
    private DateTime _end;

    public DateTime Start
    {
        get => _start.ToUniversalTime();
        set => _start = value;
    }

    public DateTime End
    {
        get => _end.ToUniversalTime();
        set => _end = value;
    }

    public Guid EmployeeId { get; set; }

    public string InternalNotes { get; set; }

    public bool IgnoreTimeClashes { get; set; }

    public class Handler : BaseUpsertAdminCommandHandler<UpsertTimeLockCommand, Appointment,
        AppointmentAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<Appointment> Entities => Context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Service)
            .Include(a => a.Employee);

        protected override void UpdateDomain(Appointment domain, UpsertTimeLockCommand request)
        {
            domain.Start = request.Start;
            domain.End = request.End;
            domain.InternalNotes = request.InternalNotes;
            domain.EmployeeId = request.EmployeeId;
        }

        protected override async Task AdditionalValidation(UpsertTimeLockCommand request,
            CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();

            var canOverbook = request.IgnoreTimeClashes && await CurrentUserManager.IsInRoleAsync(
                AuthorizationHelper.RoleName(typeof(Appointment), Appointment.CanOverbookRole));

            if (request.End <= request.Start)
                validationFailures.Add(new ValidationFailure(nameof(request.Start),
                    "Start time can't be after end time."));

            var employee = await Context.Employees
                .Include(e => e.Schedule)
                .ThenInclude(s => s.Frames)
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
            if (employee == null || employee.AccountId != CurrentUserManager.CurrentUserAccountId)
                validationFailures.Add(new ValidationFailure(nameof(request.EmployeeId), "Employee does not exist."));

            var schedule = employee?.Schedule;
            if (!canOverbook && schedule == null)
                validationFailures.Add(new ValidationFailure(nameof(request.EmployeeId), "Employee has no schedule."));

            if (!canOverbook && schedule != null && !schedule.HasAvailability(request.Start, request.End))
                validationFailures.Add(new ValidationFailure(nameof(request.EmployeeId),
                    "Employee has no availability for the selected times."));

            var hasOverlappingAppointments = await HasOverlappingAppointments(request, cancellationToken);

            if (!canOverbook && hasOverlappingAppointments)
                validationFailures.Add(new ValidationFailure(nameof(request.Start),
                    "Appointment times overlaps with other appointments."));

            if (validationFailures.Any())
                throw new ValidationException(validationFailures);
        }

        private async Task<bool> HasOverlappingAppointments(UpsertTimeLockCommand request,
            CancellationToken cancellationToken)
        {
            var entities = Context.Appointments
                .Where(a => a.EmployeeId == request.EmployeeId);

            if (request.Id.HasValue)
                entities = entities.Where(a => a.Id != request.Id);

            return await entities.BetweenDates(request.Start, request.End)
                .AnyAsync(cancellationToken);
        }
    }
}