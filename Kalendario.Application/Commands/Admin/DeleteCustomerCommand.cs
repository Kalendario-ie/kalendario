using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Customer), $"{Customer.DeleteRole}")]
public class DeleteCustomerCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteCustomerCommand, Customer>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }

        protected override Task ExtraDeletes(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}