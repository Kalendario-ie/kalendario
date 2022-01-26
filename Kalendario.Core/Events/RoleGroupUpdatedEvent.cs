using Kalendario.Core.Common;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Core.Events;

public class RoleGroupUpdatedEvent : DomainEvent
{
    public ApplicationRoleGroup RoleGroup { get; }

    public RoleGroupUpdatedEvent(ApplicationRoleGroup roleGroup)
    {
        RoleGroup = roleGroup;
    }   
}