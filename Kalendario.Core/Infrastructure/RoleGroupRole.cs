using System;

namespace Kalendario.Core.Infrastructure;

public class RoleGroupRole
{
    public Guid Id { get; set; }
    public Guid RoleGroupId { get; set; }
    public ApplicationRoleGroup RoleGroup { get; set; }
    public string RoleId { get; set; }
    public ApplicationRole Role { get; set; }
}