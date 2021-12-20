using System;
using System.Collections.Generic;

namespace Kalendario.Core.Domain
{
    public class Employee : AccountEntity
    {
        public string Name { get; set; }

        public ICollection<Service> Services { get; set; }
    }
}