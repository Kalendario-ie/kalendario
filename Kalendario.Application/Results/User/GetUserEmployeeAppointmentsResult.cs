using System.Collections.Generic;
using Kalendario.Application.ResourceModels.User;

namespace Kalendario.Application.Results.User;

public class GetUserEmployeeAppointmentsResult
{
    public IEnumerable<AppointmentUserResourceModel> Entities { get; set; }
}