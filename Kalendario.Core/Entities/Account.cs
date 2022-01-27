using System.Collections.Generic;
using Kalendario.Core.Infrastructure;

namespace Kalendario.Core.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public string Avatar { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; }

        public ICollection<Service> Services { get; set; }
        public ICollection<ServiceCategory> ServiceCategories { get; set; }
        
        public ICollection<Customer> Customers { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        
        public ICollection<ApplicationUser> Users { get; set; }
    }
}