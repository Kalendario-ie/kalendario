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
using Kalendario.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(ApplicationRoleGroup), $"{ApplicationRoleGroup.CreateRole},{ApplicationRoleGroup.UpdateRole}")]
public class UpsertApplicationRoleGroupCommand : BaseUpsertAdminCommand<RoleGroupAdminResourceModel>
{
    public string Name { get; set; }
    public List<string> Roles { get; set; }

    public class Handler : BaseUpsertAdminCommandHandler<UpsertApplicationRoleGroupCommand, ApplicationRoleGroup,
        RoleGroupAdminResourceModel>
    {
        public Handler(IKalendarioDbContext context, IMapper mapper, ICurrentUserManager currentUserManager)
            : base(context, mapper, currentUserManager)
        {
        }

        protected override IQueryable<ApplicationRoleGroup> Entities => Context.RoleGroups.Include(s => s.Roles);

        protected override void UpdateDomain(ApplicationRoleGroup domain, UpsertApplicationRoleGroupCommand request)
        {
            domain.Name = request.Name;

            domain.RoleGroupRoles = request.Roles
                .Select(id => new RoleGroupRole {RoleId = id})
                .ToList();
        }

        protected override async Task AdditionalValidation(UpsertApplicationRoleGroupCommand request,
            CancellationToken cancellationToken)
        {
            var roles = await Context.Roles
                .Where(s => request.Roles.Contains(s.Id))
                .ToListAsync(cancellationToken);

            // This will check if there is any GUID in request.Roles that is missing from the roles retrieved from the database.
            if (request.Roles.Except(roles.Select(s => s.Id)).Any())
            {
                throw new ValidationException(new List<ValidationFailure>
                {
                    new(nameof(request.Roles), "One or more roles do not exist")
                });
            }
        }
    }
}