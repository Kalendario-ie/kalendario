using System.Collections.Generic;
using System.Linq;
using Kalendario.Core.Common;
using Kalendario.Core.Entities;
using Kalendario.Core.Events;

namespace Kalendario.Core.Infrastructure;

public class ApplicationRoleGroup : AccountEntity, IHasDomainEvent
{
    private ICollection<RoleGroupRole> _roleGroupRoles;
    public string Name { get; set; }
    public ICollection<ApplicationRole> Roles { get; set; }

    public ICollection<RoleGroupRole> RoleGroupRoles
    {
        get => _roleGroupRoles;
        set
        {
            if (value.Select(r => r.RoleId).Except(_roleGroupRoles.Select(r => r.RoleId)).Any())
            {
                this.DomainEvents.Add(new RoleGroupUpdatedEvent(this));
            }

            _roleGroupRoles = value;
        }
    }

    public List<DomainEvent> DomainEvents { get; set; } = new();
}