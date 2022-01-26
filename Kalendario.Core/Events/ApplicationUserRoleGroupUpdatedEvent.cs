using Kalendario.Core.Common;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Core.Events;

public class ApplicationUserRoleGroupUpdatedEvent : DomainEvent
{
    public ApplicationUser User { get; }

    public ApplicationUserRoleGroupUpdatedEvent(ApplicationUser user)
    {
        User = user;
    }
}