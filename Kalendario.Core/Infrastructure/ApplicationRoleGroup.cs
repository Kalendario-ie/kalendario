using System.Collections.Generic;
using Kalendario.Core.Entities;

namespace Kalendario.Core.Infrastructure;

public class ApplicationRoleGroup : AccountEntity
{
    public string Name { get; set; }
    public ICollection<ApplicationRole> Roles { get; set; }
    public ICollection<RoleGroupRole> RoleGroupRoles { get; set; }
}