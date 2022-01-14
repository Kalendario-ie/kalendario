using System;
using System.Collections.Generic;

namespace Kalendario.Core.Entities
{
    public class Employee : AccountEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? ScheduleId { get; set; }

        public Schedule Schedule { get; set; }
        public ICollection<Service> Services { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; }

        public ICollection<SchedulingPanel> SchedulingPanels { get; set; }
    }
}