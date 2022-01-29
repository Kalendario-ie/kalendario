using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(ServiceCategory), $"{ServiceCategory.DeleteRole}")]
public class DeleteServiceCategoryCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteServiceCategoryCommand, ServiceCategory>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }

        protected override Task ExtraDeletes(DeleteServiceCategoryCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}