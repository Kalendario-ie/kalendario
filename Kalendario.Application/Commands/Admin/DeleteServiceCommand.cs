using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;

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
        protected override async Task ExtraDeletes(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var empServices = await Context.EmployeeServices
                .Where(s => s.ServiceId == request.Id)
                .ToListAsync(cancellationToken);
            Context.EmployeeServices.RemoveRange(empServices);
        }
    }
}