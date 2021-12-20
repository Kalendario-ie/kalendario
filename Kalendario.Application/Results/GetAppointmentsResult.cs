using System.Collections.Generic;
using Kalendario.Application.ResourceModel;

namespace Kalendario.Application.Results
{
    public class GetAppointmentsResult
    {
        public List<AppointmentResourceModel> Appointments { get; set; }
    }
}