using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Core.Infrastructure;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public ApplicationRole()
    {
    }

    public ICollection<ApplicationRoleGroup> RoleGroups { get; set; }
    public ICollection<RoleGroupRole> RoleGroupRoles { get; set; }
}