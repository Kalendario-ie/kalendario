using System;
using System.Collections.Generic;
using System.Linq;
using Kalendario.Core.Common;

namespace Kalendario.Core.Entities
{
    public class Appointment : AccountEntity, IAuditable
    {
        public const string CanOverbookRole = "CanOverbookRole";

        public Guid? CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Guid EmployeeId { get; set; }
        
        public Employee Employee { get; set; }

        public Guid? ServiceId { get; set; }
        
        public Service Service { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Price { get; set; }

        public string InternalNotes { get; set; }
        
        public new static IEnumerable<string> EntityRoles()
        {
            return BaseEntity.EntityRoles().Append(CanOverbookRole);
        }
    }
}