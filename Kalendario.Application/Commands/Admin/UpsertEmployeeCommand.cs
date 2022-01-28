using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Application.Validators;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Employee), $"{Employee.CreateRole},{Employee.UpdateRole}")]
public class UpsertEmployeeCommand : BaseUpsertAdminCommand<EmployeeAdminResourceModel>
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Guid? ScheduleId { get; set; }

    public List<Guid> Services { get; set; }


    public class Handler : BaseUpsertAdminCommandHandler<UpsertEmployeeCommand, Employee, EmployeeAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<Employee> Entities => Context.Employees.Include(e => e.EmployeeServices);

        protected override void UpdateDomain(Employee domain, UpsertEmployeeCommand request)
        {
            domain.Name = request.Name;
            domain.Email = request.Email;
            domain.PhoneNumber = request.PhoneNumber;
            domain.ScheduleId = request.ScheduleId;
            domain.EmployeeServices = request.Services
                .Select(id => new EmployeeService {ServiceId = id, AccountId = CurrentUserManager.CurrentUserAccountId})
                .ToList();
        }

        protected override async Task AdditionalValidation(UpsertEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.ScheduleId.HasValue)
                await ValidationUtils.ThrowIfNotExist<Schedule>(request.ScheduleId.Value, Context, CurrentUserManager);

            var services = await Context.Services
                .Where(s => request.Services.Contains(s.Id) && s.AccountId == CurrentUserManager.CurrentUserAccountId)
                .ToListAsync(cancellationToken);

            // This will check if there is any GUID in request.Services that is missing from the services retrieved from the database.
            if (request.Services.Except(services.Select(s => s.Id)).Any())
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new(nameof(request.Services), "One or more services do not exist")
                });
            }
        }
    }
}