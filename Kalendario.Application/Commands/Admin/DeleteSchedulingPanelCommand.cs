using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(SchedulingPanel), $"{SchedulingPanel.DeleteRole}")]
public class DeleteSchedulingPanelCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteSchedulingPanelCommand, SchedulingPanel>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }

        protected override Task ExtraDeletes(DeleteSchedulingPanelCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}