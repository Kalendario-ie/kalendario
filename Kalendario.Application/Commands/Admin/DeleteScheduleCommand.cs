using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Schedule), $"{Schedule.DeleteRole}")]
public class DeleteScheduleCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteScheduleCommand, Schedule>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }

        protected override Task ExtraDeletes(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}