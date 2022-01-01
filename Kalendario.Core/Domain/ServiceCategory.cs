using System.Collections.Generic;
using Kalendario.Core.ValueObject;

namespace Kalendario.Core.Domain;

public class ServiceCategory : AccountEntity
{
    public string Name { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public List<Service> Services { get; set; } = new();

}