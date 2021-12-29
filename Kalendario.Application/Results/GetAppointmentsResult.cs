using System.Collections.Generic;
using Kalendario.Application.ResourceModels.Admin;

namespace Kalendario.Application.Results;

public class GetAppointmentsResult
{
    public List<AppointmentAdminResourceModel> Appointments { get; set; }
}