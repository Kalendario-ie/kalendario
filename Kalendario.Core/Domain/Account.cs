using System;
using System.Collections.Generic;

namespace Kalendario.Core.Domain
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public string Avatar { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; }

        public ICollection<Service> Services { get; set; }
        
        public ICollection<Customer> Customers { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}