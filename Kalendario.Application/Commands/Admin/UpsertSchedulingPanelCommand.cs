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
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(SchedulingPanel), $"{SchedulingPanel.CreateRole},{SchedulingPanel.UpdateRole}")]
public class UpsertSchedulingPanelCommand : BaseUpsertAdminCommand<SchedulingPanelAdminResourceModel>
{
    public string Name { get; set; }
    public IEnumerable<Guid> EmployeeIds { get; set; }

    public class Handler : BaseUpsertAdminCommandHandler<UpsertSchedulingPanelCommand, SchedulingPanel,
        SchedulingPanelAdminResourceModel>
    {
        private List<Employee> Employees;

        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<SchedulingPanel> Entities => Context.SchedulingPanels.Include(s => s.Employees);

        protected override void UpdateDomain(SchedulingPanel domain, UpsertSchedulingPanelCommand request)
        {
            domain.Name = request.Name;
            domain.Employees = Employees;
        }

        protected override async Task AdditionalValidation(UpsertSchedulingPanelCommand request,
            CancellationToken cancellationToken)
        {
            Employees = await Context.Employees
                .Where(e => request.EmployeeIds.Contains(e.Id) &&
                            e.AccountId == CurrentUserManager.CurrentUserAccountId)
                .ToListAsync(cancellationToken);

            if (request.EmployeeIds.Except(Employees.Select(e => e.Id)).Any())
                throw new ValidationException(new List<ValidationFailure>
                {
                    new(nameof(request.EmployeeIds), "One or more employees do not exist")
                });
        }
    }
}
