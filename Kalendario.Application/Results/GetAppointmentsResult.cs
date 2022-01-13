using System.Collections.Generic;
using Kalendario.Application.ResourceModels.Admin;

namespace Kalendario.Application.Results;

public class GetAppointmentsResult
{
    public List<AppointmentAdminResourceModel> Entities { get; set; }
}