using System;
using System.Collections.Generic;
using Kalendario.Core.Common;

namespace Kalendario.Core.Entities
{
    public class Service : AccountEntity, ISoftDeletable
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public TimeSpan Duration { get; set; }

        public Guid? ServiceCategoryId { get; set; }
        
        public ServiceCategory ServiceCategory { get; set; } = null;
        
        public List<Employee> Employees { get; set; } = new();

        public List<EmployeeService> EmployeeServices { get; set; } = new();

        public List<Appointment> Appointments { get; set; } = new();
        public bool IsDeleted { get; set; }
    }
}