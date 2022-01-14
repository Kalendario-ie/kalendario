using System.Collections.Generic;

namespace Kalendario.Core.Entities;

public class SchedulingPanel : AccountEntity
{
    public string Name { get; set; }

    public IEnumerable<Employee> Employees { get; set; }
}