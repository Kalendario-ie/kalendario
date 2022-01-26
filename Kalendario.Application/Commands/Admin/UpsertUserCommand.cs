using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Kalendario.Application.Authorization;
using Kalendario.Application.Common.Exceptions;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Application.ResourceModels.Admin;
using Kalendario.Core.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Application.Commands.Admin;

[Authorize(AuthorizationHelper.MasterRole)]
public class UpsertUserCommand : IKalendarioProtectedCommand<ApplicationUserAdminResourceModel>
{
    [JsonIgnore] public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public Guid? RoleGroupId { get; set; }
    public Guid? EmployeeId { get; set; }

    public class Handler : IRequestHandler<UpsertUserCommand, ApplicationUserAdminResourceModel>
    {
        private readonly IKalendarioDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserManager _currentUserManager;

        public Handler(
            IKalendarioDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ICurrentUserManager currentUserManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _currentUserManager = currentUserManager;
        }

        public async Task<ApplicationUserAdminResourceModel> Handle(UpsertUserCommand request,
            CancellationToken cancellationToken)
        {
            await ThrowIfInvalidRoleGroup(request);
            await ThrowIfInvalidEmployeeId(request);
            
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var domain = await _userManager.FindByIdAsync(request.Id);

                if (domain == default)
                {
                    throw new NotFoundException(nameof(ApplicationUser), request.Id);
                }

                UpdateDomain(domain, request);

                await _userManager.UpdateAsync(domain);

                return _mapper.Map<ApplicationUserAdminResourceModel>(domain);
            }

            var newUser = new ApplicationUser() {AccountId = _currentUserManager.CurrentUserAccountId};

            UpdateDomain(newUser, request);

            await _userManager.CreateAsync(newUser);

            return _mapper.Map<ApplicationUserAdminResourceModel>(newUser);
        }

        private async Task ThrowIfInvalidRoleGroup(UpsertUserCommand request)
        {
            if (!request.RoleGroupId.HasValue)
                return;

            var roleGroup = await _context.RoleGroups.FindAsync(request.RoleGroupId.Value);

            if (roleGroup == null || !roleGroup.AccountId.Equals(_currentUserManager.CurrentUserAccountId))
                throw new ValidationException(new List<ValidationFailure>()
                {
                    new(nameof(request.RoleGroupId), "RoleGroup does not exist")
                });
        }
        
        private async Task ThrowIfInvalidEmployeeId(UpsertUserCommand request)
        {
            if (!request.EmployeeId.HasValue)
                return;

            var employee = await _context.Employees.FindAsync(request.EmployeeId.Value);

            if (employee == null || !employee.AccountId.Equals(_currentUserManager.CurrentUserAccountId))
                throw new ValidationException(new List<ValidationFailure>()
                {
                    new(nameof(request.EmployeeId), "Employee does not exist")
                });
        }


        private void UpdateDomain(ApplicationUser domain, UpsertUserCommand request)
        {
            domain.UserName = request.UserName;
            domain.Email = request.Email;
            domain.RoleGroupId = request.RoleGroupId;
            domain.EmployeeId = request.EmployeeId;
        }
    }
}