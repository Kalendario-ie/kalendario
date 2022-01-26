using System;
using System.Collections.Generic;
using System.Linq;
using Kalendario.Application.Authorization;

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
        Roles = roles.Split(',').Select(role => AuthorizationHelper.RoleName(entity, role));
    
    public AuthorizeAttribute(string roles) => Roles = roles.Split(',');

    public IEnumerable<string> Roles { get; }
}