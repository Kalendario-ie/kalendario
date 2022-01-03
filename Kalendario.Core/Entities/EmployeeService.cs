using System;

namespace Kalendario.Core.Entities
{
    public class EmployeeService : AccountEntity
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public Guid ServiceId { get; set; }

        public Service Service { get; set; }
    }
}