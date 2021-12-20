using System;

namespace Kalendario.Core.Domain
{
    public class EmployeeService
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public Guid ServiceId { get; set; }

        public Service Service { get; set; }
    }
}