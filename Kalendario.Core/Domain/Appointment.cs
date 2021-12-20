using System;

namespace Kalendario.Core.Domain
{
    public class Appointment : AccountEntity
    {

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid EmployeeId { get; set; }
        
        public Employee Employee { get; set; }

        public Guid ServiceId { get; set; }
        
        public Service Service { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Price { get; set; }

        public string InternalNotes { get; set; }
    }
}