using System;
using Kalendario.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Kalendario.Core.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public Guid? AccountId { get; set; }
    public Account Account { get; set; }

    public Guid? RoleGroupId { get; set; }
    public ApplicationRoleGroup RoleGroup { get; set; }
    
    public Guid? EmployeeId { get; set; }

    public Employee Employee { get; set; }
}