using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Models;
using Kalendario.Core.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kalendario.Application.EventHandlers;

public class RoleGroupUpdatedEventHandler : INotificationHandler<DomainEventNotification<RoleGroupUpdatedEvent>>
{
    private readonly IIdentityService _identityService;
    private readonly IKalendarioDbContext _context;

    public RoleGroupUpdatedEventHandler(IIdentityService identityService, IKalendarioDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    public async Task Handle(DomainEventNotification<RoleGroupUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        var roleGroup = notification.DomainEvent.RoleGroup;
        var users = await _context.Users
            .Where(u => u.RoleGroupId == roleGroup.Id)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            await _identityService.UpdateUserRoles(user, cancellationToken);
        }
    }
}