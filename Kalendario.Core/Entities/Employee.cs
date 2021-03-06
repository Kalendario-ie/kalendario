using System;
using System.Collections.Generic;
using Kalendario.Core.Common;

namespace Kalendario.Core.Entities
{
    public class Employee : AccountEntity, ISoftDeletable
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string PhotoUrl { get; set; }

        public Guid? ScheduleId { get; set; }

        public Schedule Schedule { get; set; }
        public ICollection<Service> Services { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; }

        public ICollection<Appointment> Appointments { get; } = new List<Appointment>();
        public ICollection<SchedulingPanel> SchedulingPanels { get; } = new List<SchedulingPanel>();
        public bool IsDeleted { get; set; }
    }
}