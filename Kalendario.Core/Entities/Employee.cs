using System.Collections.Generic;

namespace Kalendario.Core.Entities
{
    public class Employee : AccountEntity
    {
        public string Name { get; set; }

        public ICollection<Service> Services { get; set; }
    }
}