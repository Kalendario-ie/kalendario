using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Employee), $"{Employee.DeleteRole}")]
public class DeleteEmployeeCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteEmployeeCommand, Employee>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }

        protected override Task ExtraDeletes(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}