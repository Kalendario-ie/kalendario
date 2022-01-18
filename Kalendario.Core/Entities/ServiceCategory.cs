using System.Collections.Generic;
using Kalendario.Core.Common;
using Kalendario.Core.ValueObject;

namespace Kalendario.Core.Entities;

public class ServiceCategory : AccountEntity, ISoftDeletable
{
    public string Name { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public List<Service> Services { get; set; } = new();

    public bool IsDeleted { get; set; }
}