using Kalendario.Application.Commands.Admin.Common;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Application.Common.Security;
using Kalendario.Core.Entities;

namespace Kalendario.Application.Commands.Admin;

[Authorize(typeof(Appointment), $"{Appointment.DeleteRole}")]
public class DeleteAppointmentCommand : BaseDeleteAdminCommand
{
    public class Handler : BaseDeleteAdminCommandHandler<DeleteAppointmentCommand, Appointment>
    {
        public Handler(IKalendarioDbContext context, ICurrentUserManager currentUserManager)
            : base(context, currentUserManager)
        {
        }
    }
}