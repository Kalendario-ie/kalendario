using System.Collections.Generic;

namespace Kalendario.Core.Domain
{
    public class Service : AccountEntity
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}