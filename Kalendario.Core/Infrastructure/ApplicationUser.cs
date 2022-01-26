using System;
using System.Collections.Generic;
using Kalendario.Core.Common;
using Kalendario.Core.Entities;
using Kalendario.Core.Events;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Core.Infrastructure;

public class ApplicationUser : IdentityUser, IHasDomainEvent
{
    public Guid? AccountId { get; set; }
    public Account Account { get; set; }

    private Guid? _roleGroupId;

    public Guid? RoleGroupId
    {
        get => _roleGroupId;
        set
        {
            if (value.HasValue && value.Value != _roleGroupId)
            {
                DomainEvents.Add(new ApplicationUserRoleGroupUpdatedEvent(this));
            }

            _roleGroupId = value;
        }
    }

    public ApplicationRoleGroup RoleGroup { get; set; }

    public Guid? EmployeeId { get; set; }

    public Employee Employee { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new();
}