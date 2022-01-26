using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Models;
using Kalendario.Core.Events;
using MediatR;

namespace Kalendario.Application.EventHandlers;

public class ApplicationUserRoleGroupUpdatedEventHandler : INotificationHandler<DomainEventNotification<ApplicationUserRoleGroupUpdatedEvent>>
{
    private readonly IIdentityService _identityService;

    public ApplicationUserRoleGroupUpdatedEventHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(DomainEventNotification<ApplicationUserRoleGroupUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        var user = notification.DomainEvent.User;
        await _identityService.UpdateUserRoles(user, cancellationToken);
    }
}