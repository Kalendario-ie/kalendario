using System;
using Kalendario.Application.Common.Interfaces;
using Kalendario.Core.Domain;

namespace Kalendario.Application.ResourceModel
{
    public class AppointmentResourceModel : IMapFrom<Appointment>
    {
        public CustomerResourceModel Customer { get; set; }

        public EmployeeResourceModel Employee { get; set; }

        public ServiceResourceModel Service { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Price { get; set; }

        public string InternalNotes { get; set; }
    }
}