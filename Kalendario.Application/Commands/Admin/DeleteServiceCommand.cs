using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Service), $"{Service.DeleteRole}")]
public class DeleteServiceCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteServiceCommand, Service>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }
    }
}