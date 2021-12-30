using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalendario.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
    /// <param name="entity">The entity class type, must from Kalendario.Core.Domain.</param>
    /// <param name="roles">A comma delimited list of roles.</param>
    /// </summary>
    public AuthorizeAttribute(Type entity, string roles) =>
        Roles = roles.Split(',').Select(role => $"{entity.Name}_{role}");

    public IEnumerable<string> Roles { get; }
}