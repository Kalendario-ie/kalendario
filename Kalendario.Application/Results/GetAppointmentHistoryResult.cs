using System.Collections.Generic;
using Kalendario.Application.ResourceModels.Admin;

namespace Kalendario.Application.Results;

public class GetAppointmentHistoryResult
{
    public List<AppointmentHistoryAdminResourceModel> Entities { get; } = new();
}